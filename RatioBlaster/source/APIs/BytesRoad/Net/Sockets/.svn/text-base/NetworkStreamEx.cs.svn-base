namespace BytesRoad.Net.Sockets
{
    using BytesRoad.Net.Sockets.Advanced;
    using System;
    using System.IO;
    using System.Net.Sockets;

    public class NetworkStreamEx : Stream
    {
        private FileAccess _access;
        private AsyncBase _asyncCtx;
        private bool _disposed;
        private bool _ownsSocket;
        private SocketEx _socket;

        public NetworkStreamEx(SocketEx socket)
        {
            this._asyncCtx = new AsyncBase();
            this._access = FileAccess.ReadWrite;
            this._socket = socket;
        }

        public NetworkStreamEx(SocketEx socket, bool ownsSocket)
        {
            this._asyncCtx = new AsyncBase();
            this._access = FileAccess.ReadWrite;
            this._socket = socket;
            this._ownsSocket = ownsSocket;
        }

        public NetworkStreamEx(SocketEx socket, FileAccess access)
        {
            this._asyncCtx = new AsyncBase();
            this._access = FileAccess.ReadWrite;
            this._socket = socket;
            this._access = access;
        }

        public NetworkStreamEx(SocketEx socket, FileAccess access, bool ownsSocket)
        {
            this._asyncCtx = new AsyncBase();
            this._access = FileAccess.ReadWrite;
            this._socket = socket;
            this._ownsSocket = ownsSocket;
            this._access = access;
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            IAsyncResult result;
            this.CheckDisposed();
            this._asyncCtx.SetProgress(true);
            try
            {
                Read_SO d_so = new Read_SO(buffer, offset, size, callback, state);
                result = this._socket.BeginReceive(buffer, offset, size, new AsyncCallback(this.Read_End), d_so);
            }
            catch
            {
                this._asyncCtx.SetProgress(false);
                this.CheckDisposed();
                throw;
            }
            return result;
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            IAsyncResult result;
            this.CheckDisposed();
            this._asyncCtx.SetProgress(true);
            try
            {
                Write_SO e_so = new Write_SO(buffer, offset, size, callback, state);
                result = this._socket.BeginSend(buffer, offset, size, new AsyncCallback(this.Send_End), e_so);
            }
            catch
            {
                this._asyncCtx.SetProgress(false);
                this.CheckDisposed();
                throw;
            }
            return result;
        }

        private void CheckDisposed()
        {
            if (this._disposed)
            {
                throw this.GetDisposedException();
            }
        }

        public override void Close()
        {
            this.Dispose();
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
                    this._disposed = true;
                    if (this._ownsSocket)
                    {
                        this._socket.Dispose();
                    }
                }
            }
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            AsyncBase.VerifyAsyncResult(asyncResult, typeof(Read_SO), "EndRead");
            this._asyncCtx.HandleAsyncEnd(asyncResult, true);
            Read_SO d_so = (Read_SO) asyncResult;
            return d_so.Read;
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            AsyncBase.VerifyAsyncResult(asyncResult, typeof(Write_SO), "EndWrite");
            this._asyncCtx.HandleAsyncEnd(asyncResult, true);
        }

        ~NetworkStreamEx()
        {
            this.Dispose(false);
        }

        public override void Flush()
        {
            this.CheckDisposed();
        }

        private Exception GetDisposedException()
        {
            return new ObjectDisposedException(base.GetType().FullName, "Object was disposed.");
        }

        public override int Read(byte[] buffer, int offset, int size)
        {
            int num3;
            this.CheckDisposed();
            this._asyncCtx.SetProgress(true);
            try
            {
                int num = 0;
                int num2 = 1;
                while ((num2 > 0) && (num < size))
                {
                    num2 = this._socket.Receive(buffer, offset + num, size - num);
                    num += num2;
                }
                num3 = num;
            }
            catch
            {
                this.CheckDisposed();
                throw;
            }
            finally
            {
                this._asyncCtx.SetProgress(false);
            }
            return num3;
        }

        private void Read_End(IAsyncResult ar)
        {
            Read_SO asyncState = (Read_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                int num = this._socket.EndReceive(ar);
                asyncState.Read += num;
                if ((num > 0) && (asyncState.Read < asyncState.Size))
                {
                    this._socket.BeginReceive(asyncState.Buffer, asyncState.Offset + asyncState.Read, asyncState.Size - asyncState.Read, new AsyncCallback(this.Read_End), asyncState);
                }
                else
                {
                    asyncState.SetCompleted();
                }
            }
            catch (Exception exception)
            {
                if (this._disposed)
                {
                    asyncState.Exception = this.GetDisposedException();
                }
                else
                {
                    asyncState.Exception = exception;
                }
                asyncState.SetCompleted();
            }
            catch
            {
                if (this._disposed)
                {
                    asyncState.Exception = this.GetDisposedException();
                }
                else
                {
                    asyncState.Exception = new SocketException(0x2746);
                }
                asyncState.SetCompleted();
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            this.CheckDisposed();
            this.ThrowMetUnsupported("Seek");
            return 0L;
        }

        private void Send_End(IAsyncResult ar)
        {
            Write_SO asyncState = (Write_SO) ar.AsyncState;
            try
            {
                asyncState.UpdateContext();
                int num = this._socket.EndSend(ar);
                asyncState.Sent += num;
                if (asyncState.Sent < asyncState.Size)
                {
                    this._socket.BeginSend(asyncState.Buffer, asyncState.Offset + asyncState.Sent, asyncState.Size - asyncState.Sent, new AsyncCallback(this.Send_End), asyncState);
                }
                else
                {
                    asyncState.SetCompleted();
                }
            }
            catch (Exception exception)
            {
                if (this._disposed)
                {
                    asyncState.Exception = this.GetDisposedException();
                }
                else
                {
                    asyncState.Exception = exception;
                }
                asyncState.SetCompleted();
            }
            catch
            {
                if (this._disposed)
                {
                    asyncState.Exception = this.GetDisposedException();
                }
                else
                {
                    asyncState.Exception = new SocketException(0x2746);
                }
                asyncState.SetCompleted();
            }
        }

        public override void SetLength(long value)
        {
            this.CheckDisposed();
            this.ThrowMetUnsupported("SetLength");
        }

        private void ThrowMetUnsupported(string metName)
        {
            throw new NotSupportedException(string.Format("'{0}' method is not supported.", metName));
        }

        private void ThrowPropUnsupported(string propName)
        {
            throw new NotSupportedException(string.Format("'{0}' property is not supported.", propName));
        }

        public override void Write(byte[] buffer, int offset, int size)
        {
            this.CheckDisposed();
            this._asyncCtx.SetProgress(true);
            try
            {
                for (int i = 0; i < size; i += this._socket.Send(buffer, offset + i, size - i))
                {
                }
            }
            catch
            {
                this.CheckDisposed();
                throw;
            }
            finally
            {
                this._asyncCtx.SetProgress(false);
            }
        }

        public override bool CanRead
        {
            get
            {
                return ((this._access & FileAccess.Read) == FileAccess.Read);
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return ((this._access & FileAccess.Write) == FileAccess.Write);
            }
        }

        public virtual bool DataAvailable
        {
            get
            {
                bool flag;
                this.CheckDisposed();
                try
                {
                    flag = this._socket.Available > 0;
                }
                catch
                {
                    this.CheckDisposed();
                    throw;
                }
                return flag;
            }
        }

        public override long Length
        {
            get
            {
                this.ThrowPropUnsupported("Length");
                return 0L;
            }
        }

        public override long Position
        {
            get
            {
                this.ThrowPropUnsupported("Position");
                return 0L;
            }
            set
            {
                this.ThrowPropUnsupported("Position");
            }
        }

        private class Read_SO : AsyncResultBase
        {
            private byte[] _buffer;
            private int _offset;
            private int _read;
            private int _size;

            internal Read_SO(byte[] buffer, int offset, int size, AsyncCallback cb, object state) : base(cb, state)
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

        private class Write_SO : AsyncResultBase
        {
            private byte[] _buffer;
            private int _offset;
            private int _sent;
            private int _size;

            internal Write_SO(byte[] buffer, int offset, int size, AsyncCallback cb, object state) : base(cb, state)
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

            internal int Sent
            {
                get
                {
                    return this._sent;
                }
                set
                {
                    this._sent = value;
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
    }
}

