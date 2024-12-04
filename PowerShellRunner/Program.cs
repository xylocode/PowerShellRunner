using System;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Runtime.InteropServices;

namespace XyloCode.PowerShellRunner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            nint handle = Process.GetCurrentProcess().MainWindowHandle;
            SetConsoleMode(handle, ENABLE_EXTENDED_FLAGS);

            var iss = InitialSessionState.CreateDefault2();
            iss.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Bypass;
            var ps = PowerShell.Create(iss);

            var pwd = Directory.GetCurrentDirectory();
            var files = Directory.GetFiles(pwd, "*.ps1", SearchOption.TopDirectoryOnly);

            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    Console.WriteLine(">> ", file);
                    var script = File.ReadAllText(file);
                    var res = ps.AddScript(script).Invoke();

                    Console.WriteLine(">>");
                    foreach (var item in res)
                    {
                        Console.WriteLine(item.ToString());
                    }
                }
            }

            Console.WriteLine("=====---THE END---=====");
            Console.Beep();
            Console.ReadLine();
        }

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(nint hConsoleHandle, uint dwMode);
        private const uint ENABLE_EXTENDED_FLAGS = 0x0080;
    }
}
