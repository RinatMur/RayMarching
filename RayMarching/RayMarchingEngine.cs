using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayMarching
{
    internal class RayMarchingEngine
    {
        public event Action DrawComplete;

        private int screenWidth;
        private int screenHeight;

        private const double distMax = 20;
        private const float distMin = 0.01f;

        private double areaAngleX = 3.14 / 2;
        private double areaAngleY;
        private double stepAngle;
        private List<Vector3> rayDirects;

        private Vector3 posCam = new Vector3(0,0,-5);
        private Vector3 posLight = new Vector3(2,2,-0.5f);

        private Bitmap img;
        public List<Figure> figures;

        public RayMarchingEngine(Bitmap bm)
        {
            screenWidth = bm.Width;
            screenHeight = bm.Height;

            rayDirects = new List<Vector3>();
            CalculateDirrct();
            //TransformDirrectPos();
            //TransformDirrectRot();

            stepAngle = areaAngleX / screenWidth;
            areaAngleY = stepAngle * screenHeight;

            img = bm;

            figures = new List<Figure>();
            figures.Add(new Sphere(new Vector3(-1f, 0, 0f), 1.0f));
            figures.Add(new Cube(new Vector3(1, 0, 0), new Vector3(1f, 1f, 1f)));
            //figures.Add(new Thor(new Vector3(0, 0, 0), new Vector2(1f, 0.5f)));
            //figures.Add(new Cube(new Vector3(0, -11, 0), new Vector3(10f, 10f, 10f)));
        }

        private void CalculateDirrct()
        {

            for (int i = 0; i < screenHeight; i++)
            {
                for (int j = 0 ; j < screenWidth; j++)
                {
                    Vector3 vect = new Vector3(j - screenWidth / 2, screenHeight / 2 - i, screenWidth / 2);

                    rayDirects.Add(Vector3.Normalize(vect));
                }
            }
        }

        private void TransformDirrectPos()
        {
            float a = 0;
            float b = 3;
            float c = 0;

            Matrix4x4 mat = new Matrix4x4(1, 0, 0, a, 0, 1, 0, b, 0, 0, 1, c, 0, 0, 0, 1);

            posCam = Vector3.Transform(posCam, mat);

            for (int i = 0; i < rayDirects.Count; i++)
            {
                rayDirects[i] = Vector3.Transform(rayDirects[i], mat);
            }
        }
        public void TransformDirrectRotX(double f)
        {
            float cosf = Convert.ToSingle(Math.Cos(f));
            float sinf = Convert.ToSingle(Math.Sin(f));

            Matrix4x4 mat = new Matrix4x4(1, 0, 0, 0, 0, cosf, -sinf, 0, 0, sinf, cosf, 0, 0, 0, 0, 1);

            posCam = Vector3.Transform(posCam, mat);

            for (int i = 0; i < rayDirects.Count; i++)
            {
                rayDirects[i] = Vector3.Transform(rayDirects[i], mat);
            }

            Draw();
        }
        public void TransformDirrectRotY(double f)
        {
            float cosf = Convert.ToSingle(Math.Cos(f));
            float sinf = Convert.ToSingle(Math.Sin(f));

            Matrix4x4 mat = new Matrix4x4(cosf, 0, sinf, 0, 0, 1, 0, 0, -sinf, 0, cosf, 0, 0, 0, 0, 1);

            posCam = Vector3.Transform(posCam, mat);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < rayDirects.Count; i++)
            {
                rayDirects[i] = Vector3.Transform(rayDirects[i], mat);
            }

            Draw();
        }
        public void Draw()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            FastBitmap fb = new FastBitmap(img, ImageLockMode.WriteOnly);

            Parallel.For(0, rayDirects.Count, i =>
            {
                #region oldCode
                //for (int j = 0; j < screenHeight; j++)
                //{
                //    Vector3 dir = rayDirects[i, j];

                //    Vector3 point = RayMarch(dir);

                //    if (point == Vector3.Zero)
                //    {
                //        fb[i, j] = new Span<byte>(new byte[] { 0, 0, 0, 255 });
                //    }
                //    else
                //    {
                //        float scalar = Vector3.Dot(Vector3.Normalize(point), Vector3.Normalize(posLight));

                //        byte coef = Convert.ToByte(255 * ((1.0 + scalar) / 2.0));

                //        fb[i, j] = new Span<byte>(new byte[] { coef, coef, coef, 255 });
                //    }

                //}
                #endregion

                int ii = i % screenWidth;
                int jj = i / screenWidth;

                Vector3 dir = rayDirects[i];

                Vector3 point = RayMarch(dir);

                if (point == Vector3.Zero)
                {
                    fb[ii, jj] = new Span<byte>(new byte[] { 0, 0, 0, 255 });
                }
                else
                {
                    Figure fig = GetFigure(point);

                    float scalar = Vector3.Dot(fig.GetNormal(point), Vector3.Normalize(posLight));

                    byte coef = Convert.ToByte(255 * ((1.0 + scalar) / 2.0));

                    fb[ii, jj] = new Span<byte>(fig.GetColor(coef));
                }

            });

            fb.Dispose();

            sw.Stop();

            Graphics gr = Graphics.FromImage(img);
            gr.DrawString(sw.ElapsedMilliseconds.ToString(), new Font("Arial", 8), Brushes.Red, new Point(0, 0));

            DrawComplete?.Invoke();
        }

        double alpha = 3.14 / 4;
        public void RotateLight()
        {
            alpha += 3.14 / 16;

            posLight.X = Convert.ToSingle(Math.Sin(alpha) * 2 * Math.Sqrt(2));
            posLight.Z = Convert.ToSingle(Math.Cos(alpha) * 2 * Math.Sqrt(2));

        }

        private Vector3 RayMarch(Vector3 vectDir)
        {
            Vector3 vect = posCam;

            //float dist = Vector3.Distance(vect, new Vector3(0,0,0)) - 1;
            float dist = GetDistance(vect);

            while (dist > distMin && dist < distMax)
            {
                vect += vectDir * dist;

                dist = GetDistance(vect);
            }

            dist = Vector3.Distance(vect, posCam);

            if (dist > distMax)
            {
                return Vector3.Zero;
            }
            else
            {
                return vect;                
            }
        }

        private float GetDistance(Vector3 p)
        {
            //p.X = (p.X % 2 + 2) % 2;
            //p.Z = (p.Z % 2 +2) % 2;

            float dist = figures[0].GetDistance(p);

            for (int i = 1; i < figures.Count; i++)
            {
                float d = figures[i].GetDistance(p);

                if (d < distMin)
                {
                    return d;
                }
                else if (d < dist)
                {
                    dist = d;
                }
            }

            return dist;
        }

        private Figure GetFigure(Vector3 p)
        {
            int ind = 0;

            for (int i = 0; i < figures.Count; i++)
            {
                float d = figures[i].GetDistance(p);

                if (d < distMin)
                {
                    ind = i;
                    break;
                }
            }

            return figures[ind];
        }

    }
}
