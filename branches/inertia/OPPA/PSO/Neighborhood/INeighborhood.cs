using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPPA.PSO.Neighborhood
{
    public interface INeighborhood
    {
        void Arrange(List<Particle> swarm);
    }
}
