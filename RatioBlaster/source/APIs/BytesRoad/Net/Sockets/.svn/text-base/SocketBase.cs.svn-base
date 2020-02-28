namespace BytesRoad.Net.Sockets
{
    using BytesRoad.Diag;
    using BytesRoad.Net.Sockets.Advanced;
    using System;
    using System.Net;
    using System.Net.Sockets;

    internal abstract class SocketBase : AsyncBase, IDisposable
    {
        private bool _disposed;
        private bool _preAuthenticate;
        protected byte[] _proxyPassword;
        protected int _proxyPort;
        protected string _proxyServer;
        protected byte[] _proxyUser;
        private static Random _rand = new Random((int) DateTime.Now.Ticks);
        protected Socket _socket;
        private NetworkStream _stream;

        protected SocketBase()
        {
            this._proxyPort = -1;
            this._preAuthenticate = true;
            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this._socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, 1);
        }

        protected SocketBase(Socket systemSocket)
        {
            this._proxyPort = -1;
            this._preAuthenticate = true;
            if (systemSocket == null)
            {
                throw new ArgumentNullException("systemSocket");
            }
            this._socket = systemSocket;
        }

        protected SocketBase(string proxyServer, int proxyPort, byte[] proxyUser, byte[] proxyPassword)
        {
            this._proxyPort = -1;
            this._preAuthenticate = true;
            this._proxyServer = proxyServer;
            this._proxyPort = proxyPort;
            this._proxyUser = proxyUser;
            this._proxyPassword = proxyPassword;
            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        internal abstract SocketBase Accept();
        internal abstract IAsyncResult BeginAccept(AsyncCallback callback, object state);
        internal abstract IAsyncResult BeginBind(SocketBase socket, AsyncCallback callback, object state);
        internal abstract IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state);
        internal abstract IAsyncResult BeginConnect(string hostName, int port, AsyncCallback callback, object state);
        internal static IAsyncResult BeginGetHostByName(string hostName, AsyncCallback cb, object state)
        {
            return Dns.BeginGetHostEntry(hostName, cb, state);
        }

        internal virtual IAsyncResult BeginReceive(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            this.CheckDisposed();
            return this._socket.BeginReceive(buffer, offset, size, SocketFlags.None, callback, state);
        }

        internal virtual IAsyncResult BeginSend(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            this.CheckDisposed();
            return this._socket.BeginSend(buffer, offset, size, SocketFlags.None, callback, state);
        }

        internal abstract void Bind(SocketBase socket);
        protected void CheckDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
        }

        internal void Close()
        {
            this.Dispose();
        }

        internal abstract void Connect(EndPoint remoteEP);
        internal virtual void Connect(string hostName, int port)
        {
            this.CheckDisposed();
            NSTrace.WriteLineInfo(string.Format("S: Resolving name '{0}'...", hostName));
            IPHostEntry hostByName = GetHostByName(hostName);
            if (hostByName == null)
            {
                NSTrace.WriteLineInfo("S: Hostname not found, throwing exception...");
                throw new SocketException(0x2af9);
            }
            NSTrace.WriteLineInfo("S: Hostname found, connecting ...");
            this.Connect(ConstructEndPoint(hostByName, port));
            NSTrace.WriteLineInfo("S: Connection established.");
        }

        internal static IPEndPoint ConstructEndPoint(IPHostEntry host, int port)
        {
            if (0 >= host.AddressList.Length)
            {
                NSTrace.WriteLineError("Provided host structure do not contains addresses.");
                throw new ArgumentException("Provided host structure do not contains addresses.", "host");
            }
            int index = 0;
            if (1 < host.AddressList.Length)
            {
                index = _rand.Next(host.AddressList.Length - 1);
            }
            return new IPEndPoint(host.AddressList[index], port);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                this._socket.Close();
            }
        }

        internal abstract SocketBase EndAccept(IAsyncResult asyncResult);
        internal abstract void EndBind(IAsyncResult ar);
        internal abstract void EndConnect(IAsyncResult asyncResult);
        internal static IPHostEntry EndGetHostByName(IAsyncResult ar)
        {
            try
            {
                return Dns.EndGetHostEntry(ar);
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal virtual int EndReceive(IAsyncResult asyncResult)
        {
            return this._socket.EndReceive(asyncResult);
        }

        internal virtual int EndSend(IAsyncResult asyncResult)
        {
            return this._socket.EndSend(asyncResult);
        }

        ~SocketBase()
        {
            this.Dispose(false);
        }

        internal static IPHostEntry GetHostByName(string hostName)
        {
            try
            {
                return Dns.Resolve(hostName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal abstract void Listen(int backlog);
        internal virtual int Receive(byte[] buffer)
        {
            this.CheckDisposed();
            return this._socket.Receive(buffer);
        }

        internal virtual int Receive(byte[] buffer, int size)
        {
            this.CheckDisposed();
            return this._socket.Receive(buffer, size, SocketFlags.None);
        }

        internal virtual int Receive(byte[] buffer, int offset, int size)
        {
            this.CheckDisposed();
            return this._socket.Receive(buffer, offset, size, SocketFlags.None);
        }

        internal virtual int Send(byte[] buffer)
        {
            this.CheckDisposed();
            return this._socket.Send(buffer);
        }

        internal virtual int Send(byte[] buffer, int size)
        {
            this.CheckDisposed();
            return this._socket.Send(buffer, size, SocketFlags.None);
        }

        internal virtual int Send(byte[] buffer, int offset, int size)
        {
            this.CheckDisposed();
            return this._socket.Send(buffer, offset, size, SocketFlags.None);
        }

        internal virtual void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionValue)
        {
            this.CheckDisposed();
            this._socket.SetSocketOption(optionLevel, optionName, optionValue);
        }

        internal virtual void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
        {
            this.CheckDisposed();
            this._socket.SetSocketOption(optionLevel, optionName, optionValue);
        }

        internal virtual void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, object optionValue)
        {
            this.CheckDisposed();
            this._socket.SetSocketOption(optionLevel, optionName, optionValue);
        }

        internal virtual void Shutdown(SocketShutdown how)
        {
            this.CheckDisposed();
            this._socket.Shutdown(how);
        }

        internal virtual int Available
        {
            get
            {
                return this._socket.Available;
            }
        }

        internal bool Connected
        {
            get
            {
                return this._socket.Connected;
            }
        }

        internal abstract EndPoint LocalEndPoint { get; }

        protected NetworkStream NStream
        {
            get
            {
                if (this._stream == null)
                {
                    this._stream = new NetworkStream(this._socket, false);
                }
                return this._stream;
            }
        }

        internal bool PreAuthenticate
        {
            get
            {
                return this._preAuthenticate;
            }
            set
            {
                this._preAuthenticate = value;
            }
        }

        internal abstract BytesRoad.Net.Sockets.ProxyType ProxyType { get; }

        internal abstract EndPoint RemoteEndPoint { get; }

        internal Socket SystemSocket
        {
            get
            {
                return this._socket;
            }
        }
    }
}

