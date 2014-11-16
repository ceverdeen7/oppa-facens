using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPPA.PSO.Neighborhood
{
    public class RingNeighborhood : INeighborhood
    {
        public virtual void Arrange(List<Particle> swarm)
        {
            int maxIndex = (swarm.Count - 1);
            for (int i = 1; i < maxIndex; i++)
            {
                swarm[i].Neighbors.Add(swarm[i - 1]);
                swarm[i].Neighbors.Add(swarm[i + 1]);
            }
            swarm[0].Neighbors.Add(swarm[maxIndex]);
            swarm[0].Neighbors.Add(swarm[1]);
            swarm[maxIndex].Neighbors.Add(swarm[maxIndex - 1]);
            swarm[maxIndex].Neighbors.Add(swarm[0]);
        }
    }
}
