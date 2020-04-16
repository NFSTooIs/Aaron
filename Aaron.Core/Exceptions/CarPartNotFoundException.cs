using System;
using System.Runtime.Serialization;

namespace Aaron.Core.Exceptions
{
    [Serializable]
    public class CarPartNotFoundException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public CarPartNotFoundException()
        {
        }

        public CarPartNotFoundException(string message) : base(message)
        {
        }

        public CarPartNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CarPartNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}