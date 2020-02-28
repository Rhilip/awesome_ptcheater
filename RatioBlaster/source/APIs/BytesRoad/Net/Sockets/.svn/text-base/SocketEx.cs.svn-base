namespace BytesRoad.Net.Sockets
{
    using BytesRoad.Diag;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    public class SocketEx : IDisposable
    {
        private int _acceptTimeout;
        private SocketBase _baseSocket;
        private int _connectTimeout;
        private bool _disposed;
        private OpState _opState;
        private int _recvTimeout;
        private int _sendTimeout;
        private System.Threading.Timer _timer;

        public SocketEx()
        {
            this._opState = OpState.Finished;
            this._recvTimeout = -1;
            this._sendTimeout = -1;
            this._acceptTimeout = -1;
            this._connectTimeout = -1;
            NSTrace.WriteLineVerbose("-> SocketEx()");
            this._baseSocket = new Socket_None();
            this.Init();
            NSTrace.WriteLineVerbose("<- SocketEx()");
        }

        internal SocketEx(SocketBase baseSocket)
        {
            this._opState = OpState.Finished;
            this._recvTimeout = -1;
            this._sendTimeout = -1;
            this._acceptTimeout = -1;
            this._connectTimeout = -1;
            NSTrace.WriteLineVerbose("-> SocketEx(handle)");
            if (baseSocket == null)
            {
                NSTrace.WriteLineError("EX: SocketEx(handle), handle == null. " + Environment.StackTrace);
                throw new ArgumentNullException("baseSocket");
            }
            this._baseSocket = baseSocket;
            this.Init();
            NSTrace.WriteLineVerbose("<- SocketEx(handle)");
        }

        public SocketEx(BytesRoad.Net.Sockets.ProxyType proxyType, string proxyServer, int proxyPort, byte[] proxyUser, byte[] proxyPassword)
        {
            this._opState = OpState.Finished;
            this._recvTimeout = -1;
            this._sendTimeout = -1;
            this._acceptTimeout = -1;
            this._connectTimeout = -1;
            NSTrace.WriteLineVerbose("-> SocketEx(full)");
            if (proxyType == BytesRoad.Net.Sockets.ProxyType.None)
            {
                this._baseSocket = new Socket_None();
            }
            else if (BytesRoad.Net.Sockets.ProxyType.Socks4 == proxyType)
            {
                this._baseSocket = new Socket_Socks4(proxyServer, proxyPort, proxyUser);
            }
            else if (BytesRoad.Net.Sockets.ProxyType.Socks4a == proxyType)
            {
                this._baseSocket = new Socket_Socks4a(proxyServer, proxyPort, proxyUser);
            }
            else if (BytesRoad.Net.Sockets.ProxyType.Socks5 == proxyType)
            {
                this._baseSocket = new Socket_Socks5(proxyServer, proxyPort, proxyUser, proxyPassword);
            }
            else if (BytesRoad.Net.Sockets.ProxyType.HttpConnect == proxyType)
            {
                this._baseSocket = new Socket_HttpConnect(proxyServer, proxyPort, proxyUser, proxyPassword);
            }
            else
            {
                string message = string.Format("Proxy type is not supported ({0}).", proxyType.ToString());
                NSTrace.WriteLineError("EX: " + message + " " + Environment.StackTrace);
                throw new NotSupportedException(message);
            }
            this.Init();
            NSTrace.WriteLineVerbose("<- SocketEx(full)");
        }

        public SocketEx Accept()
        {
            return (SocketEx) this.DoTimeoutOp(this._acceptTimeout, new Accept_Op(this._baseSocket));
        }

        public IAsyncResult BeginAccept(AsyncCallback callback, object state)
        {
            return (IAsyncResult) this.BeginTimeoutOp(this._acceptTimeout, new Accept_Op(this._baseSocket), callback, state);
        }

        public IAsyncResult BeginBind(SocketEx socket, AsyncCallback callback, object state)
        {
            return (IAsyncResult) this.BeginTimeoutOp(this._connectTimeout, new Bind_Op(this._baseSocket, socket), callback, state);
        }

        public IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state)
        {
            return (IAsyncResult) this.BeginTimeoutOp(this._connectTimeout, new ConnectOp(this._baseSocket, remoteEP), callback, state);
        }

        public IAsyncResult BeginConnect(string hostName, int port, AsyncCallback callback, object state)
        {
            return (IAsyncResult) this.BeginTimeoutOp(this._connectTimeout, new ConnectOp(this._baseSocket, hostName, port), callback, state);
        }

        public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            return (IAsyncResult) this.BeginTimeoutOp(this._recvTimeout, new Receive_Op(this._baseSocket, buffer, offset, size), callback, state);
        }

        public IAsyncResult BeginSend(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            return (IAsyncResult) this.BeginTimeoutOp(this._sendTimeout, new Send_Op(this._baseSocket, buffer, offset, size), callback, state);
        }

        private object BeginTimeoutOp(int timeout, IOp op, AsyncCallback cb, object state)
        {
            this.CheckDisposed();
            object obj2 = null;
            this.StartTimeoutTrack(timeout);
            try
            {
                obj2 = op.BeginExecute(cb, state);
            }
            catch (Exception exception)
            {
                NSTrace.WriteLineError("SocketEx.B (ex): " + exception.ToString());
                this.StopTimeoutTrack(exception);
                throw;
            }
            catch
            {
                NSTrace.WriteLineError("SocketEx.B (non cls ex): " + Environment.StackTrace);
                this.StopTimeoutTrack(this.NonCLSException);
                throw;
            }
            return obj2;
        }

        public void Bind(SocketEx socket)
        {
            this.DoTimeoutOp(this._connectTimeout, new Bind_Op(this._baseSocket, socket));
        }

        private void CheckDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName, "Object disposed.");
            }
        }

        public void Close()
        {
            this.Dispose();
        }

        public void Connect(EndPoint remoteEP)
        {
            this.DoTimeoutOp(this._connectTimeout, new ConnectOp(this._baseSocket, remoteEP));
        }

        public void Connect(string hostName, int hostPort)
        {
            this.DoTimeoutOp(this._connectTimeout, new ConnectOp(this._baseSocket, hostName, hostPort));
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
                if (!this._disposed)
                {
                    this._baseSocket.Dispose();
                    this._timer.Dispose();
                    this._disposed = true;
                }
            }
        }

        private object DoTimeoutOp(int timeout, IOp op)
        {
            this.CheckDisposed();
            object obj2 = null;
            this.StartTimeoutTrack(timeout);
            try
            {
                obj2 = op.Execute();
            }
            catch (Exception exception)
            {
                NSTrace.WriteLineError("SocketEx (ex): " + exception.ToString());
                this.StopTimeoutTrack(exception);
                throw;
            }
            catch
            {
                NSTrace.WriteLineError("SocketEx (non clas ex): " + Environment.StackTrace);
                this.StopTimeoutTrack(this.NonCLSException);
                throw;
            }
            this.StopTimeoutTrack(null);
            return obj2;
        }

        public SocketEx EndAccept(IAsyncResult asyncResult)
        {
            return (SocketEx) this.EndTimeoutOp(new Accept_Op(this._baseSocket), asyncResult);
        }

        public void EndBind(IAsyncResult asyncResult)
        {
            this.EndTimeoutOp(new Bind_Op(this._baseSocket), asyncResult);
        }

        public void EndConnect(IAsyncResult asyncResult)
        {
            this.EndTimeoutOp(new ConnectOp(this._baseSocket), asyncResult);
        }

        public int EndReceive(IAsyncResult asyncResult)
        {
            return (int) this.EndTimeoutOp(new Receive_Op(this._baseSocket), asyncResult);
        }

        public int EndSend(IAsyncResult asyncResult)
        {
            return (int) this.EndTimeoutOp(new Send_Op(this._baseSocket), asyncResult);
        }

        private object EndTimeoutOp(IOp op, IAsyncResult ar)
        {
            object obj2 = null;
            try
            {
                obj2 = op.EndExecute(ar);
            }
            catch (Exception exception)
            {
                NSTrace.WriteLineError("SocketEx.E (ex): " + exception.ToString());
                this.StopTimeoutTrack(exception);
                throw;
            }
            catch
            {
                NSTrace.WriteLineError("SocketEx.E (Non CLS ex): " + Environment.StackTrace);
                this.StopTimeoutTrack(this.NonCLSException);
                throw;
            }
            this.StopTimeoutTrack(null);
            return obj2;
        }

        ~SocketEx()
        {
            this.Dispose(false);
        }

        private int GetTimeoutValue(int val, string propName)
        {
            if ((val < 0) && (-1 != val))
            {
                throw new ArgumentOutOfRangeException(propName, val, "Timeout value should not be less then zero (exception is only Timeout.Infinite");
            }
            if (val == 0)
            {
                return -1;
            }
            return val;
        }

        private void Init()
        {
            this._timer = new System.Threading.Timer(new TimerCallback(this.OnTimer), null, -1, -1);
        }

        public void Listen(int backlog)
        {
            this.DoTimeoutOp(this._connectTimeout, new Listen_Op(this._baseSocket, backlog));
        }

        private void OnTimer(object state)
        {
            lock (this)
            {
                if (!this._disposed)
                {
                    this._timer.Change(-1, -1);
                    if (this._opState == OpState.Working)
                    {
                        this._opState = OpState.Timedout;
                        this.Dispose();
                    }
                }
            }
        }

        public int Receive(byte[] buffer)
        {
            return (int) this.DoTimeoutOp(this._recvTimeout, new Receive_Op(this._baseSocket, buffer, 0, buffer.Length));
        }

        public int Receive(byte[] buffer, int size)
        {
            return (int) this.DoTimeoutOp(this._recvTimeout, new Receive_Op(this._baseSocket, buffer, 0, size));
        }

        public int Receive(byte[] buffer, int offset, int size)
        {
            return (int) this.DoTimeoutOp(this._recvTimeout, new Receive_Op(this._baseSocket, buffer, offset, size));
        }

        public int Send(byte[] buffer)
        {
            return (int) this.DoTimeoutOp(this._sendTimeout, new Send_Op(this._baseSocket, buffer, 0, buffer.Length));
        }

        public int Send(byte[] buffer, int size)
        {
            return (int) this.DoTimeoutOp(this._sendTimeout, new Send_Op(this._baseSocket, buffer, 0, size));
        }

        public int Send(byte[] buffer, int offset, int size)
        {
            return (int) this.DoTimeoutOp(this._sendTimeout, new Send_Op(this._baseSocket, buffer, offset, size));
        }

        internal void SetReceiveTimeout(int timeout)
        {
            this.ReceiveTimeout = timeout;
        }

        internal void SetSendTimeout(int timeout)
        {
            this.SendTimeout = timeout;
        }

        internal void SetTimeout(int timeout)
        {
            this.ConnectTimeout = timeout;
            this.AcceptTimeout = timeout;
            this.ReceiveTimeout = timeout;
            this.SendTimeout = timeout;
        }

        public void Shutdown(SocketShutdown how)
        {
            this._baseSocket.Shutdown(how);
        }

        private void StartTimeoutTrack(int timeout)
        {
            this._opState = OpState.Working;
            this._timer.Change(timeout, -1);
        }

        private void StopTimeoutTrack(Exception e)
        {
            lock (this)
            {
                if (this._opState == OpState.Timedout)
                {
                    throw new SocketException(0x274c);
                }
                this.CheckDisposed();
                this._opState = OpState.Finished;
                this._timer.Change(-1, -1);
            }
        }

        public int AcceptTimeout
        {
            get
            {
                this.CheckDisposed();
                return this._acceptTimeout;
            }
            set
            {
                this.CheckDisposed();
                this._acceptTimeout = this.GetTimeoutValue(value, "AcceptTimeout");
            }
        }

        public int Available
        {
            get
            {
                this.CheckDisposed();
                return this._baseSocket.Available;
            }
        }

        public bool Connected
        {
            get
            {
                this.CheckDisposed();
                return this._baseSocket.Connected;
            }
        }

        public int ConnectTimeout
        {
            get
            {
                this.CheckDisposed();
                return this._connectTimeout;
            }
            set
            {
                this.CheckDisposed();
                this._connectTimeout = this.GetTimeoutValue(value, "ConnectTimeout");
            }
        }

        public EndPoint LocalEndPoint
        {
            get
            {
                this.CheckDisposed();
                return this._baseSocket.LocalEndPoint;
            }
        }

        private Exception NonCLSException
        {
            get
            {
                return new Exception("Non CLS-compliant exception was thrown.");
            }
        }

        public bool PreAuthenticate
        {
            get
            {
                this.CheckDisposed();
                return this._baseSocket.PreAuthenticate;
            }
            set
            {
                this.CheckDisposed();
                this._baseSocket.PreAuthenticate = value;
            }
        }

        public BytesRoad.Net.Sockets.ProxyType ProxyType
        {
            get
            {
                this.CheckDisposed();
                return this._baseSocket.ProxyType;
            }
        }

        public int ReceiveTimeout
        {
            get
            {
                this.CheckDisposed();
                return this._recvTimeout;
            }
            set
            {
                this.CheckDisposed();
                this._recvTimeout = this.GetTimeoutValue(value, "ReceiveTimeout");
            }
        }

        public EndPoint RemoteEndPoint
        {
            get
            {
                this.CheckDisposed();
                return this._baseSocket.RemoteEndPoint;
            }
        }

        public int SendTimeout
        {
            get
            {
                this.CheckDisposed();
                return this._sendTimeout;
            }
            set
            {
                this.CheckDisposed();
                this._sendTimeout = this.GetTimeoutValue(value, "SendTimeout");
            }
        }

        public Socket SystemSocket
        {
            get
            {
                this.CheckDisposed();
                return this._baseSocket.SystemSocket;
            }
        }

        private class Accept_Op : IOp
        {
            private SocketBase _baseSocket;

            internal Accept_Op(SocketBase baseSocket)
            {
                this._baseSocket = baseSocket;
            }

            internal override object BeginExecute(AsyncCallback cb, object state)
            {
                return this._baseSocket.BeginAccept(cb, state);
            }

            internal override object EndExecute(IAsyncResult ar)
            {
                return new SocketEx(this._baseSocket.EndAccept(ar));
            }

            internal override object Execute()
            {
                return new SocketEx(this._baseSocket.Accept());
            }
        }

        private class Bind_Op : IOp
        {
            private SocketBase _baseSocket;
            private SocketEx _primConnSock;

            internal Bind_Op(SocketBase baseSocket)
            {
                this._baseSocket = baseSocket;
                this._primConnSock = null;
            }

            internal Bind_Op(SocketBase baseSocket, SocketEx primConnSock)
            {
                this._baseSocket = baseSocket;
                this._primConnSock = primConnSock;
            }

            internal override object BeginExecute(AsyncCallback cb, object state)
            {
                return this._baseSocket.BeginBind(this._primConnSock._baseSocket, cb, state);
            }

            internal override object EndExecute(IAsyncResult ar)
            {
                this._baseSocket.EndBind(ar);
                return null;
            }

            internal override object Execute()
            {
                this._baseSocket.Bind(this._primConnSock._baseSocket);
                return null;
            }
        }

        private class ConnectOp : IOp
        {
            private SocketBase _baseSocket;
            private string _hostName;
            private int _hostPort;
            private EndPoint _remoteEP;

            internal ConnectOp(SocketBase baseSocket)
            {
                this._hostPort = -1;
                this._baseSocket = baseSocket;
            }

            internal ConnectOp(SocketBase baseSocket, EndPoint remoteEP)
            {
                this._hostPort = -1;
                this._baseSocket = baseSocket;
                this._remoteEP = remoteEP;
            }

            internal ConnectOp(SocketBase baseSocket, string hostName, int hostPort)
            {
                this._hostPort = -1;
                this._baseSocket = baseSocket;
                this._hostName = hostName;
                this._hostPort = hostPort;
            }

            internal override object BeginExecute(AsyncCallback cb, object state)
            {
                if (this._remoteEP != null)
                {
                    return this._baseSocket.BeginConnect(this._remoteEP, cb, state);
                }
                return this._baseSocket.BeginConnect(this._hostName, this._hostPort, cb, state);
            }

            internal override object EndExecute(IAsyncResult ar)
            {
                this._baseSocket.EndConnect(ar);
                return null;
            }

            internal override object Execute()
            {
                if (this._remoteEP != null)
                {
                    this._baseSocket.Connect(this._remoteEP);
                }
                else
                {
                    this._baseSocket.Connect(this._hostName, this._hostPort);
                }
                return null;
            }
        }

        private class Listen_Op : IOp
        {
            private int _backlog;
            private SocketBase _baseSocket;

            internal Listen_Op(SocketBase baseSocket, int backlog)
            {
                this._backlog = backlog;
                this._baseSocket = baseSocket;
            }

            internal override object BeginExecute(AsyncCallback cb, object state)
            {
                throw new NotSupportedException("BeginListen is not supported.");
            }

            internal override object EndExecute(IAsyncResult ar)
            {
                throw new NotSupportedException("EndListen is not supported.");
            }

            internal override object Execute()
            {
                this._baseSocket.Listen(this._backlog);
                return null;
            }
        }

        private class Receive_Op : IOp
        {
            private SocketBase _baseSocket;
            private byte[] _buffer;
            private int _offset;
            private int _size;

            internal Receive_Op(SocketBase baseSocket)
            {
                this._baseSocket = baseSocket;
            }

            internal Receive_Op(SocketBase baseSocket, byte[] buffer, int offset, int size)
            {
                this._baseSocket = baseSocket;
                this._buffer = buffer;
                this._offset = offset;
                this._size = size;
            }

            internal override object BeginExecute(AsyncCallback cb, object state)
            {
                return this._baseSocket.BeginReceive(this._buffer, this._offset, this._size, cb, state);
            }

            internal override object EndExecute(IAsyncResult ar)
            {
                return this._baseSocket.EndReceive(ar);
            }

            internal override object Execute()
            {
                return this._baseSocket.Receive(this._buffer, this._offset, this._size);
            }
        }

        private class Send_Op : IOp
        {
            private SocketBase _baseSocket;
            private byte[] _buffer;
            private int _offset;
            private int _size;

            internal Send_Op(SocketBase baseSocket)
            {
                this._baseSocket = baseSocket;
            }

            internal Send_Op(SocketBase baseSocket, byte[] buffer, int offset, int size)
            {
                this._baseSocket = baseSocket;
                this._buffer = buffer;
                this._offset = offset;
                this._size = size;
            }

            internal override object BeginExecute(AsyncCallback cb, object state)
            {
                return this._baseSocket.BeginSend(this._buffer, this._offset, this._size, cb, state);
            }

            internal override object EndExecute(IAsyncResult ar)
            {
                return this._baseSocket.EndSend(ar);
            }

            internal override object Execute()
            {
                return this._baseSocket.Send(this._buffer, this._offset, this._size);
            }
        }
    }
}

