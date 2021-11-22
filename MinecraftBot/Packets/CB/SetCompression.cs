using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets.CB
{
    class SetCompression : PacketBase
    {
        public int Threshold { get; set; }

        public SetCompression(byte[] data) : base(data)
        {
            FromBytes();
        }

        public void FromBytes()
        {
            Threshold = GetVarInt();
        }
    }
}
