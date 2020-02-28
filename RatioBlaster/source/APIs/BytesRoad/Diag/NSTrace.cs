namespace BytesRoad.Diag
{
    using System;
    using System.Diagnostics;

    public class NSTrace
    {
        private static int _indentLevel;
        private static int _indentSize;

        protected NSTrace()
        {
        }

        [Conditional("TRACE")]
        public static void Close()
        {
            if (NSTraceOptions.UseSystemTrace)
            {
                Trace.Flush();
            }
            foreach (TraceListener listener in Listeners)
            {
                listener.Flush();
                listener.Close();
            }
            Listeners.Clear();
        }

        [Conditional("TRACE")]
        public static void Flush()
        {
            if (NSTraceOptions.UseSystemTrace)
            {
                Trace.Flush();
            }
            foreach (TraceListener listener in Listeners)
            {
                listener.Flush();
            }
        }

        internal static void FlushIfNeeded()
        {
            if (AutoFlush)
            {
                Flush();
            }
        }

        [Conditional("TRACE")]
        public static void Indent()
        {
            if (NSTraceOptions.UseSystemTrace)
            {
                Trace.Indent();
            }
            foreach (TraceListener listener in Listeners)
            {
                listener.IndentLevel++;
            }
        }

        [Conditional("TRACE")]
        public static void Unindent()
        {
            if (NSTraceOptions.UseSystemTrace)
            {
                Trace.Unindent();
            }
            foreach (TraceListener listener in Listeners)
            {
                listener.IndentLevel--;
            }
        }

        [Conditional("TRACE")]
        public static void Write(object value)
        {
            if (NSTraceOptions.UseSystemTrace)
            {
                Trace.Write(value);
            }
            foreach (TraceListener listener in Listeners)
            {
                listener.Write(value);
            }
            FlushIfNeeded();
        }

        [Conditional("TRACE")]
        public static void Write(string message)
        {
            if (NSTraceOptions.UseSystemTrace)
            {
                Trace.Write(message);
            }
            foreach (TraceListener listener in Listeners)
            {
                listener.Write(message);
            }
            FlushIfNeeded();
        }

        [Conditional("TRACE")]
        public static void Write(object value, string category)
        {
            if (NSTraceOptions.UseSystemTrace)
            {
                Trace.Write(value, category);
            }
            foreach (TraceListener listener in Listeners)
            {
                listener.Write(value, category);
            }
            FlushIfNeeded();
        }

        [Conditional("TRACE")]
        public static void Write(string message, string category)
        {
            if (NSTraceOptions.UseSystemTrace)
            {
                Trace.Write(message, category);
            }
            foreach (TraceListener listener in Listeners)
            {
                listener.Write(message, category);
            }
            FlushIfNeeded();
        }

        [Conditional("TRACE")]
        public static void WriteIf(bool condition, object value)
        {
            if (condition)
            {
                Write(value);
            }
        }

        [Conditional("TRACE")]
        public static void WriteIf(bool condition, string message)
        {
            if (condition)
            {
                Write(condition, message);
            }
        }

        [Conditional("TRACE")]
        public static void WriteIf(bool condition, object value, string category)
        {
            if (condition)
            {
                Write(value, category);
            }
        }

        [Conditional("TRACE")]
        public static void WriteIf(bool condition, string message, string category)
        {
            if (condition)
            {
                Write(message, category);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLine(object value)
        {
            if (NSTraceOptions.UseSystemTrace)
            {
                Trace.WriteLine(value);
            }
            foreach (TraceListener listener in Listeners)
            {
                listener.WriteLine(value);
            }
            FlushIfNeeded();
        }

        [Conditional("TRACE")]
        public static void WriteLine(string message)
        {
            if (NSTraceOptions.UseSystemTrace)
            {
                Trace.WriteLine(message);
            }
            foreach (TraceListener listener in Listeners)
            {
                listener.WriteLine(message);
            }
            FlushIfNeeded();
        }

        [Conditional("TRACE")]
        public static void WriteLine(object value, string category)
        {
            if (NSTraceOptions.UseSystemTrace)
            {
                Trace.WriteLine(value, category);
            }
            foreach (TraceListener listener in Listeners)
            {
                listener.WriteLine(value, category);
            }
            FlushIfNeeded();
        }

        [Conditional("TRACE")]
        public static void WriteLine(string message, string category)
        {
            if (NSTraceOptions.UseSystemTrace)
            {
                Trace.WriteLine(message, category);
            }
            foreach (TraceListener listener in Listeners)
            {
                listener.WriteLine(message, category);
            }
            FlushIfNeeded();
        }

        [Conditional("TRACE")]
        public static void WriteLineError(object value)
        {
            if (NSTraceOptions.TraceError)
            {
                WriteLine(value);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineError(string message)
        {
            if (NSTraceOptions.TraceError)
            {
                WriteLine(message);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineError(object value, string category)
        {
            if (NSTraceOptions.TraceError)
            {
                WriteLine(value, category);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineError(string message, string category)
        {
            if (NSTraceOptions.TraceError)
            {
                WriteLine(message, category);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineIf(bool condition, object value)
        {
            if (condition)
            {
                WriteLine(value);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineIf(bool condition, string message)
        {
            if (condition)
            {
                WriteLine(message);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineIf(bool condition, object value, string category)
        {
            if (condition)
            {
                WriteLine(value, category);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineIf(bool condition, string message, string category)
        {
            if (condition)
            {
                WriteLine(message, category);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineInfo(object value)
        {
            if (NSTraceOptions.TraceInfo)
            {
                WriteLine(value);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineInfo(string message)
        {
            if (NSTraceOptions.TraceInfo)
            {
                WriteLine(message);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineInfo(object value, string category)
        {
            if (NSTraceOptions.TraceInfo)
            {
                WriteLine(value, category);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineInfo(string message, string category)
        {
            if (NSTraceOptions.TraceInfo)
            {
                WriteLine(message, category);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineVerbose(object value)
        {
            if (NSTraceOptions.TraceVerbose)
            {
                WriteLine(value);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineVerbose(string message)
        {
            if (NSTraceOptions.TraceVerbose)
            {
                WriteLine(message);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineVerbose(object value, string category)
        {
            if (NSTraceOptions.TraceVerbose)
            {
                WriteLine(value, category);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineVerbose(string message, string category)
        {
            if (NSTraceOptions.TraceVerbose)
            {
                WriteLine(message, category);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineWarning(object value)
        {
            if (NSTraceOptions.TraceWarning)
            {
                WriteLine(value);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineWarning(string message)
        {
            if (NSTraceOptions.TraceWarning)
            {
                WriteLine(message);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineWarning(object value, string category)
        {
            if (NSTraceOptions.TraceWarning)
            {
                WriteLine(value, category);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineWarning(string message, string category)
        {
            if (NSTraceOptions.TraceWarning)
            {
                WriteLine(message, category);
            }
        }

        public static bool AutoFlush
        {
            get
            {
                return NSTraceOptions.AutoFlush;
            }
            set
            {
                NSTraceOptions.AutoFlush = value;
            }
        }

        public static int IndentLevel
        {
            get
            {
                return _indentLevel;
            }
            set
            {
                foreach (TraceListener listener in Listeners)
                {
                    listener.IndentLevel = value;
                }
            }
        }

        public static int IndentSize
        {
            get
            {
                return _indentSize;
            }
            set
            {
                foreach (TraceListener listener in Listeners)
                {
                    listener.IndentSize = value;
                }
            }
        }

        public static NSTraceListeners Listeners
        {
            get
            {
                return NSTraceOptions.Listeners;
            }
        }
    }
}

