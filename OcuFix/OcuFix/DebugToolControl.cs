using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OcuFix
{
    internal static class DebugToolControl
    {
        private const string CommandTempFile = "command_temp.txt";
        private const string OutputFile = "output_temp.txt";

        private static string GetCommandOutput(string command)
        {
            // Executing the process directly and using standard output
            // redirection causes console window to pop in and out of existence
            // Unfortunately Oculus developers have never heard about standard input/output
            // and they are using the console handle directly
            // Furthermore, the BSIPA logs would get spammed by the debug tool output
            // so this "hacky" solution is probably the best way to do it

            if (File.Exists(CommandTempFile))
                File.Delete(CommandTempFile);

            if (File.Exists(OutputFile))
                File.Delete(OutputFile);

            File.WriteAllText(CommandTempFile, command + "\r\nexit\r\n");

            var debugToolPath = Configuration.PluginConfig.Instance.DebugToolPath;
            if (!File.Exists(debugToolPath))
                throw new Exception("Debug tool path is invalid!");

            var process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            process.StartInfo.Arguments = $"/C call \"{debugToolPath}\" -f \"{CommandTempFile}\" > {OutputFile} 2>&1";
            process.Start();

            Thread.Sleep(1000);

            process.WaitForExit();

            var output = File.ReadAllText(OutputFile);

            // For example:
            // => server:asw.Mode\r\noff\r\n=> exit\r\n
            var commandStart = output.LastIndexOf(command) + command.Length + 2;
            var afterCommand = output.Substring(commandStart);
            var outputOnly = afterCommand.Substring(0, afterCommand.IndexOf("\r\n"));

            File.Delete(CommandTempFile);
            File.Delete(OutputFile);

            return outputOnly;
        }

        public static bool ASWEnabled()
        {
            var output = GetCommandOutput("server:asw.Mode");
            return output != "off";
        }

        public static bool DisableASW()
        {
            var output = GetCommandOutput("server:asw.Off");
            return output == "ASW operation disabled (off)";
        }
    }
}
