using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets.SB
{
    class PlayerBlockPlacement : PacketBase
    {
        public int Hand { get; set; }
        public long Location { get; set; }
        public int Face { get; set; }
        public float CursorPositionX { get; set; }
        public float CursorPositionY { get; set; }
        public float CursorPositionZ { get; set; }

        public bool InsideBlock { get; set; }

        public PlayerBlockPlacement() : base(OPPlay.PlayerBlockPlacement) { }

        public void Create()
        {
            AddVarInt(Hand);
            AddLong(Location);
            AddVarInt(Face);
            AddFloat(CursorPositionX);
            AddFloat(CursorPositionY);
            AddFloat(CursorPositionZ);
            AddBool(InsideBlock);
        }
    }
}
