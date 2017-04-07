using System;

namespace Toto.Simple.EPPLUS.Core
{
    public class FormularBuilderException : Exception
    {
        public FormularBuilderException(string message) : base(message)
        {
            //EMPTY!
        }

        public FormularBuilderException(string message, Exception innerException) : base(message, innerException)
        {
            //EMPTY!
        }
    }
}
