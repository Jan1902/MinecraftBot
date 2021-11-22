using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Pathfinding
{
    public class Node/* : IEquatable<Node>*/
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public int G { get; set; }
        public int H { get; set; }

        public int F { get => G + H; }

        public Node CameFrom { get; set; }

        public Node(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Node()
        {

        }

        public Node Down()
        {
            return new Node(X, Y - 1, Z);
        }

        public Node Up()
        {
            return new Node(X, Y + 1, Z);
        }

        public Node East()
        {
            return new Node(X + 1, Y, Z);
        }

        public Node West()
        {
            return new Node(X - 1, Y, Z);
        }

        public Node South()
        {
            return new Node(X, Y, Z + 1);
        }
        public Node North()
        {
            return new Node(X, Y, Z - 1);
        }

        //public bool Equals(Node other)
        //{
        //    return other.X == X && other.Y == Y && other.Z == Z;
        //}

        public override bool Equals(object obj)
        {
            if (!(obj is Node))
                return false;

            return this == (Node)obj;
        }

        public static bool operator ==(Node a, Node b)
        {
            if (a is null && b is object || b is null && a is object)
                return false;
            if (a is null && b is null)
                return true;
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Node a, Node b)
        {
            if (a is null && b is object || b is null && a is object)
                return true;
            if (a is null && b is null)
                return false;

            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        //public static bool operator ==(Node a, Node b)
        //{
        //    return a.GetHashCode() == b.GetHashCode();
        //}

        //public static bool operator !=(Node a, Node b)
        //{
        //    return a.GetHashCode() != b.GetHashCode();
        //}

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + X.GetHashCode();
            hash = hash * 23 + Y.GetHashCode();
            hash = hash * 23 + Z.GetHashCode();
            return hash;
        }
    }
}
