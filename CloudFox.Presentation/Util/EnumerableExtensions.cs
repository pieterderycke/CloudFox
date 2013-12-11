using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;

namespace CloudFox.Presentation.Util
{
    /// <summary>
    /// Contains static extension methods for IEnumerable collections.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Extension method to recursively travers a nested data structure.
        /// </summary>
        /// <typeparam name="T">The type of objects in the IEnumerable collections.</typeparam>
        /// <param name="source">The root IEnumerable collection.</param>
        /// <param name="descendBy">The function to travers to the cild IEnumerable collections.</param>
        /// <returns></returns>
        public static IEnumerable<T> Descendants<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> descendBy)
        {
            foreach (T value in source)
            {
                yield return value;

                foreach (T child in descendBy(value).Descendants<T>(descendBy))
                {
                    yield return child;
                }
            }
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IList<T>> source)
        {
            foreach (IList<T> enumerable in source)
            {
                foreach (T value in enumerable)
                {
                    yield return value;
                }
            }
        }

        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
            where T : class
        {
            return enumerable.FirstOrDefault() == null;
        }
    }
}
