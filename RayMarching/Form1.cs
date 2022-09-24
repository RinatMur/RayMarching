namespace RayMarching
{
    public partial class Form1 : Form
    {

        RayMarchingEngine engine;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(800,600);
            picBox.Image = bmp;

            engine = new RayMarchingEngine(bmp);

            engine.DrawComplete += () => picBox.Refresh();

            engine.Draw();

        }

        bool isLeft = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //engine.RotateLight();
            //engine.Draw();
            //if(isLeft)
            //{
            //    engine.figures[1].pos.X -= 0.1f;

            //    if((engine.figures[1].pos.X <= -1))
            //    {
            //        isLeft = false;
            //    }
            //}
            //else 
            //{
            //    engine.figures[1].pos.X += 0.1f;
            //    if ((engine.figures[1].pos.X >= 1))
            //    {
            //        isLeft = true;
            //    }
            //}
            //engine.Draw();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    engine.TransformDirrectRotX(-3.14 / 6.0);
                    break;
                case Keys.S:
                    engine.TransformDirrectRotX(3.14 / 6.0);
                    break;
                case Keys.A:
                    engine.TransformDirrectRotY(-3.14 / 6.0);
                    break;
                case Keys.D:
                    engine.TransformDirrectRotY(3.14 / 6.0);
                    break;
            }
        }
    }
}