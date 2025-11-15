using Microsoft.Extensions.Configuration;

namespace Services.Config
{
    public static class ConfigService
    {
        /// <summary>
        /// 配置文件相对路径
        /// </summary>
        private static readonly string ConfigPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                         "DataReceiverConfigs");

        /// <summary>
        /// 配置构建器
        /// </summary>
        private static IConfigurationRoot Configer = 
                        new ConfigurationBuilder()
                        .SetBasePath(ConfigPath)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .Build();

        /// <summary>
        /// 从Json 中载入Config
        /// </summary>
        /// <typeparam name="T">需要载入配置的类型</typeparam>
        /// <returns>配置的实例</returns>
        public static T Get<T>()
        {
            var config = Configer.GetSection(typeof(T).Name).Get<T>();
            return config == null ? throw new ArgumentNullException($"无法从配置文件中载入 {typeof(T).Name} 配置") : config;
        }

        /// <summary>
        /// 从Json 中载入Config 的对应项
        /// </summary>
        /// <returns>配置项的值</returns>
        public static string GetNode(string node)
        {
            var config = Configer.GetSection(node).Value;
            return config ?? throw new ArgumentNullException($"无法从配置文件中载入 {node} 配置");
        }
    }
}
