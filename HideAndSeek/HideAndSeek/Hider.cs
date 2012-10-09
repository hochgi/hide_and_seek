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
        //i know it shouldn't be int but for now i don't know what it should return
        //maybe not necessary?

        void Found();

        void win();

        Vector3 location
        {
            get;
        }
    }
}
