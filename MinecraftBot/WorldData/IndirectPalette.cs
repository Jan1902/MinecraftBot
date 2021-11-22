using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.WorldData
{
    public class IndirectPalette : IPalette
    {
        Dictionary<uint, BlockState> idToState;
        Dictionary<BlockState, uint> stateToId;
        byte bitsPerBlock;

        private Client client;

        public IndirectPalette(byte palBitsPerBlock, Client client)
        {
            bitsPerBlock = palBitsPerBlock;
            this.client = client;
        }

        public uint IdForState(BlockState state)
        {
            return stateToId[state];
        }

        public BlockState StateForId(uint id)
        {
            if (idToState.ContainsKey(id))
                return idToState[id];
            else
                return idToState[0];
        }

        public byte GetBitsPerBlock()
        {
            return bitsPerBlock;
        }

        public void Read(Stream stream)
        {
            idToState = new Dictionary<uint, BlockState>();
            stateToId = new Dictionary<BlockState, uint>();
            // Palette Length
            int length = VarInt.ReadVarInt(stream);
            // Palette
            for (int id = 0; id < length; id++)
            {
                uint stateId = (uint)VarInt.ReadVarInt(stream);
                BlockState state = client.GlobalPalette.GetStateFromGlobalPaletteId(stateId);
                idToState[(uint)id] = state;
                stateToId[state] = (uint)id;
            }
        }
    }
}
