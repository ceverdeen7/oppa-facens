using OPPA.Fuzzy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPPA.Genetics
{
    public class Chromosome
    {
        private float[,] moves;
        private int steps;
        private double fitness;
        private static float[] maximum = { 1, 1, 2.5f }, minimum = { 0, 0, -2.5f };
        private List<PointF> checkpoints;
        private FIS fis;
        private Car car;
        private bool[,] map;
        private static Random random = new Random(DateTime.Now.Millisecond);

        public float[,] Moves
        {
            get { return moves; }
        }

        public double Fitness
        {
            get { return fitness; }
        }

        private Chromosome(int steps, List<PointF> checkpoints, bool[,] map, float[,] moves)
        {
            this.steps = steps;
            this.checkpoints = checkpoints;
            this.moves = moves;
            car = new Car(55);
            fis = new FIS();
            this.map = map;
            CalcFitness();
        }

        public static Chromosome RandomChromosome(int steps, PointF start, List<PointF> checkpoints, bool[,] map)
        {
            float[,] moves = new float[steps + 1, 8];
            moves[0, 5] = start.X;
            moves[0, 6] = start.Y;

            Parallel.For(0, steps + 1, i =>
            {
                moves[i, 0] = random.Next(11) * 0.1f + minimum[1];
                moves[i, 1] = random.Next(11) * 0.1f + minimum[1];
                moves[i, 2] = random.Next(26) * 0.2f + minimum[2];
                //moves[i, 0] = (float)random.NextDouble() * (maximum[0] - minimum[0]) + minimum[0];
                //moves[i, 1] = (float)random.NextDouble() * (maximum[1] - minimum[1]) + minimum[1];
                //moves[i, 2] = (float)random.NextDouble() * (maximum[2] - minimum[2]) + minimum[2];
            });

            return new Chromosome(steps, checkpoints, map, moves);
        }

        private void CalcFitness()
        {
            double s = 0;
            int r = 0, ip, p = 0;
            int[] checkeds = new int[checkpoints.Count];
            for (int i = 0; i < steps; i++)
            {
                car.Speed = fis.getSpeed(moves[i, 3], moves[i, 0], moves[i, 1]); //getting speed
                car.WheelAngle = fis.getWheelAngle(moves[i, 2]); //getting wheelangle
                car.X = moves[i, 5];
                car.Y = moves[i, 6];
                car.Angle = moves[i, 7];
                car.Move(); //moving car
                moves[i + 1, 3] = car.Speed; //update speed
                moves[i + 1, 4] = car.WheelAngle; //update wheelangle
                moves[i + 1, 5] = car.X; //update x position
                moves[i + 1, 6] = car.Y; //update y position
                moves[i + 1, 7] = car.Angle; //update angle
                ip = checkpoints.IndexOf(new PointF(car.X, car.Y));
                if (ip >= 0) checkeds[ip]++;
                else if (car.X < 0 || car.Y < 0
                    || car.X > map.GetLength(0) - 1
                    || car.Y > map.GetLength(1) - 1
                    || map[(int)car.X, (int)car.Y])
                {
                    s += 10;
                }
            }
            for (int i = 0; i < checkpoints.Count; i++)
            {
                if (checkeds[i] > 0)
                {
                    p++;
                    r += checkeds[i] - 1;
                }
            }
            fitness = checkpoints.Count - p + r + s;
        }

        public Chromosome Crossover(Chromosome pair)
        {
            float[,] f1 = new float[steps + 1, 8];
            f1[0, 5] = this.moves[0 , 5];
            f1[0, 6] = this.moves[0 , 6];
            Parallel.For(0, steps + 1, i =>
            {
                if(i%2 == 0)
                {
                    f1[i, 0] = this.moves[i, 0];
                    f1[i, 1] = this.moves[i, 1];
                    f1[i, 2] = this.moves[i, 2];
                }
                else
                {
                    f1[i, 0] = pair.moves[i, 0];
                    f1[i, 1] = pair.moves[i, 1];
                    f1[i, 2] = pair.moves[i, 2];
                }
                
            });
            return new Chromosome(this.steps, this.checkpoints, this.map, moves);
        }

        public void Mutate(double rate)
        {
            double decidir = random.NextDouble();
            float a, b, s;
            if (decidir < rate)
            {
                a = random.Next(11) * 0.1f + minimum[1];
                b = random.Next(11) * 0.1f + minimum[1];
                s = random.Next(26) * 0.2f + minimum[2];
                //a = (float)random.NextDouble() * (maximum[0] - minimum[0]) + minimum[0];
                //b = (float)random.NextDouble() * (maximum[1] - minimum[1]) + minimum[1];
                //s = (float)random.NextDouble() * (maximum[2] - minimum[2]) + minimum[2];
                int pos = random.Next(steps);
                moves[pos,0] = a;
                moves[pos, 1] = b;
                moves[pos, 2] = s;
                CalcFitness();
            }
        }
    }
}
