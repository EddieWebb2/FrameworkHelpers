using System;

namespace FrameworkHelpers.Common.Models
{
    public class SystemTime
    {
        private static Func<DateTime> _action;

        static SystemTime()
        {
            _action = () =>
            {
                return DateTime.Now;
            };
        }

        public static DateTime Now
        {
            get { return _action(); }
        }

        public static void OverrideWith(Func<DateTime> action)
        {
            _action = action;
        }

        public static void Reset()
        {
            _action = () =>
            {
                return DateTime.Now;
            };
        }
    }
}
