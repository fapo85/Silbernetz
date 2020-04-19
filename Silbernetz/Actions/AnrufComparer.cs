using Silbernetz.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Silbernetz.Actions
{
    public class AnrufComparer : IComparer<Anruf>
    {
        public int Compare([AllowNull] Anruf x, [AllowNull] Anruf y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (x == null) return 1;
            return x.id.CompareTo(y.id);
        }
    }
}
