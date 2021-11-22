using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets.SB
{
    class PlayerPositionAndLookSB : PacketBase
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public bool OnGround { get; set; }

        public PlayerPositionAndLookSB() : base(OPPlay.PlayerPositionAndLookSB) { }

        public void Create()
        {
            AddDouble(X);
            AddDouble(Y);
            AddDouble(Z);
            AddFloat(Yaw);
            AddFloat(Pitch);
            AddBool(OnGround);
        }
    }
}
