using System;
using System.Runtime.Serialization;

namespace MyCefet.Api.Interfaces
{
    [Serializable]
    internal class LoginFailException : Exception
    {
        public LoginFailException()
        {
        }

        public LoginFailException(string message) : base(message)
        {
        }

        public LoginFailException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LoginFailException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}