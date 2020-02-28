namespace BytesRoad.Diag
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;

    public class NSTraceListeners : IList, ICollection, IEnumerable
    {
        private ArrayList _listeners = new ArrayList();

        internal NSTraceListeners()
        {
        }

        public int Add(TraceListener listener)
        {
            return this._listeners.Add(listener);
        }

        public void AddRange(NSTraceListeners listeners)
        {
            this._listeners.AddRange(listeners);
        }

        public void AddRange(TraceListenerCollection listeners)
        {
            this._listeners.AddRange(listeners);
        }

        public void AddRange(TraceListener[] listeners)
        {
            this._listeners.AddRange(listeners);
        }

        public virtual void Clear()
        {
            this._listeners.Clear();
        }

        public bool Contains(TraceListener listener)
        {
            return this._listeners.Contains(listener);
        }

        public void CopyTo(Array array, int index)
        {
        }

        public void CopyTo(TraceListener[] listeners, int index)
        {
            this._listeners.CopyTo(index, listeners, 0, this._listeners.Count);
        }

        public virtual IEnumerator GetEnumerator()
        {
            return this._listeners.GetEnumerator();
        }

        public int IndexOf(TraceListener listener)
        {
            return this._listeners.IndexOf(listener);
        }

        public void Insert(int index, TraceListener listener)
        {
            this._listeners.Insert(index, listener);
        }

        public void Remove(TraceListener listener)
        {
            if (!this.Contains(listener))
            {
                throw new ArgumentException("The listener does not exist in the list.", "listener");
            }
            this._listeners.Remove(listener);
        }

        public void Remove(string name)
        {
            TraceListener listener = this[name];
            if (listener == null)
            {
                throw new ArgumentException("A listener with the given name does not exist in the list.", "name");
            }
            this._listeners.Remove(listener);
        }

        public virtual void RemoveAt(int index)
        {
            this._listeners.RemoveAt(index);
        }

        int IList.Add(object val)
        {
            return this.Add((TraceListener) val);
        }

        bool IList.Contains(object val)
        {
            return this.Contains((TraceListener) val);
        }

        int IList.IndexOf(object val)
        {
            return this.IndexOf((TraceListener) val);
        }

        void IList.Insert(int index, object val)
        {
            this.Insert(index, (TraceListener) val);
        }

        void IList.Remove(object val)
        {
            this.Remove((TraceListener) val);
        }

        public virtual int Count
        {
            get
            {
                return this._listeners.Count;
            }
        }

        public virtual bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public virtual bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public virtual bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public TraceListener this[string name]
        {
            get
            {
                lock (this._listeners)
                {
                    foreach (TraceListener listener in this._listeners)
                    {
                        if (name.Equals(listener.Name))
                        {
                            return listener;
                        }
                    }
                }
                return null;
            }
        }

        public TraceListener this[int index]
        {
            get
            {
                return (TraceListener) this._listeners[index];
            }
            set
            {
                this._listeners[index] = value;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return this;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = (TraceListener) value;
            }
        }
    }
}

