using Silbernetz.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Silbernetz.Actions
{
    public class WaitTimeComparer : IComparer<WaitTimeProp>
    {
        public int Compare([AllowNull] WaitTimeProp x, [AllowNull] WaitTimeProp y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (x == null) return 1;
            return x.TimeStamp.CompareTo(y.TimeStamp);
        }
    }
}
