using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.WorldData
{
    public class ChunkSection
    {
        public BlockState[,,] BlockStates { get; private set; } = new BlockState[16,16,16];
        public int[,,] BlockLights { get; private set; } = new int[16, 16, 16];
        public int[,,] SkyLights { get; private set; } = new int[16, 16, 16];
    }
}
