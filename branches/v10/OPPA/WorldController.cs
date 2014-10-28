using OPPA.Fuzzy;
using OPPA.PSO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace OPPA
{
    public class WorldController
    {
        private Drawer drawer;
        private Car car;
        private FIS fis;
        private PSOHandler pso;
        private bool fuzzy = false, ready = false;
        Particle p;
        int i = 0;
        StringBuilder s;

        public Bitmap World
        {
            get { return drawer.World; }
        }

        public bool Fuzzy
        {
            get { return fuzzy; }
            set { fuzzy = value; }
        }

        public WorldController(string map, Point start, List<PointF> checkpoints = null) 
            : this(new Bitmap(map), start, checkpoints)
        { }

        public WorldController(Bitmap map, PointF start, List<PointF> checkpoints = null)
        {
            //checkpoints.Add(start);
            drawer = new Drawer(map, checkpoints); //Loading the map and drawer
            car = new Car(55); //L = C*0.55
            car.X = start.X;
            car.Y = start.Y;
            drawer.AddDrawable(car); //Adding car to the drawable list
            fis = new FIS();
            pso = new PSOHandler(50, 50, start, checkpoints, World); // TODO: Remove hardcode
            s = new StringBuilder();
        }

        public void Update()
        {
            if (fuzzy)
            {
                //s.AppendFormat("{0}-{1}-{2}-0-0-0-0-0;\r\n", car.Acceleration, car.Brake, car.SteeringWheel);
                //car.Speed = fis.getSpeed(car.Speed, car.Acceleration, car.Brake);
                //car.WheelAngle = fis.getWheelAngle(car.SteeringWheel);
                //car.Acceleration = 0;
                //car.Brake = 0;

                // TODO: Remove this test
                Stopwatch st = new Stopwatch();
                i = 0;
                st.Start();
                p = pso.Run(200);
                st.Stop();
                Console.WriteLine(st.ElapsedMilliseconds);
                SystemSounds.Beep.Play();
                //End of test
                fuzzy = false;
                ready = true;
            }
            else if(ready)
            {
                //Console.Write(s.ToString());
                if(i <= 50)
                {
                    car.Speed = p.BestPosition[i, 3];
                    car.WheelAngle = p.BestPosition[i, 4];
                    car.X = p.BestPosition[i, 5];
                    car.Y = p.BestPosition[i, 6];
                    car.Angle = p.BestPosition[i, 7];
                    i++;
                }
                else
                {
                    car.Speed = 0;
                    ready = false;
                }
            }
            drawer.Draw();
        }

        public void AccelerateCar()
        {
            car.Brake = 0;
            car.Acceleration += 0.2f;
        }

        public void DecelerateCar()
        {
            car.Acceleration = 0;
            car.Brake += 0.2f;
        }

        public void TurnLeft()
        {
            car.SteeringWheel -= 0.25f;
        }

        public void TurnRight()
        {
            car.SteeringWheel += 0.25f;
        }
    }
}
