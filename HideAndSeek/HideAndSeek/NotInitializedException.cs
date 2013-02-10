using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideAndSeek
{
    class NotInitializedException : Exception
    {
        public NotInitializedException(string p) : base(p) { }
    }
}
