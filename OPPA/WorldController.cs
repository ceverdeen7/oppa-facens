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

        public WorldController()
        {
            Bitmap map = new Bitmap(1000, 700);
            Graphics g = Graphics.FromImage(map);
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, map.Width, map.Height);
            g.Dispose();
            drawer = new Drawer("C:\\Users\\André\\Desktop\\map.png"); //Loading the map and drawer
            car = new Car(55, 30); //L = C*0.55
            car.X = 100;
            car.Y = 100;
            drawer.AddDrawable(car); //Adding car to the drawable list
            fis = new FIS();
            pso = new PSOHandler(500, 1000);
        }

        public void Update()
        {
            if (fuzzy)
            {
                car.Speed = fis.getSpeed(car.Speed, car.Acceleration, car.Brake);
                car.WheelAngle = fis.getWheelAngle(car.SteeringWheel);
                car.Acceleration = 0;
                car.Brake = 0;
                Stopwatch st = new Stopwatch();
                st.Start();
                pso.UpdateSwarm();
                st.Stop();
                Console.WriteLine(st.ElapsedMilliseconds);
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
