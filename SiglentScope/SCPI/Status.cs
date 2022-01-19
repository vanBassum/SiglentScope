using System;

namespace SCPI
{
    public class Status
    {
        Exception _Exception;
        private Codes Code { get; }
        public Exception Exception => _Exception ?? new Exception($"Some error has occured '{Code.ToString()}'");

        private Status(Codes code)
        {
            Code = code;
        }

        public Status(Status status, string message)
        {
            Code = status.Code;
            _Exception = new Exception(message);
        }

        public Status(Status status, Exception exception)
        {
            Code = status.Code;
            _Exception = exception;
        }

        public override bool Equals(Object obj)
        {
            if (obj is Status status)
                return status.Code == Code;
            return false;
        }

        public override string ToString()
        {
            return Exception?.Message;
        }


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


