using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets.CB
{
    class BlockChange : PacketBase
    {
        public Position Location { get; set; }

        public int BlockID { get; set; }

        public BlockChange(byte[] data) : base(data)
        {
            FromData();
        }

        public void FromData()
        {
            using (var stream = new MemoryStream(bytes.ToArray()))
            {
                Location = Position.ReadPosition(stream);
                GetBytes((int)stream.Position);
            }
            BlockID = GetVarInt();
        }
    }
}
