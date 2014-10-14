using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPPA
{
    public class Drawer
    {
        private Bitmap bmpWorld, bmpInitial;
        private Graphics gWorld;
        List<IDrawable> drawables;
        List<PointF> checkpoints;

        public Bitmap World
        {
            get { return bmpWorld; }
        }

        public Drawer(Bitmap world, List<PointF> checkpoints = null)
        {
            bmpInitial = CopyImage(world);
            bmpWorld = CopyImage(bmpInitial);
            drawables = new List<IDrawable>();
            this.checkpoints = checkpoints?? new List<PointF>();

            

            // TODO: Remove this test
            #if DEBUG
                gWorld = Graphics.FromImage(bmpInitial);
                SolidBrush br = new SolidBrush(Color.Blue);
                foreach(PointF p in this.checkpoints)
                {
                    gWorld.FillEllipse(br, p.X, p.Y, 5, 5);
                }
                gWorld.Dispose();
            #endif
            //End of test
        }

        public Drawer(int width, int height) : this(new Bitmap(width,height))
        { }

        public void Draw()
        {
            bmpWorld.Dispose();
            bmpWorld = CopyImage(bmpInitial);
            gWorld = Graphics.FromImage(bmpWorld);
            foreach(IDrawable obj in drawables)
            {
                obj.Draw(gWorld);
            }
            gWorld.Dispose();
        }

        public Bitmap CopyImage(Bitmap original)
        {
            return new Bitmap(original);
        }

        public void AddDrawable(IDrawable drawable)
        {
            drawables.Add(drawable);
        }
    }
}
