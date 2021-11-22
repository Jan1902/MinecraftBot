using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets.CB
{
    class PlayerPositionAndLookCB : PacketBase
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public byte Flags { get; set; }
        public int TeleportID { get; set; }

        public PlayerPositionAndLookCB(byte[] data) : base(data)
        {
            FromBytes();
        }

        public void FromBytes()
        {
            X = GetDouble();
            Y = GetDouble();
            Z = GetDouble();
            Yaw = GetFloat();
            Pitch = GetFloat();
            Flags = GetByte();
            TeleportID = GetVarInt();
        }
    }
}
