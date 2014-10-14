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
        /// </summary>
        float[,] moves;
        int steps, actual;
        List<PointF> checkpoints;
        float[] weight;
        Bitmap map;

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

        
        public Particle(int steps, List<PointF> checkpoints)
        {
            this.steps = steps;
            moves = new float[steps+1, 7];
            RandomMoves();
            this.checkpoints = checkpoints;
            weight = new float[steps + 1];
        }

        /// <summary>
        /// Generate random particle's moves
        /// </summary>
        private void RandomMoves()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            moves[0, 3] = (float)r.NextDouble() * 190f;
            moves[0, 5] = 170;
            moves[0, 6] = 135;
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
            map = new Bitmap(1000, 700);
            Car c = new Car(55, 30);
            Graphics g = Graphics.FromImage(map);
            int chck = 0;
            float sum = 0;
            float w;
            for (int i = 1; i <= steps; i++)
            {
                c.Speed = moves[i - 1, 3];
                c.X = moves[i - 1, 5];
                c.Y = moves[i - 1, 6];
                c.Draw(g);
                moves[i, 5] = c.X;
                moves[i, 6] = c.Y;
                w = (float)Math.Sqrt(
                    Math.Pow((c.X - checkpoints[chck].X),2)
                    + Math.Pow((c.Y - checkpoints[chck].Y), 2));
                if (w == 0) chck++;
                sum += w;
                weight[i - 1] = w;
            }
            g.Dispose();
            return sum;
        }
    }
}
