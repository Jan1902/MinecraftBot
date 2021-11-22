using MinecraftBot.WorldData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Pathfinding
{
    class PathFinder
    {
        private const int WalkCost = 10;
        private const int DiagonalWalkCost = 14;
        private const int JumpCost = 5;

        private const int MaxDistance = 150;

        private float GetDistance(Node a, Node b)
        {
            float deltaX = b.X - a.X;
            float deltaY = b.Y - a.Y;
            float deltaZ = b.Z - a.Z;

            //return (float)Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2) + Math.Pow(deltaZ, 2));
            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
        }

        public Path FindPath(Node start, Node goal)
        {
            var startTime = DateTime.Now;
            start.H = (int)(GetDistance(start, goal) * WalkCost);
            var open = new List<Node>();
            open.Add(start);
            //var closed = new List<Node>();
            var closed = new HashSet<Node>();
            var cameFrom = new Dictionary<Node, Node>();
            var action = new Dictionary<Node, Movement>();
            var done = false;
            var considered = 0;
            var world = Client.Instance.World;

            Node current;
            while (open.Count > 0)
            {
                //current = open.OrderBy(n => n.F).First();
                current = open[0];
                foreach (var tmp in open.Skip(1))
                {
                    if (tmp.F < current.F)
                        current = tmp;
                }

                open.Remove(current);
                closed.Add(current);

                if (current == goal)
                {
                    done = true;
                    break;
                }

                foreach (var neighbour in GetNeighbours(current))
                {
                    if (GetDistance(neighbour, start) > MaxDistance)
                        continue;

                    if (closed.Contains(neighbour))
                        continue;

                    if (neighbour.Y > current.Y) //Jump up
                    {
                        if (!IsWalkable(neighbour)
                            || world.IsSolid(current.Up().Up()))
                            continue;

                        neighbour.G = current.G + WalkCost + JumpCost;
                        action[neighbour] = Movement.JumpUp;
                    }
                    else if(neighbour.Y < current.Y) //Jump Down
                    {
                        if (!IsWalkable(neighbour)
                            || world.IsSolid(neighbour.Up().Up()))
                            continue;

                        neighbour.G = current.G + WalkCost + JumpCost;
                        action[neighbour] = Movement.Fall;
                    }
                    else //Walk
                    {
                        if (!IsWalkable(neighbour))
                            continue;

                        if (neighbour.X != current.X && neighbour.Z != current.Z) //Diagonal
                        {
                            if (world.IsSolid(new Node(neighbour.X, current.Y, current.Z))
                                || world.IsSolid(new Node(neighbour.X, current.Y, current.Z).Up())
                                || world.IsSolid(new Node(current.X, current.Y, neighbour.Z))
                                || world.IsSolid(new Node(current.X, current.Y, neighbour.Z).Up()))
                                continue;

                            neighbour.G = current.G + DiagonalWalkCost;
                            action[neighbour] = Movement.Walk;
                        }
                        else //Straight
                        {
                            if(GetDistance(current, neighbour) >= 2) //Straight Jump
                            {
                                var nodeBetween = new Node(current.X + (neighbour.X - current.X) / 2, current.Y, current.Z + (neighbour.Z - current.Z) / 2);
                                if (world.IsSolid(current.Up().Up())
                                    || world.IsSolid(neighbour.Up().Up())
                                    || world.IsSolid(nodeBetween)
                                    || world.IsSolid(nodeBetween.Up())
                                    || world.IsSolid(nodeBetween.Up().Up()))
                                    continue;

                                neighbour.G = current.G + 2 * WalkCost + JumpCost;
                                action[neighbour] = Movement.JumpGap;
                            }
                            else //Straight Walk
                            {
                                neighbour.G = current.G + WalkCost;
                                action[neighbour] = Movement.Walk;
                            }
                        }
                    }

                    considered++;

                    var previousEntry = open.FirstOrDefault(n => n == neighbour);
                    if (previousEntry == null || neighbour.G < previousEntry.G)
                    {
                        neighbour.H = (int)(GetDistance(neighbour, goal) * WalkCost);
                        cameFrom[neighbour] = current;
                        open.Add(neighbour);
                    }
                }
            }

            Client.Instance.SendChatMessage(String.Format("{0} possible moves considered", considered));

            if (!done)
                return null;

            Client.Instance.SendChatMessage(String.Format("Calculation took {0} ms", (DateTime.Now - startTime).TotalMilliseconds));
            Console.WriteLine(closed.Count);

            var currentNode = goal;
            var path = new List<Node>();
            var moves = new List<Movement>();
            while (currentNode != start)
            {
                path.Add(currentNode);
                moves.Add(action[action.Keys.First(k => k == currentNode)]);
                currentNode = cameFrom[cameFrom.Keys.First(k => k == currentNode)];
            }
            path.Add(start);
            path.Reverse();
            moves.Add(Movement.Walk);
            moves.Reverse();

            Visualization.PrintPathfindingData(open, closed.ToList(), path);
            return new Path(path, moves);
        }

        private bool IsWalkable(Node node)
        {
            var world = Client.Instance.World;
            var block = world.GetBlock(node.X, node.Y, node.Z);
            return world.IsSolid(node.Down())
                && !world.IsSolid(node)
                && !world.IsSolid(node.Up())
                && !block.BlockName.Contains("water")
                && !block.BlockName.Contains("lava");
        }

        private Node[] GetNeighbours(Node node)
        {
            var neighbours = new List<Node>();

            //neighbours.Add(node.Up()); //pillar up
            //neighbours.Add(node.Down()); //dig down

            neighbours.Add(node.North()); //simple walk
            neighbours.Add(node.East());
            neighbours.Add(node.South());
            neighbours.Add(node.West());

            neighbours.Add(node.North().East()); //diagonal walk
            neighbours.Add(node.North().West());
            neighbours.Add(node.South().East());
            neighbours.Add(node.South().West());

            neighbours.Add(node.North().Up()); //jump up
            neighbours.Add(node.East().Up());
            neighbours.Add(node.South().Up());
            neighbours.Add(node.West().Up());

            neighbours.Add(node.North().Down()); //fall down
            neighbours.Add(node.East().Down());
            neighbours.Add(node.South().Down());
            neighbours.Add(node.West().Down());

            neighbours.Add(node.North().North()); //jump 1 wide gap
            neighbours.Add(node.East().East());
            neighbours.Add(node.South().South());
            neighbours.Add(node.West().West());

            return neighbours.ToArray();
        }
    }

    public enum Movement
    {
        Walk,
        JumpGap,
        JumpUp,
        Fall,
        Tower,
        Dig
    }
}
