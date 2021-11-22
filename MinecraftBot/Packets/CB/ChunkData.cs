using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets.CB
{
    class ChunkData : PacketBase
    {
        public int ChunkX { get; set; }
        public int ChunkZ { get; set; }
        public bool FullChunk { get; set; }
        public int PrimaryBitMask { get; set; }
        public long[] Heightmaps { get; set; }
        public long[] WorldSurface { get; set; }
        public int BiomesLength { get; set; }
        public int[] Biomes { get; set; }
        public int Size { get; set; }
        public byte[] Data { get; set; }
        public int BlockEntityCount { get; set; }
        //Block Entities

        public ChunkData(byte[] data) : base(data)
        {
            FromData();
        }

        private void FromData()
        {
            ChunkX = GetInt();
            ChunkZ = GetInt();
            FullChunk = GetBool();
            PrimaryBitMask = GetVarInt();

            GetByte(); //COMPOUND TAG
            GetUShort(); //COMPOUND NAME LENGTH (0)
            GetByte(); //LONG ARRAY TAG
            var nameLength = GetUShort();
            GetString(nameLength);
            var arrayLength = GetInt();
            Heightmaps = new long[arrayLength];
            for (int i = 0; i < arrayLength; i++)
            {
                Heightmaps[i] = GetLong();
            }
            if(GetByte() != 0) //EITHER COMPOUND END OR ANOTHER LONG ARRAY TAG
            {
                nameLength = GetUShort();
                GetString(nameLength);
                arrayLength = GetInt();
                WorldSurface = new long[arrayLength];
                for (int i = 0; i < arrayLength; i++)
                {
                    WorldSurface[i] = GetLong();
                }
            }
            GetByte(); //COMPOUND END TAG

            if (FullChunk)
            {
                BiomesLength = GetVarInt();
                Biomes = new int[BiomesLength];
                for (int i = 0; i < BiomesLength; i++)
                {
                    Biomes[i] = GetVarInt();
                }
            }
            Size = GetVarInt();
            Data = GetBytes(Size);
            BlockEntityCount = GetVarInt();
            //Block Entities
        }
    }
}
