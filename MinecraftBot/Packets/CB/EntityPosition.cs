using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets.CB
{
    class EntityPosition : PacketBase
    {
        public int EntityID { get; set; }
        public short DeltaX { get; set; }
        public short DeltaY { get; set; }
        public short DeltaZ { get; set; }
        public bool OnGround { get; set; }

        public EntityPosition(byte[] data) : base(data)
        {
            FromData();
        }

        private void FromData()
        {
            EntityID = GetVarInt();
            DeltaX = GetShort();
            DeltaY = GetShort();
            DeltaZ = GetShort();
            OnGround = GetBool();
        }
    }
}
