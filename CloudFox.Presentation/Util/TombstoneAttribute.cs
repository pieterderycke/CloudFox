using System;

namespace CloudFox.Presentation.Util
{
    /// <summary>
    /// Attribute to indicate the ViewModel Property should be tombstoned.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class TombstoneAttribute : Attribute
    {

    }
}
