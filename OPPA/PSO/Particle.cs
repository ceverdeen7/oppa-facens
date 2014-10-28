using OPPA.Fuzzy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPPA.PSO
{
    public class Particle
    {
        /// <summary>
        /// List of current particle's moves
        /// current[i,0] = Acceleration
        /// current[i,1] = Brake
        /// current[i,2] = SteeringWheel
        /// current[i,3] = Speed
        /// current[i,4] = WheelAngle 
        /// current[i,5] = X
        /// current[i,6] = Y
        /// current[i,7] = Angle
        /// </summary>
        float[,] current, best;
        double curEval, bestEval;
        private double inertiaWeight;
        private double cognitiveWeight;
        private double socialWeight;
        private float[,] velocity;
        private float[] maximum = { 1, 1, 2.5f }, minimum = { 0, 0, -2.5f };
        int steps;
        List<PointF> checkpoints;
        FIS fis;
        Car car;
        bool[,] map;

        public double BestEvaluation
        {
            get { return bestEval; }
        }

        public float[,] BestPosition
        {
            get { return best; }
        }
        
        public Particle(int steps, PointF start, List<PointF> checkpoints, bool[,] map)
        {
            this.steps = steps;
            current = new float[steps + 1, 8];
            best = new float[steps + 1, 8];
            velocity = new float[steps + 1, 3];
            this.checkpoints = checkpoints;
            car = new Car(55);
            fis = new FIS();
            socialWeight = 0.05;
            cognitiveWeight = 0.1;
            inertiaWeight = 0.1;
            this.map = map;
            Randomize(start);
        }

        /// <summary>
        /// Generate random particle's moves
        /// </summary>
        private void Randomize(PointF start)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            current[0, 5] = start.X;
            current[0, 6] = start.Y;

            Parallel.For(0, steps + 1, i =>
            {
                current[i, 0] = (float)r.NextDouble() * (maximum[0] - minimum[0]) + minimum[0];
                current[i, 1] = (float)r.NextDouble() * (maximum[1] - minimum[1]) + minimum[1];
                current[i, 2] = (float)r.NextDouble() * (maximum[2] - minimum[2]) + minimum[2];
            });
            bestEval = curEval = Evaluate();
            best = (float[,])current.Clone();
        }

        public void Move(Particle bestN)
        {
            Random rand = new Random(DateTime.Now.Millisecond);

            //Particle bestN = FindBestNeighbor();

            // Calcula a velocidade..
            Parallel.For(0, steps + 1, k =>
                {
                    for (int i = 0; i < 3; i++)
                    {
                        double minV = -1.0f * Math.Abs(maximum[i] - minimum[i]);
                        double maxV = Math.Abs(maximum[i] - minimum[i]);
                        velocity[k,i] = (float)(inertiaWeight * velocity[k,i] + (best[k, i] - current[k, i]) * cognitiveWeight * rand.NextDouble() + (bestN.BestPosition[k, i] - current[k, i]) * socialWeight * rand.NextDouble());
                        velocity[k,i] = (float)Math.Min(Math.Max(minV, velocity[k,i]), maxV);
                        // Movimenta a partícula..
                        current[k,i] = velocity[k,i] + current[k,i];
                        current[k,i] = Math.Min(Math.Max(minimum[i], current[k,i]), maximum[i]);
                    }
                });
            
            curEval = Evaluate();
            if(curEval < bestEval)
            {
                bestEval = curEval;
                best = (float[,])current.Clone();
            }
        }

        private double Evaluate()
        {
            double s = 0;
            int r = 0, ip, p = 0;
            int[] checkeds = new int[checkpoints.Count];
            for (int i = 0; i < steps; i++)
            {
                car.Speed = fis.getSpeed(current[i, 3], current[i, 0], current[i, 1]); //getting speed
                car.WheelAngle = fis.getWheelAngle(current[i, 2]); //getting wheelangle
                car.X = current[i, 5];
                car.Y = current[i, 6];
                car.Angle = current[i, 7];
                car.Move(); //moving car
                current[i + 1, 3] = car.Speed; //update speed
                current[i + 1, 4] = car.WheelAngle; //update wheelangle
                current[i + 1, 5] = car.X; //update x position
                current[i + 1, 6] = car.Y; //update y position
                current[i + 1, 7] = car.Angle; //update angle
                ip = checkpoints.IndexOf(new PointF(car.X, car.Y));
                if (ip >= 0) checkeds[ip]++;
                else if (car.X < 0 || car.Y < 0
                    || car.X > map.GetLength(0) - 1
                    || car.Y > map.GetLength(1) - 1
                    || map[(int)car.X, (int)car.Y])
                {
                    s += 10;
                }

                //Contar quantos checkpoints passou
                //f = t - p + r
                //t total
                //p passou
                //r repetiu
            }
            for (int i = 0; i < checkpoints.Count; i++)
            {
                if(checkeds[i] > 0)
                {
                    p++;
                    r += checkeds[i] - 1;
                }
            }
                return checkpoints.Count - p + r + s;
        }

        public void Best()
        {
            float[,] mov = { { 0.2f, 0, 0, 0, 0, 170f, 135f, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 } };
            current = mov.Clone() as float[,];
            bestEval = Evaluate();
            best = current.Clone() as float[,];
        }
    }
}
