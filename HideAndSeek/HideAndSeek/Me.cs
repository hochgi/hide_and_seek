using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideAndSeek
{
    abstract class Me
    {
        internal Me() { }

        internal abstract bool isWalking();

        internal abstract Microsoft.Xna.Framework.Vector3 getHeadPosition();
    }
}
