using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets.SB
{
    class LoginStart : PacketBase
    {
        public string Username { get; set; }

        public LoginStart() : base(OPLogin.LoginStart) { }

        public void Create()
        {
            AddString(Username);
        }
    }
}
