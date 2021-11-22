using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets.CB
{
    class ChatMessage : PacketBase
    {
        public string JsonData { get; set; }
        public byte Position { get; set; }

        public ChatMessage(byte[] data) : base(data)
        {
            FromBytes();
        }

        public void FromBytes()
        {
            JsonData = GetString();
            Position = GetByte();
        }
    }
}
