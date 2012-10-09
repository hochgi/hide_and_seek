using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HideAndSeek
{
    public interface Seeker
    {
        Hider selectHider();

        Vector3 getEyesPosition();//maybe should return a different value that also considers the direction player is facing?
        //as of now we don't know what direction virtual players face..
    }
}
