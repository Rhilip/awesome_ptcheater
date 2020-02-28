namespace BytesRoad.Net.Sockets
{
    using BytesRoad.Net.Sockets.Advanced;
    using System;
    using System.Net;
    using System.Net.Sockets;

    internal class Socket_None : SocketBase
    {
        internal Socket_None()
        {
        }

        internal Socket_None(Socket systemSocket) : base(systemSocket)
        {
        }

        internal override SocketBase Accept()
        {
            base.CheckDisposed();
            return new Socket_None(base._socket.Accept());
        }

        internal override IAsyncResult BeginAccept(AsyncCallback callback, object state)
        {
            base.CheckDisposed();
            return base._socket.BeginAccept(callback, state);
        }

        internal override IAsyncResult BeginBind(SocketBase baseSocket, AsyncCallback callback, object state)
        {
            base.CheckDisposed();
            Bind_SO d_so = new Bind_SO(callback, state);
            try
            {
                IPEndPoint localEndPoint = (IPEndPoint) baseSocket.SystemSocket.LocalEndPoint;
                localEndPoint.Port = 0;
                base._socket.Bind(localEndPoint);
            }
            catch (Exception exception)
            {
                d_so.Exception = exception;
            }
            d_so.SetCompleted();
            return d_so;
        }

        internal override IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state)
        {
            base.CheckDisposed();
            Connect_SO t_so = null;
            this.SetProgress(true);
            try
            {
                t_so = new Connect_SO(-1, callback, state);
                base._socket.BeginConnect(remoteEP, new AsyncCallback(this.Connect_End), t_so);
            }
            catch (Exception exception)
            {
                this.SetProgress(false);
                throw exception;
            }
            return t_so;
        }

        internal override IAsyncResult BeginConnect(string hostName, int port, AsyncCallback callback, object state)
        {
            base.CheckDisposed();
            this.SetProgress(true);
            Connect_SO stateObject = null;
            try
            {
                stateObject = new Connect_SO(port, callback, state);
                Dns.BeginGetHostEntry(hostName, new AsyncCallback(this.GetHost_End), stateObject);
            }
            catch (Exception exception)
            {
                this.SetProgress(false);
                throw exception;
            }
            return stateObject;
        }

        internal override void Bind(SocketBase baseSocket)
        {
            base.CheckDisposed();
            IPEndPoint localEndPoint = (IPEndPoint) baseSocket.SystemSocket.LocalEndPoint;
            localEndPoint.Port = 0;
            base._socket.Bind(localEndPoint);
        }

        internal override void Connect(EndPoint remoteEP)
        {
            base.CheckDisposed();
            base._socket.Connect(remoteEP);
        }

        private void Connect_End(IAsyncResult ar)
        {
            Connect_SO asyncState = (Connect_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                base._socket.EndConnect(ar);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
            }
            asyncState.SetCompleted();
        }

        internal override SocketBase EndAccept(IAsyncResult asyncResult)
        {
            return new Socket_None(base._socket.EndAccept(asyncResult));
        }

        internal override void EndBind(IAsyncResult ar)
        {
            AsyncBase.VerifyAsyncResult(ar, typeof(Bind_SO), "EndBind");
            this.HandleAsyncEnd(ar, false);
        }

        internal override void EndConnect(IAsyncResult asyncResult)
        {
            AsyncBase.VerifyAsyncResult(asyncResult, typeof(Connect_SO), "EndConnect");
            this.HandleAsyncEnd(asyncResult, true);
        }

        private void GetHost_End(IAsyncResult ar)
        {
            Connect_SO asyncState = (Connect_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                IPHostEntry host = Dns.EndGetHostEntry(ar);
                if (host == null)
                {
                    throw new SocketException(0x2af9);
                }
                EndPoint remoteEP = SocketBase.ConstructEndPoint(host, asyncState.Port);
                base._socket.BeginConnect(remoteEP, new AsyncCallback(this.Connect_End), asyncState);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
                asyncState.SetCompleted();
            }
        }

        internal override void Listen(int backlog)
        {
            base.CheckDisposed();
            base._socket.Listen(backlog);
        }

        internal override EndPoint LocalEndPoint
        {
            get
            {
                return base._socket.LocalEndPoint;
            }
        }

        internal override BytesRoad.Net.Sockets.ProxyType ProxyType
        {
            get
            {
                return BytesRoad.Net.Sockets.ProxyType.None;
            }
        }

        internal override EndPoint RemoteEndPoint
        {
            get
            {
                return base._socket.RemoteEndPoint;
            }
        }

        private class Bind_SO : AsyncResultBase
        {
            internal Bind_SO(AsyncCallback cb, object state) : base(cb, state)
            {
            }
        }

        private class Connect_SO : AsyncResultBase
        {
            private int _port;

            internal Connect_SO(int port, AsyncCallback cb, object state) : base(cb, state)
            {
                this._port = port;
            }

            internal int Port
            {
                get
                {
                    return this._port;
                }
            }
        }
    }
}

