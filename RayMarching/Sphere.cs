using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayMarching
{
    internal class Sphere : Figure
    {
        public float rad;

        public Sphere(Vector3 p, float r) : base( p )
        {
            rad = r;
        }
        public override float GetDistance(Vector3 p)
        {
            return Vector3.Distance(p, pos) - rad;
        }

        public override Vector3 GetNormal(Vector3 p)
        {
            return Vector3.Normalize(p-pos);
        }
        public override byte[] GetColor(byte b)
        {
            return new byte[] {0,0,b,255};
        }
    }
}
