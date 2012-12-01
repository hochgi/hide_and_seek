using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HideAndSeek
{
    interface Billboard
    {
        Vector3[] getQuadBillboard(Vector3 up, Vector3 side);

        Vector3 getPosition();

        static Billboard[] sortBillboards(Billboard[] billboards, Vector3 cameraLocation)
        {
            Array.Sort<Billboard>(billboards, new Comparison<Billboard>(
                (Billboard a, Billboard b) => (int)(Vector3.DistanceSquared(cameraLocation,a.getPosition()) - Vector3.DistanceSquared(cameraLocation,b.getPosition())))
            );
            throw new NotImplementedException;
        }

        //static void drawBillboards
    }
}
