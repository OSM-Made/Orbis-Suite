using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrbisLib2.Common;
using System;
using System.Windows;

namespace OrbisDebugger
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<MainWindow>();

            services.AddLogging(builder =>
            {
                builder.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = false;
                    options.SingleLine = true;
                    options.TimestampFormat = "HH:mm:ss ";
                });
            });

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var logger = _serviceProvider.GetService<ILoggerFactory>()
            .AddFile(Config.OrbisPath + @"\Logging\OrbisDebuggerLog.txt")
            .CreateLogger<MainWindow>();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show(logger);

            base.OnStartup(e);
        }
    }
}
