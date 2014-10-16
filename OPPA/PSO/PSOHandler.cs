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

        public PSOHandler(int particles, int moves, PointF start, List<PointF> checkpoints)
        {
            this.checkpoints = checkpoints;
            GenerateSwarm(particles, moves, start);
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

        public void GenerateSwarm(int particles, int moves, PointF start)
        {
            swarm = new List<Particle>(particles);
            Parallel.For(0, particles, i =>
            {
                // TODO: Remove hardcode
                swarm.Add(new Particle(moves, start, checkpoints[0]));
            });
        }
    }
}
