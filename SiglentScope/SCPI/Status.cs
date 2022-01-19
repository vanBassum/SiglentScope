using System;

namespace SCPI
{
    public class Status
    {
        private Codes Code { get; }
        public Exception Exception { get; set; }
        public TimeSpan Duration { get; set; }

        private Status(Codes code)
        {
            Code = code;
            if (code != Codes.Success)
                Exception = new Exception($"Error '{code}'");
        }

        public override bool Equals(Object obj)
        {
            if (obj is Status status)
                return status.Code == Code;
            return false;
        }

        public static bool operator ==(Status obj1, Status obj2) => obj1.Equals(obj2);
        public static bool operator !=(Status obj1, Status obj2) => !obj1.Equals(obj2);

        public static Status Unknown => new Status(Codes.Unknown);
        public static Status Success => new Status(Codes.Success);
        public static Status UnknownError => new Status(Codes.UnknownError);
        public static Status NotConnected => new Status(Codes.NotConnected);
        public static Status ParsingError => new Status(Codes.ParsingError);
        public static Status SendingError => new Status(Codes.SendingError);

        enum Codes
        {
            Unknown,
            Success,
            UnknownError,
            SendingError,
            NotConnected,
            ParsingError,
        }
    }
}


