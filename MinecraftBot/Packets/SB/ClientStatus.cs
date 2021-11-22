using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets.SB
{
    class ClientStatus : PacketBase
    {
        public int ActionID { get; set; }

        public ClientStatus() : base(OPPlay.ClientStatus) { }

        public void Create()
        {
            AddVarInt(ActionID);
        }
    }
}
