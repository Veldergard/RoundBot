using System;
using System.IO;
using Telegram.Bot;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TelegramBot
{
    class Program
    {
        static void Main() {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            IServiceCollection services = new ServiceCollection();
            ConfigureIoC(services, configuration);
            var serviceProvider = services.BuildServiceProvider();
            services.AddSingleton<IServiceProvider>(serviceProvider);
            var handler = serviceProvider.GetService<ProjectModerationBotService>()!;
            try
            {
                handler.Handle();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void ConfigureIoC(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("1839671942:AAFruLDYynRsOvIlpEnf4YIDsTAqdEok2CI"));
            services.AddSingleton(configuration);
            
            var connection = configuration.GetConnectionString("RoundDB");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connection));

            if (configuration.GetValue<bool>("UseAzureStorage"))
            {
                services.AddScoped(_ => new Func<string, IFileStorage>(container =>
                    new AzureFileStorage(configuration, container)));
                services.AddScoped<ITokenProvider, AzureTokenProvider>();
                
                services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"server"));
            }
            else
            {
                services.AddScoped(_ => new Func<string, IFileStorage>(_ =>
                    new LocalDirectoryFileStorage(configuration)));
                services.AddScoped<ITokenProvider, MockTokenProvider>();
            }

            services.AddSingleton<IFileStoragePath, FileStoragePath>();
            services.AddSingleton<IMediaFormatter, ImageMagickFormatter>();
            services.AddSingleton<ImagePreviewHandler>();
        }
    }
}

// using System;
// using System.Configuration;
// using System.IO;
// using System.Threading.Tasks;
// using Krujok.Online.Core.Interfaces;
// using Krujok.Online.Core.Services;
// using Krujok.Online.Data;
// using Krujok.Online.FileStorage.Azure.Services;
// using Krujok.Online.FileStorage.Interfaces;
// using Krujok.Online.FileStorage.LocalDirectory.Services;
// using Krujok.Online.Media.Interfaces;
// using Krujok.Online.Media.Services;
// using Microsoft.AspNetCore.DataProtection;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using log4net;
// using Microsoft.Extensions.Logging;
//
// namespace Krujok.Tools.MediaPreviewBuilder
// {
//     public static class Program
//     {
//         public static void Main(string[] args)
//         {
//             var configuration = new ConfigurationBuilder()
//                 .AddJsonFile("appsettings.json")
//                 .Build();
//             
//             IServiceCollection services = new ServiceCollection();
//
//             ConfigureIoC(services, configuration);
//
//             var serviceProvider = services.BuildServiceProvider();
//
//             services.AddSingleton<IServiceProvider>(serviceProvider);
//
//             var handler = serviceProvider.GetService<ImagePreviewHandler>()!;
//
//             var logger = LogManager.GetLogger(typeof(Program));
//
//             try
//             {
//                 handler.Handle();
//             }
//             catch (Exception e)
//             {
//                 logger.Error(e);
//             }
//         }
//
//         private static void ConfigureIoC(IServiceCollection services, IConfiguration configuration)
//         {
//             services.AddLogging(builder => builder.AddLog4Net("log4net.config"));
//             services.AddSingleton(configuration);
//             
//             var connection = configuration.GetConnectionString("DefaultConnection");
//             services.AddDbContext<ApplicationDbContext>(options =>
//                 options.UseNpgsql(connection));
//
//             if (configuration.GetValue<bool>("UseAzureStorage"))
//             {
//                 services.AddScoped(_ => new Func<string, IFileStorage>(container =>
//                     new AzureFileStorage(configuration, container)));
//                 services.AddScoped<ITokenProvider, AzureTokenProvider>();
//                 
//                 services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"server"));
//             }
//             else
//             {
//                 services.AddScoped(_ => new Func<string, IFileStorage>(_ =>
//                     new LocalDirectoryFileStorage(configuration)));
//                 services.AddScoped<ITokenProvider, MockTokenProvider>();
//             }
//
//             services.AddSingleton<IFileStoragePath, FileStoragePath>();
//             services.AddSingleton<IMediaFormatter, ImageMagickFormatter>();
//             services.AddSingleton<ImagePreviewHandler>();
//         }
//     }
// }