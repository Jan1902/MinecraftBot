using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client(25565, "127.0.0.1");
        }
    }
}
