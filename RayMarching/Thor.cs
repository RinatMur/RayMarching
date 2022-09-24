using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayMarching
{
    internal class Thor : Figure
    {
        public Vector2 size;
        public Thor(Vector3 p, Vector2 s) : base(p)
        {
            size = s;
        }
        public override float GetDistance(Vector3 p)
        {
            //float sdTorus(vec3 p, vec2 t)
            //{
            //    vec2 q = vec2(length(p.xz) - t.x, p.y);
            //    return length(q) - t.y;
            //}
            p = p - pos;
            Vector2 q = new Vector2(new Vector2(p.X, p.Z).Length() - size.X, p.Y);

            return q.Length() - size.Y;
        }

        public override Vector3 GetNormal(Vector3 p)
        {
            return Vector3.Normalize(p - pos);
        }

        public override byte[] GetColor(byte b)
        {
            return new byte[] { 0, b, 0, 255 };
        }
    }
}
