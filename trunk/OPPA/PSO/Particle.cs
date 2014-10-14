using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPPA.PSO
{
    public class Particle
    {
        float[,] moves;
        int steps, actual;

        /// <summary>
        /// List of particle's moves
        /// moves[i,0] = Acceleration
        /// moves[i,1] = Brake
        /// moves[i,2] = SteeringWheel
        /// moves[i,3] = Speed
        /// moves[i,4] = WheelAngle 
        /// </summary>
        public float[,] Moves
        {
            get { return moves; }
            set { moves = value; }
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="steps">Number of particle's moves.</param>
        public Particle(int steps)
        {
            this.steps = steps;
            actual = 0;
            moves = new float[steps+1, 5];
            RandomMoves();
        }

        /// <summary>
        /// Generate random particle's moves
        /// </summary>
        private void RandomMoves()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            Parallel.For(0, steps, i =>
            {
                moves[i, 0] = (float)r.NextDouble();
                moves[i, 1] = (float)r.NextDouble();
                moves[i, 2] = (float)r.NextDouble()*5.0f - 2.5f;
                moves[i, 3] = moves[i, 4] = 0;
            });
        }

        public void NextMove()
        {
            if(HasMoves) actual++;
        }
    }
}
