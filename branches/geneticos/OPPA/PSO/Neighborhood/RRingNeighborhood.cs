using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPPA.PSO.Neighborhood
{
    public class RRingNeighborhood : INeighborhood
    {
        public void Arrange(List<Particle> swarm)
        {
            int maxIndex = (swarm.Count - 1);
            for (int i = 1; i < maxIndex; i++)
            {
                swarm[i].Neighbors.Add(swarm[i - 1]);
                swarm[i].Neighbors.Add(swarm[i + 1]);
                RandomNeighbor(swarm[i], swarm);
            }
            swarm[0].Neighbors.Add(swarm[maxIndex]);
            swarm[0].Neighbors.Add(swarm[1]);
            RandomNeighbor(swarm[0], swarm);
            swarm[maxIndex].Neighbors.Add(swarm[maxIndex - 1]);
            swarm[maxIndex].Neighbors.Add(swarm[0]);
            RandomNeighbor(swarm[maxIndex], swarm);
        }

        private void RandomNeighbor(Particle p, List<Particle> swarm)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            Particle n = swarm[random.Next(swarm.Count)];
            if (p != n && !p.Neighbors.Contains(n))
            {
                p.Neighbors.Add(n);
                if (!n.Neighbors.Contains(p)) n.Neighbors.Add(p);
            }
        }
    }
}
