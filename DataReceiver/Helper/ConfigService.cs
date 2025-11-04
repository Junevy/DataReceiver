
//using System.IO;

//namespace DataReceiver.Helper
//{
//    public class ConfigService
//    {
//        /// <summary>
//        /// Json 配置文件路径
//        /// </summary>
//        private static readonly string ConfigPath =
//            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestConfigs\\appsettings.json");

//        /// <summary>
//        /// 从Json 中载入Config
//        /// </summary>
//        /// <typeparam name="T">需要载入配置的类型</typeparam>
//        /// <returns>配置的实例</returns>
//        public static T Build<T>()
//        {
//            var builder = new ConfigurationBuilder()
//                .SetBasePath(AppContext.BaseDirectory)
//                .AddJsonFile(ConfigPath, optional: false, reloadOnChange: true);

//            return builder.Build().GetSection(typeof(T).Name).Get<T>();
//        }
//    }
//}
