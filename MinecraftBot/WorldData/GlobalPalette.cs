using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.WorldData
{
    public class GlobalPalette
    {
        public Dictionary<uint, BlockState> StateFromId { get; set; } = new Dictionary<uint, BlockState>();
        public Dictionary<BlockState, uint> IdFromState { get; set; } = new Dictionary<BlockState, uint>();

        public GlobalPalette()
        {
            var text = File.ReadAllText(@"GlobalPalette.json");
            var json = JObject.Parse(text);

            foreach(var item in json.Properties())
            {
                foreach(var state in item.Value["states"])
                {
                    var tmp = new BlockState((int)state["id"]);
                    tmp.BlockName = item.Path;
                    StateFromId[(uint)state["id"]] = tmp;
                    IdFromState[tmp] = (uint)state["id"];
                }
            }
            Console.WriteLine("Loaded {0} blocks", StateFromId.Count);
        }

        public uint GetGlobalPaletteIdFromState(BlockState state)
        {
            return IdFromState[state];
        }

        public BlockState GetStateFromGlobalPaletteId(uint id)
        {
            if (StateFromId.ContainsKey(id))
                return StateFromId[id];
            else
                return StateFromId[0];
        }
    }
}
