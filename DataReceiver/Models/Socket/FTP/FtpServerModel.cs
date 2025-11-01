using DataReceiver.Models.Socket.Base;
using DataReceiver.Models.Socket.Common;
using FubarDev.FtpServer;
using FubarDev.FtpServer.AccountManagement;
using FubarDev.FtpServer.FileSystem.DotNet;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using FubarDev.FtpServer.Events;

using FtpServerConfig = DataReceiver.Models.Socket.Config.FtpServerConfig;

namespace DataReceiver.Models.Socket.FTP
{
    public class FtpServerModel(FtpServerConfig config) : ConnectionBase<IFtpServer, FtpServerConfig>(config)
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FtpServerModel));
        private ServiceProvider services;

        public override async Task<ConnectionState> ConnectAsync(CancellationToken ct = default)
        {
            Log.Info("Starting config and open FTP Server.");
            if (Runtimes.State == ConnectionState.Connected
                || Runtimes.State == ConnectionState.Connecting
                || Runtimes.State == ConnectionState.Error)
                await DisconnectAsync();

            if (!Directory.Exists(Config.RootPath))
                Directory.CreateDirectory(Config.RootPath);

            var container = new ServiceCollection();

            container.AddFtpServer(builder =>
            {
                builder.UseDotNetFileSystem();

                if (Config.AllowAnonymous)
                    builder.EnableAnonymousAuthentication(); // allow anonymous logins
            });

            // Configure the FTP server
            container.Configure<FtpServerOptions>(opt =>
                {
                    opt.ServerAddress = Config.Ip;
                    opt.Port = Config.Port;
                    opt.MaxActiveConnections = Config.MaxConnections;
                    opt.ConnectionInactivityCheckInterval = Config.InactiveCheckInterval;
                });

            // use Config.RootPath as root folder
            container.Configure<DotNetFileSystemOptions>(opt =>
            {
                opt.RootPath = Config.RootPath;
                opt.FlushAfterWrite = true;  // 写入后立即刷新
                opt.StreamBufferSize = Config.BufferSize * 100;
            });

            // Config connection parameters
            container.Configure<FtpConnectionOptions>(opt =>
            {
                //opt.DefaultEncoding = Config.Encoding;
                opt.DefaultEncoding = System.Text.Encoding.UTF8;
                opt.InactivityTimeout = Config.InactiveTimeOut;
            });

            // Authentication
            container.AddSingleton<IMembershipProvider>(

                new UserShipProvider(Config.UserName, Config.Password)
            );

            //container.AddSingleton<IFtpConnectionEvent, MyFtpEventListener>();
            //FtpConnectionDataTransferStoppedEvent
            //    .Register<CustomFtpEventListener>(container);

            //container.Configure<PasvOptions>(opt =>
            //{
            //    opt.PasvMinPort = _config.PasvMinPort;
            //    opt.PasvMaxPort = _config.PasvMaxPort;
            //});

            //container.AddLogging();

            services = container.BuildServiceProvider();
            Socket = services.GetRequiredService<IFtpServer>();
            try
            {
                await Socket.StartAsync(Cts.Token);
            }
            catch (OperationCanceledException) { Log.Warn("FTP task canceled!"); }
            catch (Exception e) 
            {
                OnStateUpdated(ConnectionState.Disconnected, Runtimes.State, $"Start FTP Server error : {e.Message}");
                Log.Error(e); 
            }
            OnStateUpdated(ConnectionState.Connected, Runtimes.State, "Start FTP Server successful!");
            Log.Info("Start FTP Server successful!");
            return ConnectionState.Connected;
        }

        public override async Task DisconnectAsync()
        {
            Log.Info("Waiting for clean the FTP server.");
            try { Socket?.StopAsync(Cts?.Token ?? CancellationToken.None); } catch (Exception e) { Log.Warn($"Clean the Socket error :{e.Message} "); }
            try
            {
                if (Cts is not null && !Cts.IsCancellationRequested)
                {
                    Cts.Cancel();
                    Cts.Dispose();
                }

                services?.Dispose();
            }
            catch (Exception e) { Log.Warn($"Clean the FTP server error :{e.Message} "); }
            OnStateUpdated(ConnectionState.Disconnected, Runtimes.State, "FTP Server disconnected!");
        }

        public override void Dispose()
        {
            Log.Warn("Disposing the object.");
            DisconnectAsync().GetAwaiter().GetResult();
            base.Dispose();
        }

        public override Task<int> SendAsync(byte[] data, CancellationToken ct = default)
        {
            throw new NotImplementedException("The method was discarded.");
        }
    }
}
