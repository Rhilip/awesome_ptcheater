namespace BytesRoad.Net.Sockets
{
    using BytesRoad.Net.Sockets.Advanced;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class Socket_HttpConnect : SocketBase
    {
        private int _maxReplySize;
        private ByteVector _recvBuffer;
        private EndPoint _remoteEndPoint;
        private Regex _replyRegEx;

        internal Socket_HttpConnect(string proxyServer, int proxyPort, byte[] proxyUser, byte[] proxyPassword) : base(proxyServer, proxyPort, proxyUser, proxyPassword)
        {
            this._recvBuffer = new ByteVector();
            this._maxReplySize = 0x1000;
            this._replyRegEx = new Regex(@"^HTTP/\d+\.\d+ (?<code>\d\d\d) ?(?<reason>[^\r\n]*)?(\r)?\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        internal override SocketBase Accept()
        {
            this.ThrowUnsupportException("Accept");
            return null;
        }

        private int AnalyzeReply(ByteVector reply, out string reason)
        {
            if (reply.Size == 0)
            {
                throw new SocketException(0x274d);
            }
            string input = Encoding.ASCII.GetString(reply.Data, 0, reply.Size);
            Match match = this._replyRegEx.Match(input);
            if ((reply.Size < 14) || (match.Groups.Count != 4))
            {
                throw new ProtocolViolationException("Web proxy reply is incorrect.");
            }
            int num = int.Parse(match.Groups["code"].Value);
            reason = match.Groups["reason"].Value;
            return num;
        }

        internal override IAsyncResult BeginAccept(AsyncCallback callback, object state)
        {
            this.ThrowUnsupportException("BeginAccept");
            return null;
        }

        internal override IAsyncResult BeginBind(SocketBase baseSocket, AsyncCallback callback, object state)
        {
            this.ThrowUnsupportException("BeginBind");
            return null;
        }

        internal override IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state)
        {
            if (remoteEP == null)
            {
                throw new ArgumentNullException("remoteEP", "The value cannot be null.");
            }
            IPEndPoint point = (IPEndPoint) remoteEP;
            string hostName = point.Address.ToString();
            int port = point.Port;
            return this.BeginConnect(hostName, port, callback, state);
        }

        internal override IAsyncResult BeginConnect(string hostName, int hostPort, AsyncCallback callback, object state)
        {
            Connect_SO t_so = null;
            base.CheckDisposed();
            if (hostName == null)
            {
                throw new ArgumentNullException("hostName", "The value cannot be null.");
            }
            if ((hostPort < 0) || (hostPort > 0xffff))
            {
                throw new ArgumentOutOfRangeException("hostPort", "Value, specified for the port is out of the valid range.");
            }
            this.SetProgress(true);
            try
            {
                t_so = new Connect_SO(hostName, hostPort, base.PreAuthenticate, callback, state);
                SocketBase.BeginGetHostByName(base._proxyServer, new AsyncCallback(this.Connect_GetPrxHost_End), t_so);
            }
            catch (Exception exception)
            {
                this.SetProgress(false);
                throw exception;
            }
            return t_so;
        }

        private IAsyncResult BeginReadReply(AsyncCallback cb, object state)
        {
            ReadReply_SO y_so = new ReadReply_SO(cb, state);
            this.BeginReceive(y_so.Buffer, 0, y_so.Buffer.Length, new AsyncCallback(this.ReadReply_Recv_End), y_so);
            return y_so;
        }

        internal override IAsyncResult BeginReceive(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            Receive_SO e_so = null;
            base.CheckDisposed();
            e_so = new Receive_SO(callback, state);
            if ((e_so.Read = this.FetchBufferData(buffer, offset, size)) > 0)
            {
                e_so.SetCompleted();
                return e_so;
            }
            base._socket.BeginReceive(buffer, offset, size, SocketFlags.None, new AsyncCallback(this.Receive_End), e_so);
            return e_so;
        }

        internal override void Bind(SocketBase baseSocket)
        {
            this.ThrowUnsupportException("Bind");
        }

        internal override void Connect(EndPoint remoteEP)
        {
            if (remoteEP == null)
            {
                throw new ArgumentNullException("remoteEP", "The value cannot be null.");
            }
            IPEndPoint point = (IPEndPoint) remoteEP;
            string hostName = point.Address.ToString();
            int port = point.Port;
            this.Connect(hostName, port);
        }

        internal override void Connect(string hostName, int hostPort)
        {
            base.CheckDisposed();
            this.SetProgress(true);
            try
            {
                byte[] buffer;
                if (hostName == null)
                {
                    throw new ArgumentNullException("hostName", "The value cannot be null.");
                }
                if ((hostPort < 0) || (hostPort > 0xffff))
                {
                    throw new ArgumentOutOfRangeException("hostPort", "Value, specified for the port is out of the valid range.");
                }
                IPHostEntry hostByName = SocketBase.GetHostByName(base._proxyServer);
                if (hostByName == null)
                {
                    throw new SocketException(0x2af9);
                }
                IPEndPoint remoteEP = SocketBase.ConstructEndPoint(hostByName, base._proxyPort);
                base._socket.Connect(remoteEP);
                bool preAuthenticate = base.PreAuthenticate;
            Label_0076:
                buffer = this.GetConnectCmd(hostName, hostPort, preAuthenticate);
                base.NStream.Write(buffer, 0, buffer.Length);
                ByteVector reply = this.ReadReply();
                string reason = null;
                int num = this.AnalyzeReply(reply, out reason);
                if ((num < 200) || (num > 0x12b))
                {
                    if (((0x197 != num) || preAuthenticate) || (base._proxyUser == null))
                    {
                        throw new SocketException(0x274d);
                    }
                    preAuthenticate = true;
                    goto Label_0076;
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
                byte[] buffer = this.GetConnectCmd(asyncState.HostName, asyncState.HostPort, asyncState.UseCredentials);
                base.NStream.BeginWrite(buffer, 0, buffer.Length, new AsyncCallback(this.Connect_Write_End), asyncState);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
                asyncState.SetCompleted();
            }
        }

        private void Connect_GetPrxHost_End(IAsyncResult ar)
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

        private void Connect_ReadReply_End(IAsyncResult ar)
        {
            Connect_SO asyncState = (Connect_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                ByteVector reply = this.EndReadReply(ar);
                string reason = null;
                int num = this.AnalyzeReply(reply, out reason);
                if ((num < 200) || (num > 0x12b))
                {
                    if (((0x197 != num) || asyncState.UseCredentials) || (base._proxyUser == null))
                    {
                        throw new SocketException(0x274d);
                    }
                    asyncState.UseCredentials = true;
                    byte[] buffer = this.GetConnectCmd(asyncState.HostName, asyncState.HostPort, asyncState.UseCredentials);
                    base.NStream.BeginWrite(buffer, 0, buffer.Length, new AsyncCallback(this.Connect_Write_End), asyncState);
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

        private void Connect_Write_End(IAsyncResult ar)
        {
            Connect_SO asyncState = (Connect_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                base.NStream.EndWrite(ar);
                this.BeginReadReply(new AsyncCallback(this.Connect_ReadReply_End), asyncState);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
                asyncState.SetCompleted();
            }
        }

        internal override SocketBase EndAccept(IAsyncResult asyncResult)
        {
            this.ThrowUnsupportException("EndAccept");
            return null;
        }

        internal override void EndBind(IAsyncResult ar)
        {
            this.ThrowUnsupportException("EndBind");
        }

        internal override void EndConnect(IAsyncResult asyncResult)
        {
            AsyncBase.VerifyAsyncResult(asyncResult, typeof(Connect_SO), "EndConnect");
            this.HandleAsyncEnd(asyncResult, true);
        }

        private ByteVector EndReadReply(IAsyncResult ar)
        {
            AsyncBase.VerifyAsyncResult(ar, typeof(ReadReply_SO));
            this.HandleAsyncEnd(ar, false);
            return ((ReadReply_SO) ar).Reply;
        }

        internal override int EndReceive(IAsyncResult asyncResult)
        {
            AsyncBase.VerifyAsyncResult(asyncResult, typeof(Receive_SO), "EndReceive");
            this.HandleAsyncEnd(asyncResult, false);
            return ((Receive_SO) asyncResult).Read;
        }

        private int FetchBufferData(byte[] buffer, int offset, int size)
        {
            int num = this._recvBuffer.Size;
            if (num <= 0)
            {
                return 0;
            }
            if (offset < 0)
            {
                offset = 0;
            }
            if (size < 0)
            {
                size = buffer.Length;
            }
            int length = (num > size) ? size : num;
            Array.Copy(this._recvBuffer.Data, 0, buffer, offset, length);
            this._recvBuffer.CutHead(length);
            return length;
        }

        private int FindReplyEnd(byte[] buf, int size)
        {
            if (size >= 2)
            {
                for (int i = 0; i < size; i++)
                {
                    bool flag = false;
                    bool flag2 = false;
                    int num2 = size - i;
                    if (num2 >= 2)
                    {
                        flag2 = true;
                        if (num2 >= 4)
                        {
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        if (((buf[i] == 13) && (buf[i + 1] == 10)) && ((buf[i + 2] == 13) && (buf[i + 3] == 10)))
                        {
                            return (i + 4);
                        }
                    }
                    else
                    {
                        if (!flag2)
                        {
                            break;
                        }
                        if ((buf[i] == 10) && (buf[i + 1] == 10))
                        {
                            return (i + 2);
                        }
                    }
                }
            }
            return -1;
        }

        private string GetBasicCredentials()
        {
            int destinationIndex = 1;
            if (base._proxyUser != null)
            {
                destinationIndex += base._proxyUser.Length;
            }
            if (base._proxyPassword != null)
            {
                destinationIndex += base._proxyPassword.Length;
            }
            byte[] destinationArray = new byte[destinationIndex];
            destinationIndex = 0;
            if (base._proxyUser != null)
            {
                Array.Copy(base._proxyUser, 0, destinationArray, 0, base._proxyUser.Length);
                destinationIndex += base._proxyUser.Length;
            }
            destinationArray[destinationIndex++] = 0x3a;
            if (base._proxyPassword != null)
            {
                Array.Copy(base._proxyPassword, 0, destinationArray, destinationIndex, base._proxyPassword.Length);
                destinationIndex += base._proxyPassword.Length;
            }
            return Convert.ToBase64String(destinationArray);
        }

        private byte[] GetConnectCmd(string hostName, int hostPort, bool useCredentials)
        {
            string s = string.Format("CONNECT {0}:{1} HTTP/1.1\r\n", hostName, hostPort) + string.Format("Host: {0}:{1}\r\n", hostName, hostPort);
            if (useCredentials)
            {
                string basicCredentials = this.GetBasicCredentials();
                s = (s + "Authorization: basic " + basicCredentials + "\r\n") + "Proxy-Authorization: basic " + basicCredentials + "\r\n";
            }
            s = s + "\r\n";
            return Encoding.ASCII.GetBytes(s);
        }

        internal override void Listen(int backlog)
        {
            this.ThrowUnsupportException("Listen");
        }

        private void PutBufferData(byte[] buffer, int offset, int size)
        {
            if (this._recvBuffer.Size != 0)
            {
                throw new InvalidOperationException("PutBufferData: buffer is not empty.");
            }
            this._recvBuffer.Add(buffer, offset, size);
        }

        private ByteVector ReadReply()
        {
            ByteVector vector = new ByteVector();
            byte[] buffer = new byte[0x200];
            do
            {
                int length = this.Receive(buffer);
                if (length == 0)
                {
                    return vector;
                }
                vector.Add(buffer, 0, length);
                int offset = this.FindReplyEnd(vector.Data, vector.Size);
                if (offset > 0)
                {
                    if (offset < length)
                    {
                        this.PutBufferData(buffer, offset, length - offset);
                        vector.CutTail(length - offset);
                    }
                    return vector;
                }
            }
            while (vector.Size <= this._maxReplySize);
            throw new ProtocolViolationException("Web proxy reply exceed maximum length.");
        }

        private void ReadReply_Recv_End(IAsyncResult ar)
        {
            ReadReply_SO asyncState = (ReadReply_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                int length = this.EndReceive(ar);
                if (length == 0)
                {
                    asyncState.SetCompleted();
                }
                else
                {
                    asyncState.Reply.Add(asyncState.Buffer, 0, length);
                    int offset = this.FindReplyEnd(asyncState.Reply.Data, asyncState.Reply.Size);
                    if (offset > 0)
                    {
                        if (offset < length)
                        {
                            this.PutBufferData(asyncState.Buffer, offset, length - offset);
                            asyncState.Reply.CutTail(length - offset);
                        }
                        asyncState.SetCompleted();
                    }
                    else
                    {
                        if (asyncState.Reply.Size > this._maxReplySize)
                        {
                            throw new ProtocolViolationException("Web proxy reply exceed maximum length.");
                        }
                        this.BeginReceive(asyncState.Buffer, 0, asyncState.Buffer.Length, new AsyncCallback(this.ReadReply_Recv_End), asyncState);
                    }
                }
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
                asyncState.SetCompleted();
            }
        }

        internal override int Receive(byte[] buffer)
        {
            base.CheckDisposed();
            int num = this.FetchBufferData(buffer, -1, -1);
            if (num > 0)
            {
                return num;
            }
            return base.Receive(buffer);
        }

        internal override int Receive(byte[] buffer, int size)
        {
            base.CheckDisposed();
            int num = this.FetchBufferData(buffer, -1, size);
            if (num > 0)
            {
                return num;
            }
            return base.Receive(buffer, size);
        }

        internal override int Receive(byte[] buffer, int offset, int size)
        {
            base.CheckDisposed();
            int num = this.FetchBufferData(buffer, offset, size);
            if (num > 0)
            {
                return num;
            }
            return base.Receive(buffer, offset, size);
        }

        private void Receive_End(IAsyncResult ar)
        {
            Receive_SO asyncState = (Receive_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                asyncState.Read = base._socket.EndReceive(ar);
            }
            catch (Exception exception)
            {
                asyncState.Exception = exception;
            }
            asyncState.SetCompleted();
        }

        private void ThrowUnsupportException(string method)
        {
            throw new InvalidOperationException(string.Format("'{0}' command is not possible with Web proxy.", method));
        }

        internal override int Available
        {
            get
            {
                int size = this._recvBuffer.Size;
                if (size > 0)
                {
                    return size;
                }
                return base._socket.Available;
            }
        }

        internal override EndPoint LocalEndPoint
        {
            get
            {
                return null;
            }
        }

        internal override BytesRoad.Net.Sockets.ProxyType ProxyType
        {
            get
            {
                return BytesRoad.Net.Sockets.ProxyType.HttpConnect;
            }
        }

        internal override EndPoint RemoteEndPoint
        {
            get
            {
                return this._remoteEndPoint;
            }
        }

        private class Connect_SO : AsyncResultBase
        {
            private string _hostName;
            private int _hostPort;
            private bool _useCredentials;

            internal Connect_SO(string hostName, int hostPort, bool useCredentials, AsyncCallback cb, object state) : base(cb, state)
            {
                this._useCredentials = useCredentials;
                this._hostName = hostName;
                this._hostPort = hostPort;
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

        private class ReadReply_SO : AsyncResultBase
        {
            private byte[] _buffer;
            private ByteVector _reply;

            internal ReadReply_SO(AsyncCallback cb, object state) : base(cb, state)
            {
                this._buffer = new byte[0x200];
                this._reply = new ByteVector();
            }

            internal byte[] Buffer
            {
                get
                {
                    return this._buffer;
                }
            }

            internal ByteVector Reply
            {
                get
                {
                    return this._reply;
                }
            }
        }

        private class Receive_SO : AsyncResultBase
        {
            private int _read;

            internal Receive_SO(AsyncCallback cb, object state) : base(cb, state)
            {
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
        }
    }
}

