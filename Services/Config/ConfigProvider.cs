using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Services.Config
{
    /// <summary>
    /// 废弃的方法。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConfigProvider<T> where T : class, new()
    {
        public static readonly string ConfigPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "DataReceiverConfigs");

        private readonly IConfigurationRoot root;

        private T config;
        public T Current => config;

        public ConfigProvider(string? configPath = null)
        {
            root = new ConfigurationBuilder()
                .SetBasePath(configPath ?? ConfigPath)
                .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            LoadConfig();

            ChangeToken.OnChange(() => root.GetReloadToken(), LoadConfig);
        }

        public void LoadConfig()
        {
            config = root.GetSection(typeof(T).Name).Get<T>() ?? new T();
        }
    }
}
