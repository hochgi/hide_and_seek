using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideAndSeek
{
    class KeyboardMe : Me
    {
        internal KeyboardMe() : base() { }

        internal override bool isWalking()
        {
            throw new NotImplementedException();
        }

        internal override Microsoft.Xna.Framework.Vector3 getHeadPosition()
        {
            throw new NotImplementedException();
        }
    }
}
