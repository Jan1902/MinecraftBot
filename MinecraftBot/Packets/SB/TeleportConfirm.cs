using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets.SB
{
    class TeleportConfirm : PacketBase
    {
        public int TeleportID { get; set; }

        public TeleportConfirm() : base(OPPlay.TeleportConfirm) { }

        public void Create()
        {
            AddVarInt(TeleportID);
        }
    }
}
