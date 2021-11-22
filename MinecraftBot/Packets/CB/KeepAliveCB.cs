using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets.CB
{
    class KeepAliveCB : PacketBase
    {
        public long KeepAliveID { get; set; }

        public KeepAliveCB(byte[] data) : base(data)
        {
            FromBytes();
        }

        public void FromBytes()
        {
            KeepAliveID = GetLong();
        }
    }
}
