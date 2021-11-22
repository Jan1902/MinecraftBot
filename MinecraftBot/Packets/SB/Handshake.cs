using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets.SB
{
    class Handshake : PacketBase
    {
        public int ProtocolVersion { get; set; }
        public string ServerAddress { get; set; }
        public ushort ServerPort { get; set; }
        public int NextState { get; set; }

        public Handshake() : base(OPHandshaking.Handshake) { }

        public void Create()
        {
            AddVarInt(ProtocolVersion);
            AddString(ServerAddress);
            AddUShort(ServerPort);
            AddVarInt(NextState);
        }
    }
}
