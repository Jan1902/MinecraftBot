using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Types
{
    public class Vec2I
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vec2I()
        {

        }

        public Vec2I(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override int GetHashCode()
        {
            return (X, Y).GetHashCode();
            //int hash = 17;
            //hash = hash * 23 + X.GetHashCode();
            //hash = hash * 23 + Y.GetHashCode();
            //return hash;
        }

        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }
    }
}
