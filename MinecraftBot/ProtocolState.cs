using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot
{
    public enum ProtocolState : int
    {
        Handshaking = 0,
        Status = 1,
        Login = 2,
        Play = 3
    }
}
