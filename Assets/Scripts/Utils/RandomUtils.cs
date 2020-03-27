namespace Utils
{
    using System;
    using System.Collections.Generic;

    public static class RandomUtils
    {
        private static readonly Random Random =
            new Random(Guid.NewGuid().GetHashCode());

        public static float Range(float min, float max)
        {
            return (float) Random.NextDouble() * (max - min + 1);
        }

        public static T GetRandom<T>(this IList<T> items)
        {
            return items.Count == 0
                ? default
                : items[Random.Next(items.Count)];
        }
    }
}
