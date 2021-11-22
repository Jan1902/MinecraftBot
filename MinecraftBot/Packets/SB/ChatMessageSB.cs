using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets.SB
{
    class ChatMessageSB : PacketBase
    {
        public string Message { get; set; }

        public ChatMessageSB() : base(OPPlay.ChatMessageSB) { }

        public void Create()
        {
            AddString(Message);
        }
    }
}
