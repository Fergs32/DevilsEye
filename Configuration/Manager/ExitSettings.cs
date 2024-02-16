using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Dox.Configuration.Manager
{
    internal class ExitSettings
    {
        /*
         * 
         * This will handle all the exit events for the program (hopefully) and will be called when the user presses the exit button
         */
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public static IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;

        public static void ExitEventHandler(IntPtr handle)
        {
            Console.Write($"[{DateTime.Now:h:mm:ss tt}] ", Color.Magenta); Console.Write("Registered Internal ExitHandler");
            // TODO: Implement Exit Handler

        }


    }
}
