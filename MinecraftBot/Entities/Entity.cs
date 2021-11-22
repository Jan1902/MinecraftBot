using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Entities
{
    public class Entity
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public float Yaw { get; set; }
        public float Pitch { get; set; }

        public int EntityID { get; set; }
    }
}
