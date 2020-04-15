using System;
using System.Runtime.Serialization;

namespace Aaron.Core.Bundle
{
    [Serializable]
    public class ChunkCorruptedException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ChunkCorruptedException()
        {
        }

        public ChunkCorruptedException(string message) : base(message)
        {
        }

        public ChunkCorruptedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ChunkCorruptedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}