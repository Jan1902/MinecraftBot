using MinecraftBot.Pathfinding;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot
{
    class Visualization
    {
        public static void PrintPathfindingData(List<Node> open, List<Node> closed, List<Node> path)
        {
            var overhead = 50;
            var minX = path.Min(n => n.X) - overhead;
            var minZ = path.Min(n => n.Z) - overhead;
            var maxX = path.Max(n => n.X) + overhead;
            var maxZ = path.Max(n => n.Z) + overhead;

            var deltaX = maxX - minX;
            var deltaZ = maxZ - minZ;

            var size = deltaX > deltaZ ? deltaX : deltaZ;

            var rectSize = 1000f / size;

            var bmp = new Bitmap(1000, 1000);
            var graphics = Graphics.FromImage(bmp);

            graphics.FillRectangle(Brushes.White, 0, 0, 1000, 1000);

            foreach (var node in open)
            {
                graphics.FillRectangle(Brushes.LightBlue, (node.X - minX) * rectSize, (node.Z - minZ) * rectSize, rectSize, rectSize);
            }

            foreach (var node in closed)
            {
                graphics.FillRectangle(Brushes.Blue, (node.X - minX) * rectSize, (node.Z - minZ) * rectSize, rectSize, rectSize);
            }

            foreach (var node in path)
            {
                graphics.FillRectangle(Brushes.Green, (node.X - minX) * rectSize, (node.Z - minZ) * rectSize, rectSize, rectSize);
            }

            bmp.Save("Visualization.jpg");
        }
    }
}
