using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Security.Principal;
using System.Threading.Tasks;

namespace QuickLookBridgeApp.Service
{
    public class QuickLookService
    {
        private static readonly string PipeName = "QuickLook.App.Pipe." + WindowsIdentity.GetCurrent().User?.Value;
        public const string MessageType_Toggle = "QuickLook.App.PipeMessages.Toggle";

        public static async Task<bool> TrySendMessageAsync(string pipeMessage, string path, int timeout = 1000)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    using (var client = new NamedPipeClientStream(".", PipeName, PipeDirection.Out))
                    {
                        await client.ConnectAsync(timeout);
                        using (var writer = new StreamWriter(client))
                        {
                            await writer.WriteLineAsync($"{pipeMessage}|{path ?? string.Empty}");
                            await writer.FlushAsync();
                        }
                    }

                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"send message error\n:{e.StackTrace}");
                    return false;
                }
            });
        }
    }
}