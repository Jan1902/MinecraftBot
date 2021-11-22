using MinecraftBot.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MinecraftBot.Actions
{
    class PathfindAction : IAction
    {
        private Node _goal;

        public PathfindAction(int x, int y, int z)
        {
            _goal = new Node(x, y, z);
        }

        public void Execute()
        {
            Client.Instance.SendChatMessage(String.Format("Started calculating path to {0}", _goal.X + "/" + _goal.Y + "/" + _goal.Z));

            var playerX = (int)Math.Round(Client.Instance.LocalPlayer.X - .5);
            var playerY = (int)Client.Instance.LocalPlayer.Y;
            var playerZ = (int)Math.Round(Client.Instance.LocalPlayer.Z - .5);

            var path = new PathFinder().FindPath(new Node(playerX, playerY, playerZ), new Node(_goal.X, _goal.Y, _goal.Z));
            if (path != null)
                Client.Instance.SendChatMessage(String.Format("Found path with {0} moves", path.Nodes.Count));
            else
            {
                Client.Instance.SendChatMessage("No Path found");
                return;
            }

            foreach (var node in path.Nodes)
            {
                var move = path.Movements.ElementAt(path.Nodes.IndexOf(node));

                var player = Client.Instance.LocalPlayer;

                switch (move)
                {
                    case Movement.Walk:
                    case Movement.Fall:
                        player.LookAt(node.X + .5, node.Y + 1.6, node.Z + .5);
                        while(player.X != node.X + .5 || player.Z != node.Z + .5)
                        {
                            player.Interpolate(node.X + .5, node.Y, node.Z + .5);
                            Thread.Sleep(50);
                        }
                        break;
                    case Movement.JumpGap:
                        player.LookAt(node.X + .5, node.Y + 1.6, node.Z + .5);
                        player.Jump();
                        while (player.X != node.X + .5 || player.Z != node.Z + .5)
                        {
                            player.Interpolate(node.X + .5, node.Y, node.Z + .5);
                            Thread.Sleep(50);
                        }
                        break;
                    case Movement.JumpUp:
                        player.LookAt(node.X + .5, node.Y + 1.6, node.Z + .5);
                        player.Jump();
                        while(player.Y <= node.Y) { }
                        while (player.X != node.X + .5 || player.Z != node.Z + .5)
                        {
                            player.Interpolate(node.X + .5, node.Y, node.Z + .5);
                            Thread.Sleep(50);
                        }
                        break;                    
                    case Movement.Tower:
                        break;
                    case Movement.Dig:
                        break;
                    default:
                        break;
                }
            }

            Client.Instance.SendChatMessage("Arrived at location");
        }
    }
}
