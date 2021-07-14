using System;
using System.Windows;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using QuickLookBridgeApp.Service;

namespace QuickLookBridgeApp
{
    public partial class App : Application
    {
        private async void ApplicationStartup(object sender, StartupEventArgs e)
        {
            var args = AppInstance.GetActivatedEventArgs();
            if (args.Kind == ActivationKind.Protocol)
            {
                await ActivationService.OnActivatedByProtocol(args, e);
                Current.Shutdown();
            }

            StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
        }
    }
}