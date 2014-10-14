using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPPA
{
    public class Wheel : IDrawable
    {
        private float angle, x, y, width, length;

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        public Wheel(float x, float y, float length, float width, float angle)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.length = length;
            this.angle = angle;
        }

        public void Draw(Graphics g)
        {
            float dL = length / 2.0f;
            float dW = width / 2.0f;
            g.TranslateTransform(x + dL, y + dW);
            g.RotateTransform(angle);
            g.TranslateTransform(-dL, -dW);
            g.DrawRectangle(new Pen(Color.Black), 0, 0, length, width);
            g.TranslateTransform(dL, dW);
            g.RotateTransform(-angle);
            g.TranslateTransform(-x - dL, -y - dW);
        }
    }
}
