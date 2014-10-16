using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPPA
{
    public class Car : IDrawable
    {
        private float width, length, distL, distFB, widthW, lengthW, steeringWheel;
        private float angle, speed, acceleration, brake, x, y, axisDist;
        private Wheel LFWheel, RFWheel, LBWheel, RBWheel;
        private Wheel[] wheels;

        #region Properties
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
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }
        public float Acceleration
        {
            get { return acceleration; }
            set
            {
                if (value > 1) value = 1;
                else if (value < 0) value = 0;
                acceleration = value;
            }
        }
        public float Brake
        {
            get { return brake; }
            set
            {
                if (value > 1) value = 1;
                else if (value < 0) value = 0;
                brake = value;
            }
        }
        public float WheelAngle
        {
            get { return LFWheel.Angle; }
            set 
            { 
                LFWheel.Angle = value;
                RFWheel.Angle = value;
            }
        }
        public float SteeringWheel
        {
            get { return steeringWheel; }
            set 
            { 
                if (value < -2.5f) value = -2.5f;
                else if (value > 2.5f) value = 2.5f;
                steeringWheel = value;
            }
        }

        #endregion

        public Car(float x, float y, float length)
        {
            this.x = x;
            this.y = y;
            this.length = length;
            width = (float)Math.Ceiling(length * 0.55);
            distFB = (float)Math.Ceiling(0.1 * length); //Frontal distance
            lengthW = (float)Math.Ceiling(0.2 * length); //Wheel length
            widthW = (float)Math.Ceiling(0.17 * width); //Wheel width
            distL = (float)Math.Ceiling(0.02 * length); //lateral distance
            axisDist = (length - (2 * distFB) - lengthW);

            LFWheel = new Wheel(length - lengthW - distFB, distL, lengthW, widthW, 0);
            RFWheel = new Wheel(length - lengthW - distFB, width - widthW - distL, lengthW, widthW, 0);
            LBWheel = new Wheel(distFB, distL, lengthW, widthW, 0);
            RBWheel = new Wheel(distFB, width - widthW - distL, lengthW, widthW, 0);
            wheels = new Wheel[4];
            wheels[0] = LFWheel;
            wheels[1] = RFWheel;
            wheels[2] = LBWheel;
            wheels[3] = RBWheel;
        }

        public Car(int length) : this(0,0,length)
        {
        }

        public void Draw(Graphics g)
        {
            g.TranslateTransform(x, y);
            g.RotateTransform(angle);
            Move();

            #if DEBUG
            Pen p = new Pen(Color.Red);
            g.DrawLine(p, 0, 0, axisDist, 0);
            g.DrawLine(p, 0, -width / 2 + distL + widthW / 2, 0, width / 2 - distL - widthW / 2);
            g.DrawLine(p, axisDist, -width / 2 + distL + widthW / 2, axisDist, width / 2 - distL - widthW / 2);
            #endif
            g.TranslateTransform(-(lengthW / 2 + distFB), -width / 2);
            foreach (Wheel w in wheels)
            {
                w.Draw(g);
            }
            g.DrawRectangle(new Pen(Color.Blue), 0, 0, length, width);
            g.ResetTransform();
        }

        public void Move()
        {
            float incX, incY;
            double rad = angle * Math.PI / 180f;
            if (speed > 0)
            {
                if (Math.Abs(WheelAngle) < 0.0001f)
                {
                    incX = (float)Math.Cos(rad);
                    incY = (float)Math.Sin(rad);
                    x += incX * speed;
                    y += incY * speed;
                }
                else
                {
                    float radius = (float)(axisDist * Math.Tan((90 - WheelAngle) * Math.PI / 180f));
                    float totalAngle = (float)((360 * speed) / (2 * Math.PI * radius));

                    double yn = y + radius * Math.Sin((90 + angle) * Math.PI / 180);
                    double xn = x + radius * Math.Cos((90 + angle) * Math.PI / 180);
                    double r = Math.Sqrt(Math.Pow(yn - y, 2) + Math.Pow(xn - x, 2));
                    double t = Math.Atan2(y - yn, x - xn);
                    double f = t + totalAngle * Math.PI / 180;

                    x = (float)(xn + r * Math.Cos(f));
                    y = (float)(yn + r * Math.Sin(f));

                    angle = (angle + totalAngle) % 360f;
                }
            }
        }

    }
}
