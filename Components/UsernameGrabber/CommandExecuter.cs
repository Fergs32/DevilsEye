using System.Diagnostics;

namespace Dox.Components.UsernameGrabber
{
    public abstract class CommandExecuter
    {
        public static void ExecuteCommand(string command)
        {
            Process p = new();
            ProcessStartInfo startInfo = new()
            {
                FileName = "cmd.exe",
                Arguments = @"/c " + command // cmd.exe spesific implementation
            };
            p.StartInfo = startInfo;
            p.Start();
        }
    }
}