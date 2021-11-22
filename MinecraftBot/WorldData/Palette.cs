using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.WorldData
{
    public interface IPalette
    {
        uint IdForState(BlockState state);
        BlockState StateForId(uint id);
        byte GetBitsPerBlock();
        void Read(Stream stream);
    }
}
