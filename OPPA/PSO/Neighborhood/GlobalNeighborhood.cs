using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPPA.PSO.Neighborhood
{
    public class GlobalNeighborhood : INeighborhood
    {
        public void Arrange(List<Particle> swarm)
        {
            for (int i = 0; i < swarm.Count; i++)
            {
                for (int j = 0; j < swarm.Count; j++)
                {
                    if (i != j)
                    {
                        swarm[i].Neighbors.Add(swarm[j]);
                    }
                }
            }
        }
    }
}
