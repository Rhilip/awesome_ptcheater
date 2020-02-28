namespace BytesRoad.Net.Sockets
{
    using System;

    internal abstract class IOp
    {
        protected IOp()
        {
        }

        internal abstract object BeginExecute(AsyncCallback cb, object state);
        internal abstract object EndExecute(IAsyncResult ar);
        internal abstract object Execute();
    }
}

