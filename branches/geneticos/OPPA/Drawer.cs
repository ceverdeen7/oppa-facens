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

        public Bitmap World
        {
            get { return bmpWorld; }
        }

        public Drawer(Bitmap world)
        {
            bmpInitial = CopyImage(world);
            bmpWorld = CopyImage(bmpInitial);
            drawables = new List<IDrawable>();
        }

        public Drawer(int width, int height) : this(new Bitmap(width,height))
        {
        }

        public Drawer(string imgFile) : this(new Bitmap(imgFile))
        {
        }

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

            //TESTES
            #if DEBUG
                gWorld = Graphics.FromImage(bmpInitial);
                Pen p = new Pen(Color.Red);
                Car c = drawables[0] as Car;
                gWorld.DrawCurve(p, c.ptCurva);
                gWorld.Dispose();
            #endif
            //Fim TESTES
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
