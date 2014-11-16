using OPPA.Fuzzy;
using OPPA.Genetics;
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
        private GeneticHandler gh;
        private bool fuzzy = false, ready = false;
        Particle p;
        Chromosome c;
        int i = 0;

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
            pso = new PSOHandler(100, 20, start, checkpoints, World);
            //gh = new GeneticHandler(1000, 2000, 0.2, 50, start, checkpoints, World); // TODO: Remove hardcode
        }

        public void Update()
        {
            if (fuzzy)
            {
                // TODO: Remove this test
                Stopwatch st = new Stopwatch();
                i = 0;
                st.Start();
                p = pso.Run(2000);
                //c = gh.FindSolution();
                st.Stop();
                Console.WriteLine(st.ElapsedMilliseconds);
                //End of test
                fuzzy = false;
                ready = true;
            }
            else if (ready)
            {
                if (i <= 20)
                {
                    car.Speed = p.BestPosition[i, 3];
                    car.WheelAngle = p.BestPosition[i, 4];
                    car.X = p.BestPosition[i, 5];
                    car.Y = p.BestPosition[i, 6];
                    car.Angle = p.BestPosition[i, 7];
                    i++;

                    //car.Speed = c.Moves[i, 3];
                    //car.WheelAngle = c.Moves[i, 4];
                    //car.X = c.Moves[i, 5];
                    //car.Y = c.Moves[i, 6];
                    //car.Angle = c.Moves[i, 7];
                    //i++;
                }
                else
                {
                    car.Speed = 0;
                    ready = false;
                }
            }
            else
            {
                car.Speed = fis.getSpeed(car.Speed, car.Acceleration, car.Brake);
                car.WheelAngle = fis.getWheelAngle(car.SteeringWheel);
                car.Acceleration = 0;
                car.Brake = 0;
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
