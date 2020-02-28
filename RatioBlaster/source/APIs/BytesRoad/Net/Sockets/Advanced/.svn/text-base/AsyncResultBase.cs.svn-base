namespace BytesRoad.Net.Sockets.Advanced
{
    using BytesRoad.Diag;
    using System;
    using System.Threading;

    internal class AsyncResultBase : IAsyncResult
    {
        private AsyncCallback _callback;
        private object _callerState;
        private bool _completedSync = true;
        private System.Exception _exception;
        private bool _isCompleted;
        private bool _isHandled;
        private int _startThreadId = -1;
        private ManualResetEvent _wait;

        internal AsyncResultBase(AsyncCallback cb, object callerState)
        {
            this._callback = cb;
            this._callerState = callerState;
            this._startThreadId = Thread.CurrentThread.GetHashCode();
        }

        private void CloseWaitHandle()
        {
            lock (this)
            {
                if (this._wait != null)
                {
                    this._wait.Close();
                    this._wait = null;
                }
            }
        }

        private void dumpActivityException()
        {
            System.Exception exception = this.Exception;
            if (exception != null)
            {
                int hashCode = Thread.CurrentThread.GetHashCode();
                NSTrace.WriteLineError((string.Format("{0} ---------- Start Exception Info -----------------------------\n", hashCode) + string.Format("{0} Activity: {1}\n", hashCode, this.ActivityName) + string.Format("{0} Stack: {1}\n", hashCode, Environment.StackTrace.ToString())) + string.Format("{0} Exception: {1}\n", hashCode, exception.ToString()) + string.Format("{0} ---------- End   Exception Info -----------------------------", hashCode));
            }
        }

        internal void SetCompleted()
        {
            lock (this)
            {
                this.UpdateContext();
                this._isCompleted = true;
                if (this._wait != null)
                {
                    this._wait.Set();
                }
            }
            this.dumpActivityException();
            try
            {
                if (this.CallBack != null)
                {
                    this.CallBack(this);
                }
            }
            catch (System.Exception exception)
            {
                NSTrace.WriteLineError("Exception in CB: " + exception.ToString());
                throw;
            }
            catch
            {
                NSTrace.WriteLineError("Non CLS exception in CB: " + Environment.StackTrace.ToString());
                throw;
            }
        }

        internal void UpdateContext()
        {
            if (Thread.CurrentThread.GetHashCode() != this._startThreadId)
            {
                this._completedSync = false;
            }
        }

        internal virtual string ActivityName
        {
            get
            {
                return base.GetType().FullName;
            }
        }

        public object AsyncState
        {
            get
            {
                return this._callerState;
            }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                lock (this)
                {
                    if (this._wait == null)
                    {
                        this._wait = new ManualResetEvent(this.IsCompleted);
                    }
                }
                return this._wait;
            }
        }

        internal AsyncCallback CallBack
        {
            get
            {
                return this._callback;
            }
        }

        public bool CompletedSynchronously
        {
            get
            {
                return this._completedSync;
            }
        }

        internal System.Exception Exception
        {
            get
            {
                return this._exception;
            }
            set
            {
                this._exception = value;
            }
        }

        public bool IsCompleted
        {
            get
            {
                return this._isCompleted;
            }
        }

        internal virtual bool IsHandled
        {
            get
            {
                return this._isHandled;
            }
            set
            {
                if (value)
                {
                    this.CloseWaitHandle();
                    this._callerState = null;
                    this._callback = null;
                }
                else
                {
                    NSTrace.WriteLineError("IsHandled assigned 'false'");
                }
                this._isHandled = value;
            }
        }
    }
}

