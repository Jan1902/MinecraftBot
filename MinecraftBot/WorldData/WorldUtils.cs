using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.WorldData
{
    class WorldUtils
    {
        public static IPalette ChoosePalette(byte bitsPerBlock, Client client)
        {
            if (bitsPerBlock <= 4)
            {
                return new IndirectPalette(4, client);
            }
            else if (bitsPerBlock <= 8)
            {
                return new IndirectPalette(bitsPerBlock, client);
            }
            else
            {
                return new DirectPalette(client);
            }
        }
    }
}
