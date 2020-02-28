namespace BytesRoad.Net.Sockets
{
    using BytesRoad.Net.Sockets.Advanced;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    internal class Socket_Socks5 : SocketBase
    {
        private EndPoint _localEndPoint;
        private EndPoint _remoteEndPoint;
        private string _remoteHost;
        private int _remotePort;
        private bool _resolveHostEnabled;

        internal Socket_Socks5(string proxyServer, int proxyPort, byte[] proxyUser, byte[] proxyPassword) : base(proxyServer, proxyPort, proxyUser, proxyPassword)
        {
            this._resolveHostEnabled = true;
            this._remotePort = -1;
        }

        internal override SocketBase Accept()
        {
            base.CheckDisposed();
            this.SetProgress(true);
            try
            {
                byte[] reply = this.ReadVerifyReply();
                this._remoteEndPoint = this.ExtractReplyAddr(reply);
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
                byte[] reply = this.EndReadVerifyReply(ar);
                this._remoteEndPoint = this.ExtractReplyAddr(reply);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
            }
            asyncState.SetCompleted();
        }

        internal override IAsyncResult BeginAccept(AsyncCallback callback, object state)
        {
            base.CheckDisposed();
            Accept_SO t_so = null;
            this.SetProgress(true);
            try
            {
                t_so = new Accept_SO(callback, state);
                this.BeginReadVerifyReply(new AsyncCallback(this.Accept_Read_End), t_so);
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
            if (baseSocket == null)
            {
                throw new ArgumentNullException("baseSocket", "The value cannot be null");
            }
            Bind_SO d_so = null;
            this.SetProgress(true);
            try
            {
                d_so = new Bind_SO((Socket_Socks5) baseSocket, callback, state);
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
            if (remoteEP == null)
            {
                throw new ArgumentNullException("remoteEP", "The value cannot be null.");
            }
            Connect_SO t_so = null;
            this.SetProgress(true);
            try
            {
                t_so = new Connect_SO(remoteEP, null, -1, callback, state);
                SocketBase.BeginGetHostByName(base._proxyServer, new AsyncCallback(this.Connect_GetHost_Proxy_End), t_so);
            }
            catch (Exception exception)
            {
                this.SetProgress(false);
                throw exception;
            }
            return t_so;
        }

        internal override IAsyncResult BeginConnect(string hostName, int hostPort, AsyncCallback callback, object state)
        {
            base.CheckDisposed();
            if (hostName == null)
            {
                throw new ArgumentNullException("hostName", "The value cannot be null.");
            }
            if ((hostPort < 0) || (hostPort > 0xffff))
            {
                throw new ArgumentOutOfRangeException("hostPort", "Value, specified for the port is out of the valid range.");
            }
            Connect_SO t_so = null;
            this.SetProgress(true);
            try
            {
                t_so = new Connect_SO(null, hostName, hostPort, callback, state);
                if (this._resolveHostEnabled)
                {
                    SocketBase.BeginGetHostByName(hostName, new AsyncCallback(this.Connect_GetHost_Host_End), t_so);
                    return t_so;
                }
                SocketBase.BeginGetHostByName(base._proxyServer, new AsyncCallback(this.Connect_GetHost_Proxy_End), t_so);
            }
            catch (Exception exception)
            {
                this.SetProgress(false);
                throw exception;
            }
            return t_so;
        }

        private IAsyncResult BeginDoAuthentication(AuthMethod method, AsyncCallback cb, object state)
        {
            DoAuthentication_SO n_so = new DoAuthentication_SO(cb, state);
            if (AuthMethod.UsernamePassword == method)
            {
                this.BeginSubNegotiation_UsernamePassword(new AsyncCallback(this.SubNegotiation_UsernamePassword_End), n_so);
                return n_so;
            }
            if (AuthMethod.NoAcceptable == method)
            {
                throw new SocketException(0x274d);
            }
            if (method != AuthMethod.None)
            {
                throw new InvalidOperationException("Unknown authentication requested.");
            }
            n_so.SetCompleted();
            return n_so;
        }

        private IAsyncResult BeginNegotiate(AsyncCallback callback, object state)
        {
            bool preAuthenticate = base.PreAuthenticate;
            if (base._proxyUser == null)
            {
                preAuthenticate = false;
            }
            Negotiation_SO n_so = new Negotiation_SO(preAuthenticate, callback, state);
            byte[] buffer = this.PrepareNegotiationCmd(n_so.UseCredentials);
            base.NStream.BeginWrite(buffer, 0, buffer.Length, new AsyncCallback(this.Negotiate_Write_End), n_so);
            return n_so;
        }

        private IAsyncResult BeginReadVerifyReply(AsyncCallback cb, object state)
        {
            ReadVerifyReply_SO y_so = new ReadVerifyReply_SO(cb, state);
            this.BeginReadWhole(y_so.Phase1Data, 0, 5, new AsyncCallback(this.Phase1_End), y_so);
            return y_so;
        }

        private IAsyncResult BeginReadWhole(byte[] buffer, int offset, int size, AsyncCallback cb, object state)
        {
            ReadWhole_SO e_so = null;
            e_so = new ReadWhole_SO(buffer, offset, size, cb, state);
            base.NStream.BeginRead(buffer, offset, size, new AsyncCallback(this.ReadWhole_Read_End), e_so);
            return e_so;
        }

        private IAsyncResult BeginSubNegotiation_UsernamePassword(AsyncCallback cb, object state)
        {
            byte[] buffer = this.Prepare_UsernamePasswordCmd();
            UsernamePassword_SO d_so = new UsernamePassword_SO(cb, state);
            base.NStream.BeginWrite(buffer, 0, buffer.Length, new AsyncCallback(this.SubUsernamePassword_Write_End), d_so);
            return d_so;
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
                this.Negotiate();
                byte[] buffer = this.PrepareBindCmd((Socket_Socks5) socket);
                base.NStream.Write(buffer, 0, buffer.Length);
                byte[] reply = this.ReadVerifyReply();
                this._localEndPoint = this.ExtractReplyAddr(reply);
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
                this.BeginNegotiate(new AsyncCallback(this.Bind_Negotiate_End), asyncState);
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

        private void Bind_Negotiate_End(IAsyncResult ar)
        {
            Bind_SO asyncState = (Bind_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                this.EndNegotiate(ar);
                byte[] buffer = this.PrepareBindCmd(asyncState.BaseSocket);
                base.NStream.BeginWrite(buffer, 0, buffer.Length, new AsyncCallback(this.Bind_Write_End), asyncState);
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
                byte[] reply = this.EndReadVerifyReply(ar);
                this._localEndPoint = this.ExtractReplyAddr(reply);
                this._remoteEndPoint = null;
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
            }
            asyncState.SetCompleted();
        }

        private void Bind_Write_End(IAsyncResult ar)
        {
            Bind_SO asyncState = (Bind_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                base.NStream.EndWrite(ar);
                this.BeginReadVerifyReply(new AsyncCallback(this.Bind_Read_End), asyncState);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
                asyncState.SetCompleted();
            }
        }

        private void CheckReplyForError(byte[] reply)
        {
            byte num = reply[1];
            if (num != 0)
            {
                if ((((1 != num) && (2 != num)) && ((3 != num) && (4 != num))) && (((5 != num) && (6 != num)) && ((7 != num) && (8 != num))))
                {
                    string.Format("Socks5: Reply code is unknown ({0}).", num);
                }
                throw new SocketException(0x274d);
            }
        }

        private void CheckReplyVer(byte[] reply)
        {
            if (5 != reply[0])
            {
                throw new ProtocolViolationException(string.Format("Socks5: Unknown format of reply ({0}).", reply[0]));
            }
        }

        internal override void Connect(EndPoint remoteEP)
        {
            if (remoteEP == null)
            {
                throw new ArgumentNullException("remoteEP", "The value cannot be null.");
            }
            this.Connect(remoteEP, null, -1);
        }

        internal override void Connect(string hostName, int hostPort)
        {
            if (hostName == null)
            {
                throw new ArgumentNullException("hostName", "The value cannot be null.");
            }
            if ((hostPort < 0) || (hostPort > 0xffff))
            {
                throw new ArgumentOutOfRangeException("hostPort", "Value, specified for the port is out of the valid range.");
            }
            this.Connect(null, hostName, hostPort);
        }

        private void Connect(EndPoint remoteEP, string hostName, int hostPort)
        {
            base.CheckDisposed();
            this.SetProgress(true);
            try
            {
                if (remoteEP == null)
                {
                    if (this._resolveHostEnabled)
                    {
                        IPHostEntry host = SocketBase.GetHostByName(hostName);
                        if (host != null)
                        {
                            remoteEP = SocketBase.ConstructEndPoint(host, hostPort);
                        }
                    }
                    if ((hostName == null) && (remoteEP == null))
                    {
                        throw new ArgumentNullException("hostName", "The value cannot be null.");
                    }
                }
                IPHostEntry hostByName = SocketBase.GetHostByName(base._proxyServer);
                if (hostByName == null)
                {
                    throw new SocketException(0x2af9);
                }
                IPEndPoint point = SocketBase.ConstructEndPoint(hostByName, base._proxyPort);
                base._socket.Connect(point);
                this.Negotiate();
                byte[] buffer = this.PrepareConnectCmd(remoteEP, hostName, hostPort);
                base.NStream.Write(buffer, 0, buffer.Length);
                byte[] reply = this.ReadVerifyReply();
                this._localEndPoint = this.ExtractReplyAddr(reply);
                this._remoteEndPoint = remoteEP;
                if (remoteEP == null)
                {
                    this._remotePort = hostPort;
                    this._remoteHost = hostName;
                }
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
                this.BeginNegotiate(new AsyncCallback(this.Connect_Negotiate_End), asyncState);
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
                if (host != null)
                {
                    asyncState.RemoteEndPoint = SocketBase.ConstructEndPoint(host, asyncState.HostPort);
                }
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

        private void Connect_Negotiate_End(IAsyncResult ar)
        {
            Connect_SO asyncState = (Connect_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                this.EndNegotiate(ar);
                byte[] buffer = this.PrepareConnectCmd(asyncState.RemoteEndPoint, asyncState.HostName, asyncState.HostPort);
                base.NStream.BeginWrite(buffer, 0, buffer.Length, new AsyncCallback(this.Connect_Write_End), asyncState);
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
                byte[] reply = this.EndReadVerifyReply(ar);
                this._localEndPoint = this.ExtractReplyAddr(reply);
                this._remoteEndPoint = asyncState.RemoteEndPoint;
                if (asyncState.RemoteEndPoint == null)
                {
                    this._remotePort = asyncState.HostPort;
                    this._remoteHost = asyncState.HostName;
                }
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
            }
            asyncState.SetCompleted();
        }

        private void Connect_Write_End(IAsyncResult ar)
        {
            Connect_SO asyncState = (Connect_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                base.NStream.EndWrite(ar);
                this.BeginReadVerifyReply(new AsyncCallback(this.Connect_Read_End), asyncState);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
                asyncState.SetCompleted();
            }
        }

        private void DoAuthentication(AuthMethod method)
        {
            if (method != AuthMethod.None)
            {
                if (AuthMethod.UsernamePassword == method)
                {
                    this.SubNegotiation_UsernamePassword();
                }
                else
                {
                    if (AuthMethod.NoAcceptable == method)
                    {
                        throw new SocketException(0x274d);
                    }
                    throw new InvalidOperationException("Unknown authentication requested.");
                }
            }
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

        private void EndDoAuthentication(IAsyncResult ar)
        {
            AsyncBase.VerifyAsyncResult(ar, typeof(DoAuthentication_SO));
            this.HandleAsyncEnd(ar, false);
        }

        private void EndNegotiate(IAsyncResult ar)
        {
            AsyncBase.VerifyAsyncResult(ar, typeof(Negotiation_SO));
            this.HandleAsyncEnd(ar, false);
        }

        private byte[] EndReadVerifyReply(IAsyncResult ar)
        {
            AsyncBase.VerifyAsyncResult(ar, typeof(ReadVerifyReply_SO));
            this.HandleAsyncEnd(ar, false);
            return ((ReadVerifyReply_SO) ar).Reply;
        }

        private void EndReadWhole(IAsyncResult ar)
        {
            AsyncBase.VerifyAsyncResult(ar, typeof(ReadWhole_SO));
            this.HandleAsyncEnd(ar, false);
        }

        private void EndSubNegotiation_UsernamePassword(IAsyncResult ar)
        {
            AsyncBase.VerifyAsyncResult(ar, typeof(UsernamePassword_SO));
            this.HandleAsyncEnd(ar, true);
        }

        private AuthMethod ExtractAuthMethod(byte[] reply)
        {
            if (reply[0] != 5)
            {
                throw new ProtocolViolationException(string.Format("Socks5 server returns reply with unknown version ({0}).", reply[0]));
            }
            byte num = reply[1];
            if (num == 0)
            {
                return AuthMethod.None;
            }
            if (2 == num)
            {
                return AuthMethod.UsernamePassword;
            }
            if (0xff != num)
            {
                throw new ProtocolViolationException(string.Format("Socks5 server requires not declared authentication method ({0}).", num));
            }
            return AuthMethod.NoAcceptable;
        }

        private IPEndPoint ExtractReplyAddr(byte[] reply)
        {
            byte num = reply[3];
            if (num != 1)
            {
                throw new ArgumentException(string.Format("Socks5: Address type in reply is unknown ({0}).", num), "reply");
            }
            int port = (reply[8] << 8) | reply[9];
            long newAddress = (((reply[7] << 0x18) | (reply[6] << 0x10)) | (reply[5] << 8)) | reply[4];
            newAddress &= (long) 0xffffffffL;
            return new IPEndPoint(new IPAddress(newAddress), port);
        }

        private int GetAddrFieldLength(byte[] reply)
        {
            byte num = reply[3];
            if (1 == num)
            {
                return 4;
            }
            if (3 == num)
            {
                return (1 + reply[4]);
            }
            if (4 != num)
            {
                throw new ProtocolViolationException(string.Format("Socks5: Unknown address type in reply ({0}).", num));
            }
            return 0x10;
        }

        internal override void Listen(int backlog)
        {
            base.CheckDisposed();
            if (this._localEndPoint == null)
            {
                throw new ArgumentException("Attempt to listen on socket which has not been bound with Bind.");
            }
        }

        private void Negotiate()
        {
            byte[] buffer;
            bool preAuthenticate = base.PreAuthenticate;
            if (base._proxyUser == null)
            {
                preAuthenticate = false;
            }
        Label_0011:
            buffer = this.PrepareNegotiationCmd(preAuthenticate);
            base.NStream.Write(buffer, 0, buffer.Length);
            byte[] buffer2 = new byte[2];
            this.ReadWhole(buffer2, 0, 2);
            AuthMethod method = this.ExtractAuthMethod(buffer2);
            if (((AuthMethod.NoAcceptable == method) && !preAuthenticate) && (base._proxyUser != null))
            {
                preAuthenticate = true;
                goto Label_0011;
            }
            this.DoAuthentication(method);
        }

        private void Negotiate_DoAuth_End(IAsyncResult ar)
        {
            Negotiation_SO asyncState = (Negotiation_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                this.EndDoAuthentication(ar);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
            }
            asyncState.SetCompleted();
        }

        private void Negotiate_ReadWhole_End(IAsyncResult ar)
        {
            Negotiation_SO asyncState = (Negotiation_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                this.EndReadWhole(ar);
                AuthMethod method = this.ExtractAuthMethod(asyncState.Reply);
                if (((AuthMethod.NoAcceptable == method) && !asyncState.UseCredentials) && (base._proxyUser != null))
                {
                    asyncState.UseCredentials = true;
                    byte[] buffer = this.PrepareNegotiationCmd(asyncState.UseCredentials);
                    base.NStream.BeginWrite(buffer, 0, buffer.Length, new AsyncCallback(this.Negotiate_Write_End), asyncState);
                }
                else
                {
                    this.BeginDoAuthentication(method, new AsyncCallback(this.Negotiate_DoAuth_End), asyncState);
                }
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
                asyncState.SetCompleted();
            }
        }

        private void Negotiate_Write_End(IAsyncResult ar)
        {
            Negotiation_SO asyncState = (Negotiation_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                base.NStream.EndWrite(ar);
                this.BeginReadWhole(asyncState.Reply, 0, 2, new AsyncCallback(this.Negotiate_ReadWhole_End), asyncState);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
                asyncState.SetCompleted();
            }
        }

        private void Phase1_End(IAsyncResult ar)
        {
            ReadVerifyReply_SO asyncState = (ReadVerifyReply_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                this.EndReadWhole(ar);
                int size = this.VerifyReplyAndGetLeftBytes(asyncState.Phase1Data);
                asyncState.Reply = new byte[5 + size];
                asyncState.Phase1Data.CopyTo(asyncState.Reply, 0);
                this.BeginReadWhole(asyncState.Reply, 5, size, new AsyncCallback(this.Phase2_End), asyncState);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
                asyncState.SetCompleted();
            }
        }

        private void Phase2_End(IAsyncResult ar)
        {
            ReadVerifyReply_SO asyncState = (ReadVerifyReply_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                this.EndReadWhole(ar);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
            }
            asyncState.SetCompleted();
        }

        private byte[] Prepare_UsernamePasswordCmd()
        {
            if (base._proxyUser == null)
            {
                throw new ArgumentNullException("ProxyUser", "The value cannot be null.");
            }
            int length = base._proxyUser.Length;
            if (length > 0xff)
            {
                throw new ArgumentException("Proxy user name cannot be more then 255 characters.", "ProxyUser");
            }
            int num2 = 0;
            if (base._proxyPassword != null)
            {
                num2 = base._proxyPassword.Length;
                if (num2 > 0xff)
                {
                    throw new ArgumentException("Proxy password cannot be more then 255 characters.", "ProxyPassword");
                }
            }
            byte[] destinationArray = new byte[((2 + length) + 1) + num2];
            destinationArray[0] = 1;
            destinationArray[1] = (byte) length;
            Array.Copy(base._proxyUser, 0, destinationArray, 2, length);
            destinationArray[2 + length] = (byte) num2;
            if (num2 > 0)
            {
                Array.Copy(base._proxyPassword, 0, destinationArray, 3 + length, num2);
            }
            return destinationArray;
        }

        private byte[] PrepareBindCmd(Socket_Socks5 baseSocket)
        {
            if (baseSocket.RemoteEndPoint != null)
            {
                return this.PrepareCmd(baseSocket.RemoteEndPoint, 2);
            }
            if (baseSocket._remoteHost == null)
            {
                throw new InvalidOperationException("Unable to prepare bind command because of insufficient information.");
            }
            return this.PrepareCmd(baseSocket._remoteHost, baseSocket._remotePort, 2);
        }

        private byte[] PrepareCmd(EndPoint remoteEP, byte cmdVal)
        {
            if (remoteEP == null)
            {
                throw new ArgumentNullException("remoteEP", "The value cannot be null.");
            }
            byte[] buffer = new byte[10];
            IPEndPoint point = (IPEndPoint) remoteEP;
            buffer[0] = 5;
            buffer[1] = cmdVal;
            buffer[2] = 0;
            buffer[3] = 1;
            long address = point.Address.Address;
            buffer[4] = (byte) (address & 0xffL);
            buffer[5] = (byte) ((address & 0xff00L) >> 8);
            buffer[6] = (byte) ((address & 0xff0000L) >> 0x10);
            buffer[7] = (byte) ((address & 0xff000000L) >> 0x18);
            buffer[8] = (byte) ((point.Port & 0xff00) >> 8);
            buffer[9] = (byte) (point.Port & 0xff);
            return buffer;
        }

        private byte[] PrepareCmd(string remoteHost, int remotePort, byte cmdVal)
        {
            if (remoteHost == null)
            {
                throw new ArgumentNullException("remoteHost", "The value cannot be null.");
            }
            int length = remoteHost.Length;
            if (length > 0xff)
            {
                throw new ArgumentException("Name of destination host cannot be more then 255 characters.", "remoteHost");
            }
            byte[] destinationArray = new byte[(5 + length) + 2];
            destinationArray[0] = 5;
            destinationArray[1] = cmdVal;
            destinationArray[2] = 0;
            destinationArray[3] = 3;
            destinationArray[4] = (byte) length;
            Array.Copy(Encoding.Default.GetBytes(remoteHost), 0, destinationArray, 5, length);
            destinationArray[5 + length] = (byte) ((remotePort & 0xff00) >> 8);
            destinationArray[(5 + length) + 1] = (byte) (remotePort & 0xff);
            return destinationArray;
        }

        private byte[] PrepareConnectCmd(EndPoint remoteEP, string hostName, int hostPort)
        {
            if (remoteEP != null)
            {
                return this.PrepareCmd(remoteEP, 1);
            }
            if (hostName == null)
            {
                throw new InvalidOperationException("Unable to prepare connect command because of insufficient information.");
            }
            return this.PrepareCmd(hostName, hostPort, 1);
        }

        private byte[] PrepareNegotiationCmd(bool useCredentials)
        {
            if (useCredentials)
            {
                return new byte[] { 5, 2, 0, 2 };
            }
            return new byte[] { 5, 1, 0 };
        }

        private byte[] ReadVerifyReply()
        {
            byte[] buffer = new byte[5];
            this.ReadWhole(buffer, 0, 5);
            int size = this.VerifyReplyAndGetLeftBytes(buffer);
            byte[] array = new byte[5 + size];
            buffer.CopyTo(array, 0);
            this.ReadWhole(array, 5, size);
            return array;
        }

        private void ReadWhole(byte[] buffer, int offset, int size)
        {
            for (int i = 0; i < size; i += base.NStream.Read(buffer, offset + i, size - i))
            {
            }
        }

        private void ReadWhole_Read_End(IAsyncResult ar)
        {
            ReadWhole_SO asyncState = (ReadWhole_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                asyncState.Read += base.NStream.EndRead(ar);
                if (asyncState.Read < asyncState.Size)
                {
                    base.NStream.BeginRead(asyncState.Buffer, asyncState.Offset + asyncState.Read, asyncState.Size - asyncState.Read, new AsyncCallback(this.ReadWhole_Read_End), asyncState);
                }
                else
                {
                    asyncState.SetCompleted();
                }
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
                asyncState.SetCompleted();
            }
        }

        private void SubNegotiation_UsernamePassword()
        {
            byte[] buffer = this.Prepare_UsernamePasswordCmd();
            base.NStream.Write(buffer, 0, buffer.Length);
            byte[] buffer2 = new byte[2];
            this.ReadWhole(buffer2, 0, 2);
            this.Validate_UsernamePasswordReply(buffer2);
        }

        private void SubNegotiation_UsernamePassword_End(IAsyncResult ar)
        {
            DoAuthentication_SO asyncState = (DoAuthentication_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                this.EndSubNegotiation_UsernamePassword(ar);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
            }
            asyncState.SetCompleted();
        }

        private void SubUsernamePassword_Read_End(IAsyncResult ar)
        {
            UsernamePassword_SO asyncState = (UsernamePassword_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                this.EndReadWhole(ar);
                this.Validate_UsernamePasswordReply(asyncState.Reply);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
            }
            asyncState.SetCompleted();
        }

        private void SubUsernamePassword_Write_End(IAsyncResult ar)
        {
            UsernamePassword_SO asyncState = (UsernamePassword_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                base.NStream.EndWrite(ar);
                this.BeginReadWhole(asyncState.Reply, 0, 2, new AsyncCallback(this.SubUsernamePassword_Read_End), asyncState);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
                asyncState.SetCompleted();
            }
        }

        private void Validate_UsernamePasswordReply(byte[] reply)
        {
            if (1 != reply[0])
            {
                throw new ProtocolViolationException(string.Format("Socks5: Unknown reply format for username/password authentication ({0}).", reply[0]));
            }
            if (reply[1] != 0)
            {
                string.Format("Socks5: Username/password authentication failed ({0}).", reply[1]);
                throw new SocketException(0x274d);
            }
        }

        private int VerifyReplyAndGetLeftBytes(byte[] reply)
        {
            this.CheckReplyVer(reply);
            this.CheckReplyForError(reply);
            return ((this.GetAddrFieldLength(reply) - 1) + 2);
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
                return BytesRoad.Net.Sockets.ProxyType.Socks5;
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
            private Socket_Socks5 _baseSocket;
            private IPAddress _proxyIP;
            private int _readBytes;

            internal Bind_SO(Socket_Socks5 baseSocket, AsyncCallback cb, object state) : base(cb, state)
            {
                this._baseSocket = baseSocket;
            }

            internal Socket_Socks5 BaseSocket
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
            private string _hostName;
            private int _hostPort;
            private int _readBytes;
            private EndPoint _remoteEndPoint;

            internal Connect_SO(EndPoint remoteEndPoint, string hostName, int hostPort, AsyncCallback cb, object state) : base(cb, state)
            {
                this._hostPort = -1;
                this._remoteEndPoint = remoteEndPoint;
                this._hostPort = hostPort;
                this._hostName = hostName;
            }

            internal string HostName
            {
                get
                {
                    return this._hostName;
                }
            }

            internal int HostPort
            {
                get
                {
                    return this._hostPort;
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

        private class DoAuthentication_SO : AsyncResultBase
        {
            internal DoAuthentication_SO(AsyncCallback cb, object state) : base(cb, state)
            {
            }
        }

        private class Negotiation_SO : AsyncResultBase
        {
            private byte[] _reply;
            private bool _useCredentials;

            internal Negotiation_SO(bool useCredentials, AsyncCallback cb, object state) : base(cb, state)
            {
                this._reply = new byte[2];
                this._useCredentials = useCredentials;
            }

            internal byte[] Reply
            {
                get
                {
                    return this._reply;
                }
            }

            internal bool UseCredentials
            {
                get
                {
                    return this._useCredentials;
                }
                set
                {
                    this._useCredentials = value;
                }
            }
        }

        private class ReadVerifyReply_SO : AsyncResultBase
        {
            private byte[] _phase1Data;
            private byte[] _reply;

            internal ReadVerifyReply_SO(AsyncCallback cb, object state) : base(cb, state)
            {
                this._phase1Data = new byte[5];
            }

            internal byte[] Phase1Data
            {
                get
                {
                    return this._phase1Data;
                }
            }

            internal byte[] Reply
            {
                get
                {
                    return this._reply;
                }
                set
                {
                    this._reply = value;
                }
            }
        }

        private class ReadWhole_SO : AsyncResultBase
        {
            private byte[] _buffer;
            private int _offset;
            private int _read;
            private int _size;

            internal ReadWhole_SO(byte[] buffer, int offset, int size, AsyncCallback cb, object state) : base(cb, state)
            {
                this._buffer = buffer;
                this._offset = offset;
                this._size = size;
            }

            internal byte[] Buffer
            {
                get
                {
                    return this._buffer;
                }
            }

            internal int Offset
            {
                get
                {
                    return this._offset;
                }
            }

            internal int Read
            {
                get
                {
                    return this._read;
                }
                set
                {
                    this._read = value;
                }
            }

            internal int Size
            {
                get
                {
                    return this._size;
                }
            }
        }

        private class UsernamePassword_SO : AsyncResultBase
        {
            private byte[] _reply;

            internal UsernamePassword_SO(AsyncCallback cb, object state) : base(cb, state)
            {
                this._reply = new byte[2];
            }

            internal byte[] Reply
            {
                get
                {
                    return this._reply;
                }
            }
        }
    }
}

