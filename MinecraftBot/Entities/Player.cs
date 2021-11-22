using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Entities
{
    class Player : Entity
    {
        public Guid PlayerID { get; set; }

        public string Name { get; set; }
    }
}
