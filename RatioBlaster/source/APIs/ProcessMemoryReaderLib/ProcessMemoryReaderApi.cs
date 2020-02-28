namespace ProcessMemoryReaderLib
{
    using System;
    using System.Runtime.InteropServices;

    internal class ProcessMemoryReaderApi
    {
        public const uint PROCESS_VM_READ = 0x10;

        [DllImport("kernel32.dll")]
        public static extern int CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern int ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, uint size, out IntPtr lpNumberOfBytesRead);
    }
}

