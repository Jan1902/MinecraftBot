using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets.SB
{
    class KeepAliveSB : PacketBase
    {
        public long KeepAliveID { get; set; }

        public KeepAliveSB() : base(OPPlay.KeepAliveSB) { }

        public void Create()
        {
            AddLong(KeepAliveID);
        }
    }
}
