using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.WorldData
{
    public class DirectPalette : IPalette
    {
        private Client client;

        public DirectPalette(Client client)
        {
            this.client = client;
        }

        public uint IdForState(BlockState state)
        {
            return (uint)client.GlobalPalette.GetGlobalPaletteIdFromState(state);
        }

        public BlockState StateForId(uint id)
        {
            return client.GlobalPalette.GetStateFromGlobalPaletteId(id);
        }

        public byte GetBitsPerBlock()
        {
            return (byte)Math.Ceiling(Math.Log(client.GlobalPalette.StateFromId.Count, 2));
            //return 14; //Math.Ceiling(Math.Log(BlockState.TotalNumberOfStates, 2)) currently 14
        }

        public void Read(Stream stream)
        {
            // No Data
        }
    }
}
