using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.WorldData
{
    public class BlockState
    {
        public int Id { get; set; }
        public string BlockName { get; set; }

        public BlockState(int id)
        {
            Id = id;
        }

        public BlockState(int id, string blockName)
        {
            Id = id;
            BlockName = blockName;
        }
    }
}
