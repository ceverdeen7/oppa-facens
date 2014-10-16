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
        /// List of particle's moves
        /// moves[i,0] = Acceleration
        /// moves[i,1] = Brake
        /// moves[i,2] = SteeringWheel
        /// moves[i,3] = Speed
        /// moves[i,4] = WheelAngle 
        /// moves[i,5] = X
        /// moves[i,6] = Y
        /// moves[i,7] = Angle
        /// </summary>
        float[,] moves;
        int steps, actual;
        float[] weight;
        PointF goal;

        public float Acceleration
        {
            get { return moves[actual, 0]; }
        }

        public float Brake
        {
            get { return moves[actual, 1]; }
        }

        public float SteeringWheel
        {
            get { return moves[actual, 2]; }
        }

        /// <summary>
        /// Actual speed of the car
        /// </summary>
        public float Speed
        {
            get { return moves[actual, 3]; }
            set { moves[actual + 1, 3] = value; }
        }

        public float WheelAngle
        {
            get { return moves[actual, 4]; }
            set { moves[actual + 1, 4] = value; }
        }

        public bool HasMoves
        {
            get { return !(actual == steps); }
        }

        
        public Particle(int steps, PointF start, PointF goal)
        {
            this.steps = steps;
            moves = new float[steps+1, 8];
            this.goal = goal;
            RandomMoves(start);
            weight = new float[steps + 1];
        }

        /// <summary>
        /// Generate random particle's moves
        /// </summary>
        private void RandomMoves(PointF start)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            moves[0, 3] = (float)r.NextDouble() * 190f;
            moves[0, 5] = start.X;
            moves[0, 6] = start.Y;

            Parallel.For(0, steps + 1, i =>
            {
                moves[i, 0] = (float)r.NextDouble();
                moves[i, 1] = (float)r.NextDouble();
                moves[i, 2] = (float)r.NextDouble()*5.0f - 2.5f;
            });
        }

        public void NextMove()
        {
            if(HasMoves) actual++;
        }

        public float Evaluate()
        {
            Car c = new Car(55);
            float sum = 0;
            float w;
            int i = 1;
            weight[0] = w = (float)Math.Sqrt(
                    Math.Pow((moves[0, 5] - goal.X), 2)
                    + Math.Pow((moves[0, 6] - goal.Y), 2));
            while (w != 0 && i <= steps)
            {
                c.Speed = moves[i - 1, 3];
                c.X = moves[i - 1, 5];
                c.Y = moves[i - 1, 6];
                c.Angle = moves[i - 1, 7];
                c.Move();
                moves[i, 5] = c.X;
                moves[i, 6] = c.Y;
                moves[i, 7] = c.Angle;
                w = (float)Math.Sqrt(
                    Math.Pow((c.X - goal.X), 2)
                    + Math.Pow((c.Y - goal.Y), 2));
                sum += w;
                weight[i] = w;
                i++;
            }
            return sum;
        }
    }
}
