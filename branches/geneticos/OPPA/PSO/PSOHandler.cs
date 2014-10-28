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
        private Particle best;
        private bool[,] map;

        public PSOHandler(int particles, int moves, PointF start, List<PointF> checkpoints, Bitmap world)
        {
            this.checkpoints = checkpoints;
            MapWorld(world);
            GenerateSwarm(particles, moves, start, world);
            best = FindBestParticle();
        }

        public Particle Run(int interations)
        {
            for (int k = 0; k < interations; k++ )
            {
                Parallel.ForEach(swarm, p =>
                    {
                        p.Move(best);
                    });
                best = FindBestParticle();
            }     
            return best;
        }

        private void MapWorld(Bitmap world)
        {
            map = new bool[world.Width, world.Height];
            Color c = Color.FromArgb(127, 127, 127);
            for(int j = 0; j < world.Height; j++)
            {
                for(int i = 0; i < world.Width; i++)
                {
                    map[i, j] = !c.Equals(world.GetPixel(i, j));
                }
            }
        }

        private Particle FindBestParticle()
        {
            Particle best = swarm[0];
            for (int i = 1; i < swarm.Count; i++)
            {
                if (swarm[i].BestEvaluation < best.BestEvaluation)
                {
                    best = swarm[i];
                }
            }
            return best;
        }

        private void GenerateSwarm(int particles, int moves, PointF start, Bitmap world)
        {
            swarm = new List<Particle>(particles);
            Parallel.For(0, particles - 1, i =>
            {
                swarm.Add(new Particle(moves, start, checkpoints, map));
            });
            Particle p = new Particle(moves, start, checkpoints, map);
            p.Best();
            swarm.Add(p);
        }
    }
}
