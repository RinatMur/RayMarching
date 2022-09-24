using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayMarching
{
    internal abstract class Figure
    {
        public Vector3 pos;
        public Figure (Vector3 p)
        {
            pos = p;

        }

        public abstract float GetDistance(Vector3 p);
        public abstract Vector3 GetNormal(Vector3 p);
        public abstract byte[] GetColor(byte b);

    }
}
