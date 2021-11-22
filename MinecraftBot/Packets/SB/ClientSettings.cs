using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets.SB
{
    class ClientSettings : PacketBase
    {
        public string Locale { get; set; }
        public byte ViewDistance { get; set; }
        public int ChatMode { get; set; }
        public bool ChatColors { get; set; }
        public byte DisplayedSkinParts { get; set; }
        public int MainHand { get; set; }

        public ClientSettings() : base(OPPlay.ClientSettings) { }

        public void Create()
        {
            AddString(Locale);
            AddByte(ViewDistance);
            AddVarInt(ChatMode);
            AddBool(ChatColors);
            AddByte(DisplayedSkinParts);
            AddVarInt(MainHand);
        }
    }
}
