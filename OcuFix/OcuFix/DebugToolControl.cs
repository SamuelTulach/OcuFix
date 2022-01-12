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

        public enum ASWMode
        {
            Unknown,
            Disabled,
            Auto,
            Clock45,
            Sim45
        }

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

            process.WaitForExit(3000);

            // When Oculus service is not running
            // Oculus debug tool CLI will be printing out forever
            // "ERROR: Unable to start debug session, retrying..." 
            if (!process.HasExited)
                process.Kill();

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

        public static ASWMode GetASWMode()
        {
            var output = GetCommandOutput("server:asw.Mode");
            switch (output)
            {
                case "off":
                    return ASWMode.Disabled;
                case "auto":
                    return ASWMode.Auto;
                case "Clock45":
                    return ASWMode.Clock45;
                case "Sim45":
                    return ASWMode.Sim45;
                default:
                    return ASWMode.Unknown;
            }
        }

        private static bool CheckCommand(string command, string target)
        {
            var output = GetCommandOutput(command);
            return output == target;
        }

        public static bool SetASWMode(ASWMode mode)
        {
            switch (mode)
            {
                case ASWMode.Disabled:
                    return CheckCommand("server:asw.Off", "ASW operation disabled (off)");
                case ASWMode.Auto:
                    return CheckCommand("server:asw.Auto", "Auto ASW operation enabled");
                case ASWMode.Clock45:
                    return CheckCommand("server:asw.Clock45", "45 Hz rendering extrapolation enabled");
                case ASWMode.Sim45:
                    return CheckCommand("server:asw.Sim45", "45 Hz rendering simulation enabled");
                default:
                    return false;
            }
        }
    }
}
