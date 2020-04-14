using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Aaron.Core.Utils
{
    /// <summary>
    /// Utility functions and extensions for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableUtils
    {
        /// <summary>
        /// Returns distinct items in a <see cref="IEnumerable{T}"/> based on keys generated through a given key function.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <param name="items">The <see cref="IEnumerable{T}"/> to retrieve distinct items from.</param>
        /// <param name="property">A <see cref="Func{T, TKey}"/> to generate a <typeparamref name="TKey"/> from a <typeparamref name="T"/> instance.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> with the distinct items from <paramref name="items"/>.</returns>
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
        {
            return items.GroupBy(property).Select(x => x.First());
        }


        /// <summary>
        /// Clumps items into same size lots.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source list of items.</param>
        /// <param name="size">The maximum size of the clumps to make.</param>
        /// <returns>A list of list of items, where each list of items is no bigger than the size given.</returns>
        public static IEnumerable<IEnumerable<T>> Clump<T>(this IEnumerable<T> source, int size)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (size < 1)
                throw new ArgumentOutOfRangeException(nameof(size), "size must be greater than 0");

            return ClumpIterator(source, size);
        }

        private static IEnumerable<IEnumerable<T>> ClumpIterator<T>(IEnumerable<T> source, int size)
        {
            Debug.Assert(source != null, "source is null.");

            var items = new T[size];
            var count = 0;
            foreach (var item in source)
            {
                items[count] = item;
                count++;

                if (count != size) continue;
                yield return items;
                items = new T[size];
                count = 0;
            }

            if (count <= 0) yield break;
            if (count == size)
                yield return items;
            else
            {
                var tempItems = new T[count];
                Array.Copy(items, tempItems, count);
                yield return tempItems;
            }
        }
    }
}