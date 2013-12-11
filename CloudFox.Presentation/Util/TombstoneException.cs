using System;

namespace CloudFox.Presentation.Util
{
    /// <summary>
    /// Exception to indicate that an error occured during tombstoning.
    /// </summary>
    public class TombstoneException: Exception
    {
        public TombstoneException(string message)
            : base(message)
        {
        }

        public TombstoneException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
