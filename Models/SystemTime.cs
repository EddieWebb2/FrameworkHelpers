using System;

namespace FrameworkHelpers.Models
{
    public class SystemTime
    {
        private static Func<DateTime> _action;

        static SystemTime() => _action = () => DateTime.Now;

        public static DateTime Now => _action();

        public static void OverrideWith(Func<DateTime> action) => _action = action;

        public static void Reset() => _action = () => DateTime.Now;
    }
}
