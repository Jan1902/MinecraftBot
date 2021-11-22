using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Pathfinding
{
    class Path
    {
        public List<Node> Nodes { get; set; }
        public List<Movement> Movements { get; set; }

        public Path(List<Node> nodes, List<Movement> movements)
        {
            Nodes = nodes;
            Movements = movements;
        }

        public Path()
        {

        }
    }
}
