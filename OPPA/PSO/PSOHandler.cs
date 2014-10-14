using OPPA.Fuzzy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPPA.PSO
{
    public class PSOHandler
    {
        private List<Particle> swarm;
        private List<PointF> checkpoints;

        public PSOHandler(int particles, int moves, List<PointF> checkpoints)
        {
            this.checkpoints = checkpoints;
            GenerateSwarm(particles, moves);
        }

        public void UpdateSwarm()
        {
            float[] weights = new float[swarm.Count];
            Parallel.For(0, swarm.Count, i => {
                FIS fis = new FIS();
                Particle p = swarm[i];
                while (p.HasMoves)
                {
                    p.Speed = fis.getSpeed(p.Speed, p.Acceleration, p.Brake);
                    p.WheelAngle = fis.getWheelAngle(p.SteeringWheel);
                    p.NextMove();
                }
                weights[i] = p.Evaluate();
            });
        }

        public void GenerateSwarm(int particles, int moves)
        {
            swarm = new List<Particle>(particles);
            Parallel.For(0, particles, i =>
            {
                swarm.Add(new Particle(moves, checkpoints));
            });
        }
    }
}
