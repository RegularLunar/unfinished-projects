using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MemoryScanner
{
    public class MemScanner
    {
        private Process targetProcess;
        private IntPtr processHandle;
        public MemScanner(Process process)
        {
            targetProcess = process;
            processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, targetProcess.Id);
        }
        public byte[] ConvertStringToBytes(string byteString)
        {
            string[] elements = byteString.Split(' ');

            byte[] convertedBytes = new byte[elements.Length];

            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i].Contains("?"))
                {
                    convertedBytes[i] = 0x0;
                }
                else
                {
                    convertedBytes[i] = Convert.ToByte(elements[i], 16);
                }
            }
            return convertedBytes;
        }

        // not done
        public List<IntPtr> ScanMemory(string byteString)
        {
            List<IntPtr> results = new List<IntPtr>();
            IntPtr currentAddress = IntPtr.Zero;
            int bytesRead = 0;
            byte[] signatureByteArray = ConvertStringToBytes(byteString);
            while (VirtualQueryEx(processHandle, currentAddress, out MEMORY_BASIC_INFORMATION mbi, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))))
            {
                if (mbi.State == MEM_COMMIT && (mbi.Protect != PAGE_READWRITE || mbi.Protect != PAGE_READONLY))
                {
                    byte[] buffer = new byte[(int)mbi.RegionSize];
                    if (ReadProcessMemory(processHandle, mbi.BaseAddress, buffer, buffer.Length, out bytesRead))
                    {
                        for (int i = 0; i < bytesRead - signatureByteArray.Length; i++)
                        {
                            bool match = true;
                            for (int j = 0; j < signatureByteArray.Length; j++)
                            {
                                if (signatureByteArray[j] != 0 && buffer[i + j] != signatureByteArray[j])
                                {
                                    match = false;
                                    break;
                                }
                                if (match)
                                {
                                    results.Add(mbi.BaseAddress + i);
                                }
                            }
                        }
                    }
                    currentAddress = new IntPtr(currentAddress.ToInt64() + mbi.RegionSize.ToInt64());
                }

            }
            return results;
        }

        //
        private const uint PROCESS_ALL_ACCESS = 0x1F0FFF;
        private const uint MEM_COMMIT = 0x1000;
        private const uint PAGE_READONLY = 0x02;
        private const uint PAGE_READWRITE = 0x04;

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        static extern bool VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public IntPtr RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }
    }
}