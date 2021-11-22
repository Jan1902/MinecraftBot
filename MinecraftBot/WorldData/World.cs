using MinecraftBot.Entities;
using MinecraftBot.Pathfinding;
using MinecraftBot.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.WorldData
{
    public class World
    {
        //public List<Chunk> Chunks = new List<Chunk>();
        public Dictionary<Vec2I, Chunk> Chunks = new Dictionary<Vec2I, Chunk>();

        public object ChunkLocker { get; set; } = new object();

        public List<Entity> Entities { get; set; } = new List<Entity>();

        private int GetChunkHash(int x, int z)
        {
            int hash = 17;
            hash = hash * 23 + x.GetHashCode();
            hash = hash * 23 + z.GetHashCode();
            return hash;
    }

        public BlockState GetBlock(int blockX, int blockY, int blockZ)
        {
            //lock(ChunkLocker)
            //{
            //    chunk = Chunks.Where(c => c.X == Math.Floor((double)blockX / 16) && c.Z == Math.Floor((double)blockZ / 16)).FirstOrDefault();
            //}

            var vec = new Vec2I((int)Math.Floor((double)blockX / 16), (int)Math.Floor((double)blockZ / 16));
            if (!Chunks.ContainsKey(vec))
                return new BlockState(0, "minecraft:air");

            Chunk chunk = Chunks[vec];
            //if(chunk == null)
            //    return new BlockState(0, "minecraft:air");

            var section = chunk.Sections[(int)Math.Floor((double)(blockY / 16))];
            if (section == null)
                return new BlockState(0, "minecraft:air");

            var offsetX = blockX % 16;
            var offsetY = blockY % 16;
            var offsetZ = blockZ % 16;
            offsetX = offsetX >= 0 ? offsetX : 16 + offsetX;
            offsetY = offsetY >= 0 ? offsetY : 16 + offsetY;
            offsetZ = offsetZ >= 0 ? offsetZ : 16 + offsetZ;
            return section.BlockStates[offsetX, offsetY, offsetZ];
        }

        public bool IsSolid(Node node)
        {
            var block = GetBlock(node.X, node.Y, node.Z);
            return !(block.BlockName.Substring(10) == "grass"
                || block.BlockName.Substring(10) == "tall_grass"
                || block.BlockName.Contains("poppy")
                || block.BlockName.Contains("dandelion")
                || block.BlockName.Contains("air")
                || block.BlockName.Contains("water")
                || block.BlockName.Contains("lava"));
        }

        public void SetBlock(int blockX, int blockY, int blockZ, int id)
        {
            //Chunk chunk;
            //lock (ChunkLocker)
            //{
            //    chunk = Chunks.Where(c => c.X == Math.Floor((double)blockX / 16) && c.Z == Math.Floor((double)blockZ / 16)).FirstOrDefault();
            //}
            //if (chunk == null)
            //    return;

            //var section = chunk.Sections[(int)Math.Floor((double)(blockY / 16))];
            //if (section == null)
            //    return;

            //var offsetX = blockX % 16;
            //var offsetY = blockY % 16;
            //var offsetZ = blockZ % 16;
            //offsetX = offsetX >= 0 ? offsetX : 16 + offsetX;
            //offsetY = offsetY >= 0 ? offsetY : 16 + offsetY;
            //offsetZ = offsetZ >= 0 ? offsetZ : 16 + offsetZ;
            //section.BlockStates[offsetX, offsetY, offsetZ] = Client.Instance.GlobalPalette.GetStateFromGlobalPaletteId((uint)id);
        }
    }
}
