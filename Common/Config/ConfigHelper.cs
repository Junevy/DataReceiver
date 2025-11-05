
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Common
{
    public static class ConfigHelper
    {
        /// <summary>
        /// Json 配置文件路径
        /// </summary>
        private static readonly string ConfigPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets\\Configs\\appsettings.json");

        /// <summary>
        /// 从Json 中载入Config
        /// </summary>
        /// <typeparam name="T">需要载入配置的类型</typeparam>
        /// <returns>配置的实例</returns>
        public static T Build<T>()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(ConfigPath, optional: false, reloadOnChange: true);

            var config = builder.Build().GetSection(typeof(T).Name).Get<T>();
            return config == null ? throw new ArgumentNullException($"无法从配置文件中载入 {typeof(T).Name} 配置") : config;
        }
    }
}
