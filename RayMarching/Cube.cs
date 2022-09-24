using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayMarching
{
    internal class Cube : Figure
    {
        public Vector3 size;
        private List<Vector3> normals;

        public Cube(Vector3 p, Vector3 s ) : base(p)
        {
            size = s;
            NormalsInit();
        }

        private void NormalsInit()
        {
            normals = new List<Vector3>();

            normals.Add(new Vector3(0, 1, 0));
            normals.Add(new Vector3(1, 0, 0));
            normals.Add(new Vector3(0, -1, 0));
            normals.Add(new Vector3(-1, 0, 0));
            normals.Add(new Vector3(0, 0, -1));
            normals.Add(new Vector3(0, 0, 1));
        }

        public override float GetDistance(Vector3 p)
        {
            Vector3 q = Vector3.Abs(p-pos) - size;

            //length(max(q,0.0)) + min(max(q.x,max(q.y,q.z)),0.0);

            float res = Vector3.Max(q, Vector3.Zero).Length() +
                Vector3.Min(Vector3.Max(q.X * Vector3.UnitX, Vector3.Max(q.Y * Vector3.UnitY, q.Z * Vector3.UnitZ)), Vector3.Zero).Length();

            return res;
        }

        public override Vector3 GetNormal(Vector3 p)
        {
            p = p - pos;
            p = Vector3.Normalize(p);
            float scalar = -1f;
            int ind = 0;

            for (int i = 0; i < normals.Count; i++)
            {
                float s = Vector3.Dot(normals[i], p);

                if(s > scalar)
                {
                    ind = i;
                    scalar = s;
                }
            }

            return normals[ind];
        }

        public override byte[] GetColor(byte b)
        {
            return new byte[] { b, 0, 0, 255 };
        }
    }
}
