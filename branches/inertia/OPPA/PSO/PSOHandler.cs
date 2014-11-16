using OPPA.Fuzzy;
using OPPA.PSO.Neighborhood;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPPA.PSO
{
    public class PSOHandler
    {
        private List<Particle> swarm;
        private List<PointF> checkpoints;
        private INeighborhood neighborhood;
        private bool[,] map;



        private string result;

        public PSOHandler(int particles, int moves, PointF start, List<PointF> checkpoints, Bitmap world)
        {
            this.checkpoints = checkpoints;
            MapWorld(world);
            GenerateSwarm(particles, moves, start, world);
            neighborhood = new RingNeighborhood();
            neighborhood.Arrange(swarm);
            //swarm[0].Best();
            SetupResult(particles);
        }

        public Particle Run(int iterations)
        {
            for (int k = 0; k < iterations; k++ )
            {
                Parallel.ForEach(swarm, p =>
                    {
                        p.Move();
                    });
                UpdateResult();
            }
            CSV();
            return FindBestParticle();
        }

        public void CSV()
        {
            File.WriteAllText(@"C:\Users\Public\result_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".csv", result);
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
            Parallel.For(0, particles, i =>
            {
                swarm.Add(new Particle(moves, start, checkpoints, map));
            });
        }

        private void SetupResult(int particles)
        {
            result = "P0";
            for(int i = 1; i < particles; i++)
            {
                result += ";P" + i;
            }
            result += "\r\n";
        }

        private void UpdateResult()
        {
            result += swarm[0].BestEvaluation;
            for (int i = 1; i < swarm.Count; i++)
            {
                result += ";" + swarm[i].BestEvaluation;
            }
            result += "\r\n";
        }
    }
}
