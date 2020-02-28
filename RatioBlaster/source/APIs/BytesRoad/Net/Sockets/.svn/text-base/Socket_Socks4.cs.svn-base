namespace BytesRoad.Net.Sockets
{
    using BytesRoad.Net.Sockets.Advanced;
    using System;
    using System.Net;
    using System.Net.Sockets;

    internal class Socket_Socks4 : SocketBase
    {
        private EndPoint _localEndPoint;
        private EndPoint _remoteEndPoint;
        private byte[] _response;

        internal Socket_Socks4(string proxyServer, int proxyPort, byte[] proxyUser) : base(proxyServer, proxyPort, proxyUser, null)
        {
            this._response = new byte[8];
        }

        internal override SocketBase Accept()
        {
            base.CheckDisposed();
            this.SetProgress(true);
            try
            {
                for (int i = 0; i < 8; i += base.NStream.Read(this._response, i, this._response.Length - i))
                {
                }
                this.VerifyResponse();
            }
            finally
            {
                this.SetProgress(false);
            }
            return this;
        }

        private void Accept_Read_End(IAsyncResult ar)
        {
            Accept_SO asyncState = (Accept_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                asyncState.ReadBytes += base.NStream.EndRead(ar);
                if (asyncState.ReadBytes < 8)
                {
                    base.NStream.BeginRead(this._response, asyncState.ReadBytes, 8 - asyncState.ReadBytes, new AsyncCallback(this.Accept_Read_End), asyncState);
                }
                else
                {
                    this.VerifyResponse();
                    asyncState.SetCompleted();
                }
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
                asyncState.SetCompleted();
            }
        }

        internal override IAsyncResult BeginAccept(AsyncCallback callback, object state)
        {
            base.CheckDisposed();
            Accept_SO t_so = null;
            this.SetProgress(true);
            try
            {
                t_so = new Accept_SO(callback, state);
                base.NStream.BeginRead(this._response, 0, 8, new AsyncCallback(this.Accept_Read_End), t_so);
            }
            catch
            {
                this.SetProgress(false);
                throw;
            }
            return t_so;
        }

        internal override IAsyncResult BeginBind(SocketBase baseSocket, AsyncCallback callback, object state)
        {
            base.CheckDisposed();
            Bind_SO d_so = null;
            this.SetProgress(true);
            try
            {
                d_so = new Bind_SO(baseSocket, callback, state);
                SocketBase.BeginGetHostByName(base._proxyServer, new AsyncCallback(this.Bind_GetHost_End), d_so);
            }
            catch (Exception exception)
            {
                this.SetProgress(false);
                throw exception;
            }
            return d_so;
        }

        internal override IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state)
        {
            base.CheckDisposed();
            Connect_SO t_so = null;
            this.SetProgress(true);
            try
            {
                t_so = new Connect_SO(remoteEP, -1, callback, state);
                SocketBase.BeginGetHostByName(base._proxyServer, new AsyncCallback(this.Connect_GetHost_Proxy_End), t_so);
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
            Connect_SO t_so = null;
            this.SetProgress(true);
            try
            {
                t_so = new Connect_SO(null, port, callback, state);
                SocketBase.BeginGetHostByName(hostName, new AsyncCallback(this.Connect_GetHost_Host_End), t_so);
            }
            catch (Exception exception)
            {
                this.SetProgress(false);
                throw exception;
            }
            return t_so;
        }

        internal override void Bind(SocketBase socket)
        {
            base.CheckDisposed();
            this.SetProgress(true);
            try
            {
                IPHostEntry hostByName = SocketBase.GetHostByName(base._proxyServer);
                if (hostByName == null)
                {
                    throw new SocketException(0x2af9);
                }
                IPEndPoint remoteEP = SocketBase.ConstructEndPoint(hostByName, base._proxyPort);
                base._socket.Connect(remoteEP);
                byte[] buffer = this.PrepareBindCmd(socket);
                base.NStream.Write(buffer, 0, buffer.Length);
                for (int i = 0; i < 8; i += base.NStream.Read(this._response, i, this._response.Length - i))
                {
                }
                this.VerifyResponse();
                this._localEndPoint = this.ConstructBindEndPoint(remoteEP.Address);
                this._remoteEndPoint = null;
            }
            finally
            {
                this.SetProgress(false);
            }
        }

        private void Bind_Connect_End(IAsyncResult ar)
        {
            Bind_SO asyncState = (Bind_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                base._socket.EndConnect(ar);
                byte[] buffer = this.PrepareBindCmd(asyncState.BaseSocket);
                base.NStream.BeginWrite(buffer, 0, buffer.Length, new AsyncCallback(this.Bind_Write_End), asyncState);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
                asyncState.SetCompleted();
            }
        }

        private void Bind_GetHost_End(IAsyncResult ar)
        {
            Bind_SO asyncState = (Bind_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                IPHostEntry host = SocketBase.EndGetHostByName(ar);
                if (host == null)
                {
                    throw new SocketException(0x2af9);
                }
                IPEndPoint remoteEP = SocketBase.ConstructEndPoint(host, base._proxyPort);
                asyncState.ProxyIP = remoteEP.Address;
                base._socket.BeginConnect(remoteEP, new AsyncCallback(this.Bind_Connect_End), asyncState);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
                asyncState.SetCompleted();
            }
        }

        private void Bind_Read_End(IAsyncResult ar)
        {
            Bind_SO asyncState = (Bind_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                int num = base.NStream.EndRead(ar);
                asyncState.ReadBytes += num;
                if (asyncState.ReadBytes < 8)
                {
                    base.NStream.BeginRead(this._response, asyncState.ReadBytes, 8 - asyncState.ReadBytes, new AsyncCallback(this.Bind_Read_End), asyncState);
                }
                else
                {
                    this.VerifyResponse();
                    this._localEndPoint = this.ConstructBindEndPoint(asyncState.ProxyIP);
                    this._remoteEndPoint = null;
                    asyncState.SetCompleted();
                }
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
                asyncState.SetCompleted();
            }
        }

        private void Bind_Write_End(IAsyncResult ar)
        {
            Bind_SO asyncState = (Bind_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                base.NStream.EndWrite(ar);
                base.NStream.BeginRead(this._response, 0, 8, new AsyncCallback(this.Bind_Read_End), asyncState);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
                asyncState.SetCompleted();
            }
        }

        internal override void Connect(EndPoint remoteEP)
        {
            base.CheckDisposed();
            this.SetProgress(true);
            try
            {
                IPHostEntry hostByName = SocketBase.GetHostByName(base._proxyServer);
                if (hostByName == null)
                {
                    throw new SocketException(0x2af9);
                }
                IPEndPoint point = SocketBase.ConstructEndPoint(hostByName, base._proxyPort);
                base._socket.Connect(point);
                this._localEndPoint = null;
                this._remoteEndPoint = remoteEP;
                byte[] buffer = this.PrepareConnectCmd(remoteEP);
                base.NStream.Write(buffer, 0, buffer.Length);
                for (int i = 0; i < 8; i += base.NStream.Read(this._response, i, this._response.Length - i))
                {
                }
                this.VerifyResponse();
            }
            finally
            {
                this.SetProgress(false);
            }
        }

        private void Connect_Connect_End(IAsyncResult ar)
        {
            Connect_SO asyncState = (Connect_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                base._socket.EndConnect(ar);
                this._localEndPoint = null;
                this._remoteEndPoint = asyncState.RemoteEndPoint;
                byte[] buffer = this.PrepareConnectCmd(asyncState.RemoteEndPoint);
                base.NStream.BeginWrite(buffer, 0, buffer.Length, new AsyncCallback(this.Connect_Write_End), asyncState);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
                asyncState.SetCompleted();
            }
        }

        private void Connect_GetHost_Host_End(IAsyncResult ar)
        {
            Connect_SO asyncState = (Connect_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                IPHostEntry host = SocketBase.EndGetHostByName(ar);
                if (host == null)
                {
                    throw new SocketException(0x2af9);
                }
                asyncState.RemoteEndPoint = SocketBase.ConstructEndPoint(host, asyncState.Port);
                SocketBase.BeginGetHostByName(base._proxyServer, new AsyncCallback(this.Connect_GetHost_Proxy_End), asyncState);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
                asyncState.SetCompleted();
            }
        }

        private void Connect_GetHost_Proxy_End(IAsyncResult ar)
        {
            Connect_SO asyncState = (Connect_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                IPHostEntry host = SocketBase.EndGetHostByName(ar);
                if (host == null)
                {
                    throw new SocketException(0x2af9);
                }
                IPEndPoint remoteEP = SocketBase.ConstructEndPoint(host, base._proxyPort);
                base._socket.BeginConnect(remoteEP, new AsyncCallback(this.Connect_Connect_End), asyncState);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
                asyncState.SetCompleted();
            }
        }

        private void Connect_Read_End(IAsyncResult ar)
        {
            Connect_SO asyncState = (Connect_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                int num = base.NStream.EndRead(ar);
                asyncState.ReadBytes += num;
                if (asyncState.ReadBytes < 8)
                {
                    base.NStream.BeginRead(this._response, asyncState.ReadBytes, 8 - asyncState.ReadBytes, new AsyncCallback(this.Connect_Read_End), asyncState);
                }
                else
                {
                    this.VerifyResponse();
                    asyncState.SetCompleted();
                }
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
                asyncState.SetCompleted();
            }
        }

        private void Connect_Write_End(IAsyncResult ar)
        {
            Connect_SO asyncState = (Connect_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                base.NStream.EndWrite(ar);
                base.NStream.BeginRead(this._response, 0, 8, new AsyncCallback(this.Connect_Read_End), asyncState);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
                asyncState.SetCompleted();
            }
        }

        private IPEndPoint ConstructBindEndPoint(IPAddress proxyIP)
        {
            int port = (this._response[2] << 8) | this._response[3];
            long newAddress = (((this._response[7] << 0x18) | (this._response[6] << 0x10)) | (this._response[5] << 8)) | this._response[4];
            newAddress &= (long) 0xffffffffL;
            if (0L == newAddress)
            {
                return new IPEndPoint(proxyIP, port);
            }
            return new IPEndPoint(new IPAddress(newAddress), port);
        }

        internal override SocketBase EndAccept(IAsyncResult asyncResult)
        {
            AsyncBase.VerifyAsyncResult(asyncResult, typeof(Accept_SO), "EndAccept");
            this.HandleAsyncEnd(asyncResult, true);
            return this;
        }

        internal override void EndBind(IAsyncResult asyncResult)
        {
            AsyncBase.VerifyAsyncResult(asyncResult, typeof(Bind_SO), "EndBind");
            this.HandleAsyncEnd(asyncResult, true);
        }

        internal override void EndConnect(IAsyncResult asyncResult)
        {
            AsyncBase.VerifyAsyncResult(asyncResult, typeof(Connect_SO), "EndConnect");
            this.HandleAsyncEnd(asyncResult, true);
        }

        internal override void Listen(int backlog)
        {
            base.CheckDisposed();
            if (this._localEndPoint == null)
            {
                throw new ArgumentException("Attempt to listen on socket which has not been bound with Bind.");
            }
        }

        private byte[] PrepareBindCmd(SocketBase baseSocket)
        {
            return this.PrepareCmd(baseSocket.RemoteEndPoint, 2);
        }

        private byte[] PrepareCmd(EndPoint remoteEP, byte cmdVal)
        {
            int length = 0;
            if (base._proxyUser != null)
            {
                length = base._proxyUser.Length;
            }
            IPEndPoint point = (IPEndPoint) remoteEP;
            byte[] destinationArray = new byte[(8 + length) + 1];
            destinationArray[0] = 4;
            destinationArray[1] = cmdVal;
            destinationArray[2] = (byte) ((point.Port & 0xff00) >> 8);
            destinationArray[3] = (byte) (point.Port & 0xff);
            long address = point.Address.Address;
            destinationArray[7] = (byte) ((address & 0xff000000L) >> 0x18);
            destinationArray[6] = (byte) ((address & 0xff0000L) >> 0x10);
            destinationArray[5] = (byte) ((address & 0xff00L) >> 8);
            destinationArray[4] = (byte) (address & 0xffL);
            if (length > 0)
            {
                Array.Copy(base._proxyUser, 0, destinationArray, 8, length);
            }
            destinationArray[8 + length] = 0;
            return destinationArray;
        }

        private byte[] PrepareConnectCmd(EndPoint remoteEP)
        {
            return this.PrepareCmd(remoteEP, 1);
        }

        private void VerifyResponse()
        {
            string str = null;
            if ((this._response[0] != 0) && (this._response[0] != 4))
            {
                throw new ProtocolViolationException(string.Format("Socks4: Reply format is unknown ({0}).", this._response[0]));
            }
            if (this._response[1] != 90)
            {
                byte num = this._response[1];
                if (0x5b == num)
                {
                    str = string.Format("Socks4: Request rejected or failed ({0}).", num);
                }
                else if (0x5c == num)
                {
                    str = string.Format("Socks4: Request rejected because SOCKS server cannot connect to identd on the client ({0}).", num);
                }
                else if (0x5d == num)
                {
                    str = string.Format("Socks4: Request rejected because the client program and identd report different user-ids ({0}).", num);
                }
                else
                {
                    str = string.Format("Socks4: Socks server return unknown error code ({0}).", num);
                }
            }
            if (str != null)
            {
                throw new SocketException(0x274d);
            }
        }

        internal override EndPoint LocalEndPoint
        {
            get
            {
                return this._localEndPoint;
            }
        }

        internal override BytesRoad.Net.Sockets.ProxyType ProxyType
        {
            get
            {
                return BytesRoad.Net.Sockets.ProxyType.Socks4;
            }
        }

        internal override EndPoint RemoteEndPoint
        {
            get
            {
                return this._remoteEndPoint;
            }
        }

        private class Accept_SO : AsyncResultBase
        {
            private int _readBytes;

            internal Accept_SO(AsyncCallback cb, object state) : base(cb, state)
            {
            }

            internal int ReadBytes
            {
                get
                {
                    return this._readBytes;
                }
                set
                {
                    this._readBytes = value;
                }
            }
        }

        private class Bind_SO : AsyncResultBase
        {
            private SocketBase _baseSocket;
            private IPAddress _proxyIP;
            private int _readBytes;

            internal Bind_SO(SocketBase baseSocket, AsyncCallback cb, object state) : base(cb, state)
            {
                this._baseSocket = baseSocket;
            }

            internal SocketBase BaseSocket
            {
                get
                {
                    return this._baseSocket;
                }
            }

            internal IPAddress ProxyIP
            {
                get
                {
                    return this._proxyIP;
                }
                set
                {
                    this._proxyIP = value;
                }
            }

            internal int ReadBytes
            {
                get
                {
                    return this._readBytes;
                }
                set
                {
                    this._readBytes = value;
                }
            }
        }

        private class Connect_SO : AsyncResultBase
        {
            private int _port;
            private int _readBytes;
            private EndPoint _remoteEndPoint;

            internal Connect_SO(EndPoint remoteEndPoint, int port, AsyncCallback cb, object state) : base(cb, state)
            {
                this._port = -1;
                this._remoteEndPoint = remoteEndPoint;
                this._port = port;
            }

            internal int Port
            {
                get
                {
                    return this._port;
                }
            }

            internal int ReadBytes
            {
                get
                {
                    return this._readBytes;
                }
                set
                {
                    this._readBytes = value;
                }
            }

            internal EndPoint RemoteEndPoint
            {
                get
                {
                    return this._remoteEndPoint;
                }
                set
                {
                    this._remoteEndPoint = value;
                }
            }
        }
    }
}

