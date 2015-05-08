namespace FabrikamFiber.Extranet.Web.Helpers
{
    using System;

    public static class Guard
    {
        public static void ThrowIfNull(object value, string name)
        {
            if (value == null)
                throw new ArgumentNullException(name);
        }

        public static void ThrowIfNullOrEmpty(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(name);
        }

        public static void ThrowIfNotInRange(int value, int min, int max, string name)
        {
            if (value < min || value > max)
                throw new ArgumentOutOfRangeException(name);
        }
    }
}