using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideAndSeek
{
    class AlreadyInitializedException : Exception
    {
        public AlreadyInitializedException(string p) : base(p) {}
    }
}
