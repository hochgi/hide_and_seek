using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HideAndSeek
{
    public interface Hider
    {
        List<Vector3> getPartsPositions();//returns the shape of the person

        void Found();

        void win();

        void Done();
    }
}
