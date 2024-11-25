using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Iris___Injector
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Console.Title = "Iris - Injector, x64 | By RegularLunar";
            try
            {
                Console.WriteLine("Select the executable file.");
                string processPath = Program.OpenFileBrowser("[ | ] Waiting for .EXE...", "Executable Files (*.exe)|*.exe");
                if (string.IsNullOrEmpty(processPath))
                {
                    Console.Clear();
                    Console.WriteLine("No executable selected. Exiting.");
                    Pause();
                    return;
                }
                string processName = Path.GetFileNameWithoutExtension(processPath);
                Console.Clear();
                Console.WriteLine("[ | ] Waiting for .DLL...");
                string dllPath = Program.OpenFileBrowser("Select the DLL file to inject", "DLL Files (*.dll)|*.dll");
                if (string.IsNullOrEmpty(dllPath))
                {
                    Console.Clear();
                    Console.WriteLine("No DLL selected. Exiting.");
                    Pause();
                    return;
                }
                int processId = Program.getProcessId(processName);
                if (processId != -1)
                {
                    Injector.InjectDll(processId, dllPath);
                    Console.Clear();
                    Console.WriteLine("DLL injection successful. :D");
                    Console.WriteLine("https://regularlunar.pages.dev");
                    Pause();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Process not found. Open the process before injecting. ;)");
                    Pause();
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("Error: " + ex.Message);
                Pause();
            }
        }

        private static string OpenFileBrowser(string title, string filter)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = title;
                ofd.Filter = filter;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return ofd.FileName;
                }
            }
            return null;
        }

        private static int getProcessId(string processName)
        {
            int result;
            try
            {
                Process[] processes = Process.GetProcessesByName(processName);
                if (processes.Length != 0)
                {
                    result = processes[0].Id;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("No process with the specified name was found.");
                    Pause();
                    result = -1;
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("Error retrieving process ID: " + ex.Message);
                Pause();
                result = -1;
            }

            return result;
        }
        private static void Pause()
        {
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
