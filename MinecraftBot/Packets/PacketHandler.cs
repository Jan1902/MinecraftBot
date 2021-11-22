using Be.IO;
using MinecraftBot.Entities;
using MinecraftBot.Packets.CB;
using MinecraftBot.Packets.SB;
using MinecraftBot.WorldData;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MinecraftBot.Packets
{
    public class PacketHandler
    {
        private delegate void Handler(byte[] data);

        private Dictionary<OPHandshaking, Handler> handshakeHandlers = new Dictionary<OPHandshaking, Handler>();
        private Dictionary<OPLogin, Handler> loginHandlers = new Dictionary<OPLogin, Handler>();
        private Dictionary<OPPlay, Handler> playHandlers = new Dictionary<OPPlay, Handler>();

        private ProtocolState state = ProtocolState.Handshaking;

        private int chunksReceived;
        private bool firstKeepAliveReceived;

        public PacketHandler()
        {
            RegisterHandlers();
        }

        private void RegisterHandlers()
        {
            loginHandlers.Add(OPLogin.LoginSuccess, HandleLoginSuccess);
            loginHandlers.Add(OPLogin.SetCompression, HandleSetCompression);
            playHandlers.Add(OPPlay.PlayerPositionAndLookCB, HandlePlayerPositionAndLook);
            playHandlers.Add(OPPlay.KeepAliveCB, HandleKeepAlive);
            playHandlers.Add(OPPlay.ChatMessageCB, HandleChatMessage);
            playHandlers.Add(OPPlay.ChunkData, HandleChunkData);
            playHandlers.Add(OPPlay.BlockChange, HandleBlockChange);
            playHandlers.Add(OPPlay.SpawnEntity, HandleSpawnEntity);
            playHandlers.Add(OPPlay.SpawnLivingEntity, HandleSpawnLivingEntity);
            playHandlers.Add(OPPlay.SpawnPlayer, HandleSpawnPlayer);
            playHandlers.Add(OPPlay.EntityPosition, HandleEntityPosition);
            playHandlers.Add(OPPlay.EntityPositionAndRotation, HandleEntityPositionAndRotation);
        }

        public void HandlePacket(byte op, byte[] data)
        {
            switch (state)
            {
                case ProtocolState.Login:
                    if (Enum.IsDefined(typeof(OPLogin), op) && loginHandlers.ContainsKey((OPLogin)op))
                        loginHandlers[(OPLogin)op](data);
                    break;
                case ProtocolState.Handshaking:
                    if (Enum.IsDefined(typeof(OPHandshaking), op) && handshakeHandlers.ContainsKey((OPHandshaking)op))
                        handshakeHandlers[(OPHandshaking)op](data);
                    break;
                case ProtocolState.Play:
                    if (Enum.IsDefined(typeof(OPPlay), op) && playHandlers.ContainsKey((OPPlay)op))
                        playHandlers[(OPPlay)op](data);
                    break;
            }
        }

        public void SetState(ProtocolState state)
        {
            this.state = state;
        }

        private void HandleSpawnEntity(byte[] data)
        {
            var spawnEntity = new SpawnEntity(data);
            //Console.WriteLine("SpawnEntity");
            //Console.WriteLine("Entity ID: " + spawnEntity.EntityID);
            //Console.WriteLine("Type: " + spawnEntity.Type);
            //Console.WriteLine("Position: " + spawnEntity.X + "/" + spawnEntity.Y + "/" + spawnEntity.Z);
            //Console.WriteLine();
        }

        private void HandleSpawnLivingEntity(byte[] data)
        {
            var spawnLivingEntity = new SpawnLivingEntity(data);
            //Console.WriteLine("SpawnLivingEntity");
            //Console.WriteLine("Entity ID: " + spawnLivingEntity.EntityID);
            //Console.WriteLine("Type: " + spawnLivingEntity.Type);
            //Console.WriteLine("Position: " + spawnLivingEntity.X + "/" + spawnLivingEntity.Y + "/" + spawnLivingEntity.Z);
            //Console.WriteLine();
        }

        private void HandleSpawnPlayer(byte[] data)
        {
            var spawnPlayer = new SpawnPlayer(data);
            //Console.WriteLine("SpawnPlayer");
            //Console.WriteLine("Entity ID: " + spawnPlayer.EntityID);
            //Console.WriteLine("UUID: " + spawnPlayer.PlayerUUID);
            //Console.WriteLine("Position: " + spawnPlayer.X + "/" + spawnPlayer.Y + "/" + spawnPlayer.Z);
            //Console.WriteLine();
            var existingPlayer = Client.Instance.World.Entities.FirstOrDefault(e => e.EntityID == spawnPlayer.EntityID);
            if(existingPlayer != null)
            {
                existingPlayer.X = spawnPlayer.X;
                existingPlayer.Y = spawnPlayer.X;
                existingPlayer.Z = spawnPlayer.X;
                //existingPlayer.Yaw = spawnPlayer.Yaw;
                //existingPlayer.Pitch = spawnPlayer.Pitch;
            }
            else
            {
                Client.Instance.World.Entities.Add(new Player
                {
                    X = spawnPlayer.X,
                    Y = spawnPlayer.Y,
                    Z = spawnPlayer.Z,
                    //Yaw = spawnPlayer.Yaw,
                    //Pitch = spawnPlayer.Pitch,
                    EntityID = spawnPlayer.EntityID,
                    PlayerID = spawnPlayer.PlayerUUID,
                    Name = "Unknown"
                });
            }
        }

        private void HandleEntityPosition(byte[] data)
        {
            var entityPosition = new EntityPosition(data);
            //Console.WriteLine("EntityPosition");
            //Console.WriteLine("Entity ID: " + entityPosition.EntityID);
            //Console.WriteLine("DeltaX: " + entityPosition.DeltaX);
            //Console.WriteLine("DeltaY: " + entityPosition.DeltaY);
            //Console.WriteLine("DeltaZ: " + entityPosition.DeltaZ);
            var entity = Client.Instance.World.Entities.FirstOrDefault(e => e.EntityID == entityPosition.EntityID);
            if(entity != null)
            {
                entity.X += entityPosition.DeltaX / 4096;
                entity.Y += entityPosition.DeltaY / 4096;
                entity.Z += entityPosition.DeltaZ / 4096;
            }
        }

        private void HandleEntityPositionAndRotation(byte[] data)
        {
            var entityPositionAndRotation = new EntityPositionAndRotation(data);
            //Console.WriteLine("EntityPosition");
            //Console.WriteLine("Entity ID: " + entityPosition.EntityID);
            //Console.WriteLine("DeltaX: " + entityPosition.DeltaX);
            //Console.WriteLine("DeltaY: " + entityPosition.DeltaY);
            //Console.WriteLine("DeltaZ: " + entityPosition.DeltaZ);
            var entity = Client.Instance.World.Entities.FirstOrDefault(e => e.EntityID == entityPositionAndRotation.EntityID);
            if (entity != null)
            {
                entity.X += entityPositionAndRotation.DeltaX / 4096;
                entity.Y += entityPositionAndRotation.DeltaY / 4096;
                entity.Z += entityPositionAndRotation.DeltaZ / 4096;
            }
        }

        private void HandleBlockChange(byte[] data)
        {
            var blockChange = new BlockChange(data);
            Client.Instance.World.SetBlock((int)blockChange.Location.X, (int)blockChange.Location.Y, (int)blockChange.Location.Z, blockChange.BlockID);
            Console.WriteLine("BlockChange");
            Console.WriteLine("Position: " + blockChange.Location.X + "/" + blockChange.Location.Y + "/" + blockChange.Location.Z);
            Console.WriteLine("ID: " + blockChange.BlockID);
            Console.WriteLine();
        }

        private void HandleLoginSuccess(byte[] data)
        {
            var loginSuccess = new LoginSuccess(data);
            Console.WriteLine("LoginSuccess");
            Console.WriteLine("UUID: " + loginSuccess.UUID);
            Console.WriteLine("Username: " + loginSuccess.Username);
            Console.WriteLine();

            state = ProtocolState.Play;

            var clientSettings = new ClientSettings();
            clientSettings.Locale = "en_GB";
            clientSettings.ViewDistance = 10;
            clientSettings.ChatMode = 0;
            clientSettings.ChatColors = false;
            clientSettings.DisplayedSkinParts = 0;
            clientSettings.MainHand = 1;
            clientSettings.Create();
            Client.Instance.Connection.SendPacket(clientSettings);
        }

        private void HandleSetCompression(byte[] data)
        {
            var setCompression = new SetCompression(data);
            Console.WriteLine("SetCompression");
            Console.WriteLine("Threshold: " + setCompression.Threshold);
            Console.WriteLine();
            Client.Instance.Connection.EnableCompression(setCompression.Threshold);
        }

        private void HandlePlayerPositionAndLook(byte[] data)
        {
            var playerPositionAndlook = new PlayerPositionAndLookCB(data);
            Console.WriteLine("PlayerPositionAndLook");
            Console.WriteLine("X/Y/Z: " + playerPositionAndlook.X + "/" + playerPositionAndlook.Y + "/" + playerPositionAndlook.Z);
            Console.WriteLine("Yaw/Pitch: " + playerPositionAndlook.Yaw + "/" + playerPositionAndlook.Pitch);
            Console.WriteLine("TeleportID: " + playerPositionAndlook.TeleportID);
            Console.WriteLine("RelativeFlags: " + playerPositionAndlook.Flags);
            Console.WriteLine();
            if (playerPositionAndlook.Flags != 0)
                return;

            Client.Instance.LocalPlayer.X = playerPositionAndlook.X;
            Client.Instance.LocalPlayer.Y = playerPositionAndlook.Y;
            Client.Instance.LocalPlayer.Z = playerPositionAndlook.Z;
            Client.Instance.LocalPlayer.Yaw = playerPositionAndlook.Yaw;
            Client.Instance.LocalPlayer.Pitch = playerPositionAndlook.Pitch;
            Client.Instance.EnablePositionUpdates();

            var teleportConfirm = new TeleportConfirm();
            teleportConfirm.TeleportID = playerPositionAndlook.TeleportID;
            teleportConfirm.Create();
            Client.Instance.Connection.SendPacket(teleportConfirm);

            if (playerPositionAndlook.TeleportID == 1)
            {
                var clientStatus = new ClientStatus();
                clientStatus.ActionID = 0;
                clientStatus.Create();
                Client.Instance.Connection.SendPacket(clientStatus);
            }
        }

        private void HandleKeepAlive(byte[] data)
        {
            var keepAlive = new KeepAliveCB(data);
            Console.WriteLine("KeepAlive");
            Console.WriteLine("KeepAliveID: " + keepAlive.KeepAliveID);
            Console.WriteLine();

            var keepAliveSB = new KeepAliveSB();
            keepAliveSB.KeepAliveID = keepAlive.KeepAliveID;
            keepAliveSB.Create();
            Client.Instance.Connection.SendPacket(keepAliveSB);

            if(!firstKeepAliveReceived)
            {
                firstKeepAliveReceived = true;
                Client.Instance.SendChatMessage("Done receiving Map Data");
            }
        }

        private void HandleChatMessage(byte[] data)
        {
            var chatMessage = new ChatMessage(data);
            JObject json = JObject.Parse(chatMessage.JsonData);

            if ((string)json["translate"] == "commands.message.display.incoming")
            {
                var player = (string)json["with"][0]["insertion"];
                var message = (string)json["with"][1]["text"];
                if (player == "Bot")
                    return;

                Console.WriteLine("ChatMessage");
                Console.WriteLine("Position: " + chatMessage.Position);
                Console.WriteLine(player + ": " + message);
                Console.WriteLine();
                Client.Instance.HandleChatCommand(player, message, true);
            }
            else if ((string)json["translate"] == null)
            {
                var text = (string)json["extra"][0]["text"];
                if (!text.Contains("<"))
                    return;
                var player = text.Substring(1).Substring(0, text.IndexOf('>') - 1);
                var message = text.Substring(text.IndexOf('>') + 2);
                if (player == "Bot")
                    return;

                Console.WriteLine("ChatMessage");
                Console.WriteLine("Position: " + chatMessage.Position);
                Console.WriteLine(player + ": " + message);
                Console.WriteLine();
                Client.Instance.HandleChatCommand(player, message, false);
            }
        }

        private void HandleChunkData(byte[] data)
        {
            ParseChunkData(data);
        }

        private void ParseChunkData(byte[] data)
        {
            var chunkData = new ChunkData(data);

            if(Math.Floor((double)Client.Instance.LocalPlayer.X / 16) == chunkData.ChunkX && Math.Floor((double)Client.Instance.LocalPlayer.Z / 16) == chunkData.ChunkZ)
            {
                if(Client.Instance.LocalPlayer.X != 0 || Client.Instance.LocalPlayer.Z != 0 || Client.Instance.LocalPlayer.Y != 0)
                {
                    if (!Client.Instance.LocalPlayer.GravityThread.IsAlive)
                        Client.Instance.LocalPlayer.GravityThread.Start();
                }
            }

            Chunk chunk;
            if (chunkData.FullChunk)
                chunk = new Chunk(chunkData.ChunkX, chunkData.ChunkZ);
            else
                chunk = Client.Instance.World.Chunks[new Types.Vec2I(chunkData.ChunkX, chunkData.ChunkZ)];

            using (var stream = new MemoryStream(chunkData.Data))
            {
                using (var reader = new BeBinaryReader(stream))
                {
                    for (int sectionY = 0; sectionY < 16; sectionY++)
                    {
                        if ((chunkData.PrimaryBitMask & (1 << sectionY)) != 0)
                        {
                            var blockCount = reader.ReadInt16();
                            byte bitsPerBlock = reader.ReadByte();
                            IPalette palette = WorldUtils.ChoosePalette(bitsPerBlock, Client.Instance);
                            palette.Read(stream);

                            uint individualValueMask = (uint)((1 << palette.GetBitsPerBlock()) - 1);

                            int dataArrayLength = VarInt.ReadVarInt(stream);
                            ulong[] dataArray = new ulong[dataArrayLength];
                            for (int i = 0; i < dataArrayLength; i++) //for some reason, the big endian reader doesnt work in this situation
                            {
                                byte[] tmp = new byte[8];
                                tmp = reader.ReadBytes(8);
                                Array.Reverse(tmp);
                                dataArray[i] = BitConverter.ToUInt64(tmp, 0);
                            }

                            ChunkSection section = new ChunkSection();

                            for (int y = 0; y < 16; y++)
                            {
                                for (int z = 0; z < 16; z++)
                                {
                                    for (int x = 0; x < 16; x++)
                                    {
                                        int blockNumber = (((y * 16) + z) * 16) + x;
                                        int startLong = blockNumber * palette.GetBitsPerBlock() / 64;
                                        int startOffset = blockNumber * palette.GetBitsPerBlock() % 64;
                                        int endLong = ((blockNumber + 1) * palette.GetBitsPerBlock() - 1) / 64;

                                        uint dta;
                                        if (startLong == endLong)
                                        {
                                            dta = (uint)(dataArray[startLong] >> startOffset);
                                        }
                                        else
                                        {
                                            int endOffset = 64 - startOffset;
                                            dta = (uint)(dataArray[startLong] >> startOffset | dataArray[endLong] << endOffset);
                                        }
                                        dta &= individualValueMask;

                                        section.BlockStates[x, y, z] = palette.StateForId(dta);
                                    }
                                }
                            }

                            //for (int y = 0; y < 16; y++)
                            //{
                            //    for (int z = 0; z < 16; z++)
                            //    {
                            //        for (int x = 0; x < 16; x += 2)
                            //        {
                            //            byte value = reader.ReadByte();

                            //            section.BlockLights[x, y, z] = value & 0xF;
                            //            section.BlockLights[x + 1, y, z] = (value >> 4) & 0xF;
                            //        }
                            //    }
                            //}

                            //if (true) //currentDimension.HasSkylight() expected to always be true for now (keep bot in overworld!)
                            //{
                            //    for (int y = 0; y < 16; y++)
                            //    {
                            //        for (int z = 0; z < 16; z++)
                            //        {
                            //            for (int x = 0; x < 16; x += 2)
                            //            {
                            //                byte value = reader.ReadByte();

                            //                section.SkyLights[x, y, z] = value & 0xF;
                            //                section.SkyLights[x + 1, y, z] = (value >> 4) & 0xF;
                            //            }
                            //        }
                            //    }
                            //}

                            chunk.Sections[sectionY] = section;
                        }
                    }

                    //if(chunkData.FullChunk)
                    //{
                    //    for (int z = 0; z < 16; z++)
                    //    {
                    //        for (int x = 0; x < 16; x++)
                    //        {
                    //            chunk.Biomes[x, z] = reader.ReadInt32();
                    //        }
                    //    }
                    //}
                }
            }
            if (chunkData.FullChunk)
            {
                lock (Client.Instance.World.ChunkLocker)
                {
                    Client.Instance.World.Chunks[new Types.Vec2I(chunkData.ChunkX, chunkData.ChunkZ)] = (chunk);
                }
            }
            else
            {
                Client.Instance.World.Chunks[new Types.Vec2I(chunkData.ChunkX, chunkData.ChunkZ)] = chunk;
            }
            chunksReceived++;
            if(chunksReceived == 310)
            {
                Console.WriteLine("Done receiving World Data");
                var startTime = DateTime.Now;
                for (int i = 0; i < 65000; i++)
                {
                    Client.Instance.World.GetBlock((int)Client.Instance.LocalPlayer.X + 5, (int)Client.Instance.LocalPlayer.Y + 5, (int)Client.Instance.LocalPlayer.Z + 5);
                }
                Console.WriteLine((DateTime.Now - startTime).TotalMilliseconds);
                Console.WriteLine();
            }
        }
    }
}
