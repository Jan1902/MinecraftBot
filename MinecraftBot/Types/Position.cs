using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot
{
    class Position
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Position(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Position()
        {

        }

        public static Position ReadPosition(Stream stream)
        {
            var ulongBytes = new byte[8];
            stream.Read(ulongBytes, 0, 8);
            Array.Reverse(ulongBytes);

            var val = BitConverter.ToUInt64(ulongBytes, 0);
            var pos = new Position();
            pos.X = val >> 38;
            pos.Y = val & 0xFFF;
            pos.Z = val << 26 >> 38;
            if (pos.X >= Math.Pow(2, 25)) { pos.X -= Math.Pow(2, 26); }
            if (pos.Y >= Math.Pow(2, 11)) { pos.Y -= Math.Pow(2, 12); }
            if (pos.Z >= Math.Pow(2, 25)) { pos.Z -= Math.Pow(2, 26); }
            return pos;
        }

        public static long PositionToLong(int x, int y, int z)
        {
            return ((x & 0x3FFFFFF) << 38) | ((z & 0x3FFFFFF) << 12) | (y & 0xFFF);
        }
    }
}
