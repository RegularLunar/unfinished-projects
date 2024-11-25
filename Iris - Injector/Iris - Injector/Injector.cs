using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Iris___Injector
{
    internal class Injector
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadLibraryW(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public static void InjectDll(int processId, string dllPath)
        {
            IntPtr intPtr = Injector.OpenProcess(PROCESS_ALL_ACCESS, false, (uint)processId);
            if (intPtr == IntPtr.Zero)
            {
                int errorCode = Marshal.GetLastWin32Error();
                Console.Clear();
                Console.WriteLine("See https://learn.microsoft.com/en-us/windows/win32/debug/system-error-codes--0-499- for more details");
                throw new Exception($"Failed to open process. Error code: {errorCode}");
            }
            uint size = (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char)));
            IntPtr address = Injector.VirtualAllocEx(intPtr, IntPtr.Zero, size, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
            if (address == IntPtr.Zero)
            {
                int errorCode = Marshal.GetLastWin32Error();
                Console.Clear();
                Console.WriteLine("See https://learn.microsoft.com/en-us/windows/win32/debug/system-error-codes--0-499- for more details");
                throw new Exception($"Failed to allocate memory in the target process. Error code: {errorCode}");
            }
            byte[] buffer = Encoding.Unicode.GetBytes(dllPath);
            int lpNumberOfBytesWritten;
            if (!Injector.WriteProcessMemory(intPtr, address, buffer, (uint)buffer.Length, out lpNumberOfBytesWritten) || lpNumberOfBytesWritten != buffer.Length)
            {
                int errorCode = Marshal.GetLastWin32Error();
                Console.Clear();
                Console.WriteLine("See https://learn.microsoft.com/en-us/windows/win32/debug/system-error-codes--0-499- for more details");
                throw new Exception($"Failed to write memory in the target process. Error code: {errorCode}");
            }
            IntPtr moduleHandle = Injector.GetModuleHandle("kernel32.dll");
            if (moduleHandle == IntPtr.Zero)
            {
                int errorCode = Marshal.GetLastWin32Error();
                Console.Clear();
                Console.WriteLine("See https://learn.microsoft.com/en-us/windows/win32/debug/system-error-codes--0-499- for more details");
                throw new Exception($"Failed to get module handle for kernel32.dll. Error code: {errorCode}");
            }
            IntPtr loadLibraryAddr = Injector.GetProcAddress(moduleHandle, "LoadLibraryW");
            if (loadLibraryAddr == IntPtr.Zero)
            {
                int errorCode = Marshal.GetLastWin32Error();
                Console.Clear();
                Console.WriteLine("See https://learn.microsoft.com/en-us/windows/win32/debug/system-error-codes--0-499- for more details");
                throw new Exception($"Failed to get LoadLibraryW address. Error code: {errorCode}");
            }
            if (Injector.CreateRemoteThread(intPtr, IntPtr.Zero, 0, loadLibraryAddr, address, 0, IntPtr.Zero) == IntPtr.Zero)
            {
                int errorCode = Marshal.GetLastWin32Error();
                Console.Clear();
                Console.WriteLine("See https://learn.microsoft.com/en-us/windows/win32/debug/system-error-codes--0-499- for more details");
                throw new Exception($"Failed to create remote thread. Error code: {errorCode}");
            }
        }

        const uint PROCESS_CREATE_THREAD = 0x0002;
        const uint PROCESS_QUERY_INFORMATION = 0x0400;
        const uint PROCESS_VM_OPERATION = 0x0008;
        const uint PROCESS_VM_WRITE = 0x0020;
        const uint PROCESS_VM_READ = 0x0010;

        const uint MEM_COMMIT = 0x1000;
        const uint MEM_RESERVE = 0x2000;
        const uint PAGE_READWRITE = 0x04;
        const uint PROCESS_ALL_ACCESS = 0x001F0FFF;
    }
}
