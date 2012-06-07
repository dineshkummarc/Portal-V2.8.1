using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Portal.API
{
    [Serializable]
    public class PortalException : System.Exception
    {
        public PortalException()
        {
        }

        public PortalException(string message)
            : base(message)
        {
        }

        public PortalException(string message, Exception innerExeption)
            : base(message, innerExeption)
        {
        }

        protected PortalException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
