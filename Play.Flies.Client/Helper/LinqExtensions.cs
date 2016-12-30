using System;
using System.Collections.Generic;
using System.Linq;

namespace Play.Flies.Client.Helper
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list)
        {
            var random = new Random(DateTime.Now.Ticks.GetHashCode());
            return list.OrderBy(_ => random.Next());
        }
    }
}