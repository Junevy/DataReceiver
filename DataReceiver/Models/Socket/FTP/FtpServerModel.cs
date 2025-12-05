using DataReceiver.Models.Socket.Base;
using DataReceiver.Models.Socket.Common;
using FubarDev.FtpServer;
using HandyControl.Controls;
using log4net;
using System.IO;
using FtpServerConfig = DataReceiver.Models.Socket.Config.FtpServerConfig;

namespace DataReceiver.Models.Socket.FTP
{
    public class FtpServerModel : ConnectionBase<IFtpServer, FtpServerConfig>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FtpServerModel));
        public FtpServerModel(FtpServerConfig config, IFtpServer server) : base(config)
        {
            Socket = server;
            
        }

        /// <summary>
        /// Start and connect the FTP server.
        /// </summary>
        /// <param name="ct">Token</param>
        /// <returns>States of the connection.</returns>
        /// <exception cref="InvalidOperationException"></exception>
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
                //Socket Paused.
                if (Socket.Status == FtpServiceStatus.Paused)
                    await Socket.ContinueAsync(Cts.Token);

                // Socket waiting to start.
                else if (Socket.Status == FtpServiceStatus.ReadyToRun)
                    await Socket.StartAsync(Cts.Token);

                else
                {
                    Log.Error("Ftp server status error : it's not Paused or ReadtoRun!");
                    throw new InvalidOperationException("Ftp server status error : it's not Paused or ReadtoRun!");
                }

            }
            catch (OperationCanceledException)
            {
                Log.Warn("FTP task canceled!");
                OnStateUpdated(ConnectionState.Disconnected, Runtimes.State, $"FTP Server canceled.");
                return ConnectionState.Disconnected;
            }
            catch (Exception e)
            {
                OnStateUpdated(ConnectionState.Disconnected, Runtimes.State, $"Start FTP Server error : {e.Message}");
                Log.Error(e);
                return ConnectionState.Disconnected;
            }

            OnStateUpdated(ConnectionState.Connected, Runtimes.State, "Start FTP Server successful!");
            Log.Info("Start FTP Server successful!");
            return ConnectionState.Connected;
        }


        /// <summary>
        /// Pause the FTP server.
        /// </summary>
        /// <returns></returns>
        public override async Task DisconnectAsync()
        {
            Log.Info("Waiting for clean the FTP server.");
            try
            {
                Socket?.PauseAsync(
                    (Cts == null || Cts.IsCancellationRequested)
                    ? CancellationToken.None
                    : Cts.Token);
            }
            catch (Exception e) { Log.Warn($"Clean the Socket error :{e.Message} "); }
            OnStateUpdated(ConnectionState.Disconnected, Runtimes.State, "FTP Server disconnected!");
        }

        /// <summary>
        /// Dispose the FTP server. When dispoed, the server can not be restart again.
        /// </summary>
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
