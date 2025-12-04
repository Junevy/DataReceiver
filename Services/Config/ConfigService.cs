using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Services.Config
{
    public static class ConfigService
    {
        /// <summary>
        /// 配置文件相对路径
        /// </summary>
        public static readonly string ConfigPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                         "DataReceiverConfigs");

        /// <summary>
        /// 配置构建器
        /// </summary>
        private static readonly IConfigurationRoot Configer = 
                        new ConfigurationBuilder()
                        .SetBasePath(ConfigPath)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .Build();
        public static IConfigurationRoot Configuration => Configer;


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

        /// <summary>
        /// 更新某个Section
        /// </summary>
        /// <typeparam name="T">需要更新的Section</typeparam>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static bool SaveSection<T>(T config, string sectionName)
        {
            try
            {
                var jsonPath = Path.Combine(ConfigPath, "appsettings.json");
                var jsonText = File.ReadAllText(jsonPath);

                var root = JsonNode.Parse(jsonText)!.AsObject();
                root[sectionName] = JsonNode.Parse(JsonSerializer.Serialize(config));
                File.WriteAllText(jsonPath, root.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
