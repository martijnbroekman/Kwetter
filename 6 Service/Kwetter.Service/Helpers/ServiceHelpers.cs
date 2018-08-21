using System;
using System.Collections.Generic;
using System.Text;

namespace Kwetter.Service.Helpers
{
    public static class ServiceHelpers
    {
        public static void CheckValidId(int id)
        {
            if (id <= 0)
                throw new ArgumentException($"The value of {nameof(id)} cannot be less than 1");
        }

        public static void CheckRange(int from, int to)
        {
            if (from > to)
                throw new ArgumentException($"The value of {nameof(from)} must be less than the value of {nameof(to)}");
            if (to - from > 40)
                throw new ArgumentException($"The value of {nameof(to)} - {nameof(from)} cannot be larger than 40");
        }
    }
}
