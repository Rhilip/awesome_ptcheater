namespace BytesRoad.Net.Sockets.Advanced
{
    using BytesRoad.Diag;
    using System;
    using System.Threading;

    internal class AsyncBase
    {
        protected bool inProgress;

        internal AsyncBase()
        {
        }

        internal virtual void CheckProgress()
        {
            lock (this)
            {
                if (this.inProgress)
                {
                    throw new InvalidOperationException("Attempt to start operation which is already in the progress");
                }
            }
        }

        private void dumpActivityException(AsyncResultBase ar)
        {
            Exception exception = ar.Exception;
            int hashCode = Thread.CurrentThread.GetHashCode();
            NSTrace.WriteLineError(string.Format("{0} -----------------------------", hashCode));
            NSTrace.WriteLineError(string.Format("{0} Activity: {1}", hashCode, ar.ActivityName));
            NSTrace.WriteLineError(string.Format("{0} Exception: {1}", hashCode, exception.GetType().FullName));
            NSTrace.WriteLineError(string.Format("{0} Message: {1}", hashCode, exception.ToString()));
            NSTrace.WriteLineError(string.Format("{0} Stack: {1}", hashCode, exception.StackTrace));
            NSTrace.WriteLineError(string.Format("{0} -----------------------------", hashCode));
        }

        internal virtual void HandleAsyncEnd(IAsyncResult ar, bool turnProgressOff)
        {
            if (!ar.GetType().IsSubclassOf(typeof(AsyncResultBase)) && !ar.GetType().Equals(typeof(AsyncResultBase)))
            {
                throw new ArgumentException("asyncResult was not returned by a call to End* method.", "asyncResult");
            }
            AsyncResultBase base2 = (AsyncResultBase) ar;
            if (base2.IsHandled)
            {
                throw new InvalidOperationException("End* method was previously called for the asynchronous operation.");
            }
            if (!base2.IsCompleted)
            {
                base2.AsyncWaitHandle.WaitOne();
            }
            base2.IsHandled = true;
            if (turnProgressOff)
            {
                this.SetProgress(false);
            }
            if (base2.Exception != null)
            {
                throw base2.Exception;
            }
        }

        internal virtual void SetProgress(bool progress)
        {
            lock (this)
            {
                if (progress)
                {
                    if (this.inProgress)
                    {
                        throw new InvalidOperationException("Attempt to start operation which is already in the progress");
                    }
                    this.inProgress = true;
                }
                else
                {
                    this.inProgress = false;
                }
            }
        }

        internal static void VerifyAsyncResult(IAsyncResult ar, Type arType)
        {
            VerifyAsyncResult(ar, arType, null);
        }

        internal static void VerifyAsyncResult(IAsyncResult ar, Type arType, string metName)
        {
            if (ar == null)
            {
                throw new ArgumentNullException("asyncResult", "The value cannot be null.");
            }
            if (metName == null)
            {
                metName = "End*";
            }
            if (!ar.GetType().Equals(arType))
            {
                throw new ArgumentException("asyncResult was not returned by a call to the " + metName + " method.", "asyncResult");
            }
            AsyncResultBase base2 = (AsyncResultBase) ar;
            if (base2.IsHandled)
            {
                throw new InvalidOperationException(metName + " was previously called for the asynchronous operation.");
            }
        }
    }
}

