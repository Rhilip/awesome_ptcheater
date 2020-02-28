namespace rm2
{
    using BytesRoad.Net.Sockets;
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct ProxyInfo
    {
        public ProxyType proxyType;
        public string proxyServer;
        public int proxyPort;
        public byte[] proxyUser;
        public byte[] proxyPassword;
    }
}

