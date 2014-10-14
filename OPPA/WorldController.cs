using OPPA.Fuzzy;
using OPPA.PSO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
        private bool fuzzy = false;

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
            checkpoints.Add(start);
            drawer = new Drawer(map, checkpoints); //Loading the map and drawer
            car = new Car(55, 30); //L = C*0.55
            car.X = start.X;
            car.Y = start.Y;
            drawer.AddDrawable(car); //Adding car to the drawable list
            fis = new FIS();
            pso = new PSOHandler(1000, 200, checkpoints);
        }

        public void Update()
        {
            if (fuzzy)
            {
                car.Speed = fis.getSpeed(car.Speed, car.Acceleration, car.Brake);
                car.WheelAngle = fis.getWheelAngle(car.SteeringWheel);
                car.Acceleration = 0;
                car.Brake = 0;

                // TODO: Remove this test
                //Stopwatch st = new Stopwatch();
                //st.Start();
                //pso.UpdateSwarm();
                //st.Stop();
                //Console.WriteLine(st.ElapsedMilliseconds);
                //End of test
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
