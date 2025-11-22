using DataReceiver.Models.Socket.Base;
using DataReceiver.Models.Socket.Common;
using FubarDev.FtpServer;
using FubarDev.FtpServer.AccountManagement;
using FubarDev.FtpServer.FileSystem.DotNet;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

using FtpServerConfig = DataReceiver.Models.Socket.Config.FtpServerConfig;

namespace DataReceiver.Models.Socket.FTP
{
    public class FtpServerModel : ConnectionBase<IFtpServer, FtpServerConfig>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FtpServerModel));

        //public IFtpServer Socket { get; set; } = server;
        //private readonly ConfigProvider<FtpServerConfig> _configProvider = configProvider;

        public FtpServerModel(FtpServerConfig config, IFtpServer server): base(config)
        {
            Socket = server;
        }


        public override async Task<ConnectionState> ConnectAsync(CancellationToken ct = default)
        {
            Log.Info("Starting config and open FTP Server.");
            if (Runtimes.State == ConnectionState.Connected
                || Runtimes.State == ConnectionState.Connecting
                || Runtimes.State == ConnectionState.Error)
                await DisconnectAsync();

            if (!Directory.Exists(Config.RootPath))
                Directory.CreateDirectory(Config.RootPath);

            if (Cts is null || Cts.IsCancellationRequested)
                Cts = new();
            
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
            try { Socket.StopAsync(Cts.Token); } catch (Exception e) { Log.Warn($"Clean the Socket error :{e.Message} "); }
            try
            {
                if (Cts is not null && !Cts.IsCancellationRequested)
                {
                    Cts.Cancel();
                    Cts.Dispose();
                }

                //Server?.Dispose();
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
