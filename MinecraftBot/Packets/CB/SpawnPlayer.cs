using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets.CB
{
    class SpawnPlayer : PacketBase
    {
        public int EntityID { get; set; }
        public Guid PlayerUUID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public byte Yaw { get; set; }
        public byte Pitch { get; set; }

        public SpawnPlayer(byte[] data) : base(data)
        {
            FromData();
        }

        private void FromData()
        {
            EntityID = GetVarInt();
            GetULong();
            GetULong();
            X = GetDouble();
            Y = GetDouble();
            Z = GetDouble();
            Yaw = GetByte();
            Pitch = GetByte();
        }
    }
}
