using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HideAndSeek
{
    //interface to represent hider
    public interface Hider
    {
        //returns the shape of the person
        List<Vector3> getPartsPositions();

        //what to do when found by Seeker
        void Found();

        //what to do when win against Seeker
        void win();

        Vector3 Location { get; }
    }
}
