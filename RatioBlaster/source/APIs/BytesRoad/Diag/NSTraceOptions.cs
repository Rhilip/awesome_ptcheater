namespace BytesRoad.Diag
{
    using System;
    using System.Diagnostics;

    public class NSTraceOptions
    {
        private static bool _autoFlush = false;
        private static TraceLevel _level = TraceLevel.Off;
        private static NSTraceListeners _listeners = new NSTraceListeners();
        private static bool _useSystemTrace = false;

        private NSTraceOptions()
        {
        }

        public static bool AutoFlush
        {
            get
            {
                return _autoFlush;
            }
            set
            {
                _autoFlush = value;
            }
        }

        public static TraceLevel Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
            }
        }

        public static NSTraceListeners Listeners
        {
            get
            {
                return _listeners;
            }
        }

        public static bool TraceError
        {
            get
            {
                if (((_level != TraceLevel.Error) && (_level != TraceLevel.Warning)) && (_level != TraceLevel.Info))
                {
                    return (_level == TraceLevel.Verbose);
                }
                return true;
            }
        }

        public static bool TraceInfo
        {
            get
            {
                if (_level != TraceLevel.Info)
                {
                    return (_level == TraceLevel.Verbose);
                }
                return true;
            }
        }

        public static bool TraceVerbose
        {
            get
            {
                return (_level == TraceLevel.Verbose);
            }
        }

        public static bool TraceWarning
        {
            get
            {
                if ((_level != TraceLevel.Warning) && (_level != TraceLevel.Info))
                {
                    return (_level == TraceLevel.Verbose);
                }
                return true;
            }
        }

        public static bool UseSystemTrace
        {
            get
            {
                return _useSystemTrace;
            }
            set
            {
                _useSystemTrace = value;
            }
        }
    }
}

