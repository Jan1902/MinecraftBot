using CaptiveAire.EndianUtil.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets.CB
{
    class LoginSuccess : PacketBase
    {
        public Guid UUID { get; set; }
        public string Username { get; set; }

        public LoginSuccess(byte[] data) : base(data)
        {
            FromBytes();
        }

        public void FromBytes()
        {
            var bytes = new byte[16];
            Array.Copy(EndianBitConverter.Big.GetBytes(GetULong()), bytes, 8);
            Array.Copy(EndianBitConverter.Big.GetBytes(GetULong()), bytes, 8);
            UUID = new Guid(bytes);
            Username = GetString();
        }
    }
}
