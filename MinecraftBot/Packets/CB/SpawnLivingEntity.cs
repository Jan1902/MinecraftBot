using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets.CB
{
    class SpawnLivingEntity : PacketBase
    {
        public int EntityID { get; set; }
        public Guid EntityUUID { get; set; }
        public int Type { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public byte Pitch { get; set; }
        public byte Yaw { get; set; }
        public byte HeadPitch { get; set; }
        public short VelocityX { get; set; }
        public short VelocityY { get; set; }
        public short VelocityZ { get; set; }

        public SpawnLivingEntity(byte[] data) : base(data)
        {
            FromData();
        }

        public void FromData()
        {
            EntityID = GetVarInt();
            GetULong();
            GetULong();
            Type = GetVarInt();
            X = GetDouble();
            Y = GetDouble();
            Z = GetDouble();
            Yaw = GetByte();
            Pitch = GetByte();
            HeadPitch = GetByte();
            VelocityX = GetShort();
            VelocityY = GetShort();
            VelocityZ = GetShort();
        }
    }
}
