namespace ProcessMemoryReaderLib
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    public class ProcessMemoryReader
    {
        private IntPtr m_hProcess = IntPtr.Zero;
        private Process m_ReadProcess;

        public void CloseHandle()
        {
            if (ProcessMemoryReaderApi.CloseHandle(this.m_hProcess) == 0)
            {
                throw new Exception("CloseHandle failed");
            }
        }

        public void OpenProcess()
        {
            this.m_hProcess = ProcessMemoryReaderApi.OpenProcess(0x10, 1, (uint) this.m_ReadProcess.Id);
        }

        public byte[] ReadProcessMemory(IntPtr MemoryAddress, uint bytesToRead, out int bytesReaded)
        {
            IntPtr ptr;
            byte[] buffer = new byte[bytesToRead];
            ProcessMemoryReaderApi.ReadProcessMemory(this.m_hProcess, MemoryAddress, buffer, bytesToRead, out ptr);
            bytesReaded = ptr.ToInt32();
            return buffer;
        }

        public Process ReadProcess
        {
            get
            {
                return this.m_ReadProcess;
            }
            set
            {
                this.m_ReadProcess = value;
            }
        }
    }
}

