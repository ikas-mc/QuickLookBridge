using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Windows.ApplicationModel.Activation;

namespace QuickLookBridgeApp.Service
{
    public class ActivationService
    {
        public const string URI_PREFIX = "quick-look://toggle/?file=";

        public static async Task OnActivatedByProtocol(IActivatedEventArgs args, StartupEventArgs e)
        {
            var result = ParseAndCheckFileAsync(args, e.Args, false, out var filePath);

            if (result)
            {
                Debug.WriteLine($"file:{filePath}");
                await QuickLookService.TrySendMessageAsync(QuickLookService.MessageType_Toggle, filePath);
            }
            //TODO 
            else
            {
                Debug.WriteLine("no file");
                await QuickLookService.TrySendMessageAsync(QuickLookService.MessageType_Toggle, string.Empty);
            }
        }

        public static bool ParseAndCheckFileAsync(IActivatedEventArgs eventArgs, string[] args, bool checkFile,
            out string filePath)
        {
            filePath = null != args && args.Any() ? args[0] : null;
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            if (filePath.StartsWith(URI_PREFIX))
            {
                filePath = ParseFilePathFromUriString(filePath);
            }

            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            return !checkFile || Directory.Exists(filePath) || File.Exists(filePath);
        }

        public static string BuildUriString(string filePath)
        {
            var param = Uri.EscapeDataString(filePath);
            return $"quick-look://toggle/?file={param}";
        }

        public static string ParseFilePathFromUriString(string uri)
        {
            var paramValue = uri.Substring(URI_PREFIX.Length);
            return Uri.UnescapeDataString(paramValue);
        }
    }
}