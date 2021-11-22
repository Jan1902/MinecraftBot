using Ionic.Zlib;
using MinecraftBot.Actions;
using MinecraftBot.Entities;
using MinecraftBot.Packets;
using MinecraftBot.Packets.SB;
using MinecraftBot.Pathfinding;
using MinecraftBot.WorldData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MinecraftBot
{
    public class Client
    {
        public static Client Instance;

        public LocalPlayer LocalPlayer { get; private set; }
        public World World { get; private set; }
        public GlobalPalette GlobalPalette { get; private set; }

        private bool updatePosition;
        private bool spinning;

        private bool stopRequested;

        public PacketHandler PacketHandler { get; private set; }
        public Connection Connection { get; private set; }

        public Queue<IAction> ActionPipeline { get; set; } = new Queue<IAction>();

        public Client(int port, string address)
        {
            Instance = this;
            GlobalPalette = new GlobalPalette();
            World = new World();
            LocalPlayer = new LocalPlayer();
            PacketHandler = new PacketHandler();
            Connection = new Connection();

            new Thread(UpdatePositionThread).Start();
            new Thread(ExecuteActions).Start();

            while (!stopRequested) { }
        }

        private void ExecuteActions()
        {
            while(!stopRequested)
            {
                if(ActionPipeline.Count > 0)
                {
                    var action = ActionPipeline.Dequeue();

                    action.Execute();

                    Thread.Sleep(50);
                }
            }
        }

        public void SendChatMessage(string message)
        {
            var chatMessage = new ChatMessageSB();
            chatMessage.Message = message;
            chatMessage.Create();
            Connection.SendPacket(chatMessage);
        }

        private void UpdatePositionThread()
        {
            while (!stopRequested)
            {
                if (updatePosition)
                {
                    var playerPositionAndLook = new PlayerPositionAndLookSB();
                    playerPositionAndLook.X = LocalPlayer.X;
                    playerPositionAndLook.Y = LocalPlayer.Y;
                    playerPositionAndLook.Z = LocalPlayer.Z;
                    playerPositionAndLook.Yaw = LocalPlayer.Yaw;
                    playerPositionAndLook.Pitch = LocalPlayer.Pitch;
                    playerPositionAndLook.OnGround = LocalPlayer.OnGround;
                    playerPositionAndLook.Create();
                    Connection.SendPacket(playerPositionAndLook);
                }
                if (spinning)
                    LocalPlayer.Yaw += 5;
                Thread.Sleep(50);
            }
        }

        public void HandleChatCommand(string player, string message, bool whisper)
        {
            if (whisper)
                message = "!bot " + message;

            var tokens = message.ToLower().Split(' ');
            if (tokens[0] != "!bot" && !whisper)
                return;

            if (tokens.Length == 1)
                return;

            switch (tokens[1])
            {
                case "spin":
                    spinning = !spinning;
                    break;
                case "block":
                    if (tokens.Length != 5)
                        return;

                    var blockX = int.Parse(tokens[2]);
                    var blockY = int.Parse(tokens[3]);
                    var blockZ = int.Parse(tokens[4]);
                    SendChatMessage(string.Format("Block at {0}/{1}/{2} is {3}", blockX, blockY, blockZ, World.GetBlock(blockX, blockY, blockZ).BlockName));
                    break;
                case "place":
                    if (tokens.Length != 5)
                        return;

                    var placeX = int.Parse(tokens[2]);
                    var placeY = int.Parse(tokens[3]);
                    var placeZ = int.Parse(tokens[4]);

                    PlaceBlock(placeX, placeY, placeZ);
                    SendChatMessage(string.Format("Placed Block at {0}/{1}/{2}", placeX, placeY, placeZ));
                    break;
                case "goto":
                    if (tokens.Length != 5)
                        return;

                    var pathX = Math.Round(double.Parse(tokens[2]));
                    var pathY = Math.Round(double.Parse(tokens[3]));
                    var pathZ = Math.Round(double.Parse(tokens[4]));

                    ActionPipeline.Enqueue(new PathfindAction((int)pathX, (int)pathY, (int)pathZ));
                    break;
                case "dc":
                case "stop":
                    stopRequested = true;
                    break;
                case "jump":
                    LocalPlayer.Jump();
                    break;
                case "chunk":
                    var chunkX = int.Parse(tokens[2]);
                    var chunkY = int.Parse(tokens[3]);
                    var chunkZ = int.Parse(tokens[4]);
                    var layer = int.Parse(tokens[5]);

                    var section = World.Chunks[new Types.Vec2I(chunkX, chunkZ)].Sections[chunkY];
                    var output = "";
                    for (int z = 0; z < 16; z++)
                    {
                        for (int x = 0; x < 16; x++)
                        {
                            output += (section.BlockStates[x, layer, z].BlockName.Split(':').Last() == "air" ? " " : section.BlockStates[x, layer, z].BlockName.Split(':').Last().Substring(0, 1)) + " ";
                        }
                        output += "\n";
                    }
                    Console.WriteLine(output);
                    break;
                case "come":
                    var master = World.Entities.FirstOrDefault(e => e is Player);
                    if (master == null)
                    {
                        SendChatMessage("Master Player not found");
                        return;
                    }

                    var goalX = Math.Round(master.X - .5);
                    var goalY = Math.Round(master.Y);
                    var goalZ = Math.Round(master.Z - .5);

                    ActionPipeline.Enqueue(new PathfindAction((int)goalX, (int)goalY, (int)goalZ));

                    break;
                default:
                    if (whisper)
                        SendChatMessage("/w " + player + " What?");
                    else
                        SendChatMessage("What?");
                    break;
            }
        }

        public void PlaceBlock(int x, int y, int z)
        {
            LocalPlayer.LookAt(x, y, z);
            var playerBlockPlacement = new PlayerBlockPlacement();
            playerBlockPlacement.Location = Position.PositionToLong(x, y, z);
            playerBlockPlacement.Face = 0;
            playerBlockPlacement.Hand = 0;
            playerBlockPlacement.CursorPositionX = .5f;
            playerBlockPlacement.CursorPositionY = .5f;
            playerBlockPlacement.CursorPositionZ = .5f;
            playerBlockPlacement.InsideBlock = false;
            playerBlockPlacement.Create();
            Connection.SendPacket(playerBlockPlacement);
        }

        public void EnablePositionUpdates()
        {
            updatePosition = true;
        }
    }
}
