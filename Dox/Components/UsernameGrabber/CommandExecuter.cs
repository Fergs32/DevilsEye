using System.Diagnostics;

namespace Dox.Components.UsernameGrabber
{
    public class CommandExecuter
    {
        public static void ExecuteCommand(string command)
        {
            Process p = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = @"/c " + command; // cmd.exe spesific implementation
            p.StartInfo = startInfo;
            p.Start();
        }
    }
}