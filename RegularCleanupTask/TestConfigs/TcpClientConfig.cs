namespace RegularCleanupTask.TestConfigs
{
    public class TcpClientConfig
    {
        public string Ip { get; set; }
        public int Port { get; set; }

        public TimeSpan Time = TimeSpan.FromSeconds(30);
        public bool Enable = true;

    }
}
