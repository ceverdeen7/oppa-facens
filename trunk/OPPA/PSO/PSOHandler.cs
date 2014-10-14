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

        public PSOHandler(int particles, int moves)
        {
            GenerateSwarm(particles, moves);
        }

        public void UpdateSwarm()
        {
            Parallel.ForEach(swarm, p => {
                FIS fis = new FIS();
                while (p.HasMoves)
                {
                    p.Speed = fis.getSpeed(p.Speed, p.Acceleration, p.Brake);
                    p.WheelAngle = fis.getWheelAngle(p.SteeringWheel);
                    p.NextMove();
                }
            });
        }

        public void GenerateSwarm(int particles, int moves)
        {
            swarm = new List<Particle>(particles);
            Parallel.For(0, particles, i =>
            {
                swarm.Add(new Particle(moves));
            });
        }
    }
}
