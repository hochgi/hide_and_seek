using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HideAndSeek
{
    //interface to represent seeker
    public interface Seeker
    {
        //find a hider
        Hider selectHider();

        //return the location of Seeker's eyes
        Vector3 getEyesPosition();
    }
}
