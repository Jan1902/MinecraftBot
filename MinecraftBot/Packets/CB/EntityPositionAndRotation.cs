using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets.CB
{
    class EntityPositionAndRotation : PacketBase
    {
        public int EntityID { get; set; }
        public short DeltaX { get; set; }
        public short DeltaY { get; set; }
        public short DeltaZ { get; set; }
        public byte Yaw { get; set; }
        public byte Pitch { get; set; }
        public bool OnGround { get; set; }

        public EntityPositionAndRotation(byte[] data) : base(data)
        {
            FromData();
        }

        private void FromData()
        {
            EntityID = GetVarInt();
            DeltaX = GetShort();
            DeltaY = GetShort();
            DeltaZ = GetShort();
            Yaw = GetByte();
            Pitch = GetByte();
            OnGround = GetBool();
        }
    }
}
