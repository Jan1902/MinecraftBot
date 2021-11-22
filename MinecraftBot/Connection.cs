using Ionic.Zlib;
using Microsoft.IO;
using MinecraftBot.Packets;
using MinecraftBot.Packets.SB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot
{
    public class Connection
    {
        private Socket socket;
        private byte[] buf = new byte[4096];
        private int queuePos;

        private byte[] DataQueue = new byte[32768];

        private bool compressionEnabled;
        private int compressionThreshold;

        private string address = "127.0.0.1";
        private int port = 25565;

        private readonly RecyclableMemoryStreamManager manager = new RecyclableMemoryStreamManager();

        public Connection()
        {
            socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(address, port);

            socket.BeginReceive(buf, 0, buf.Length, 0, new AsyncCallback(ReceiveCallback), null);
            SendStartPackets();
        }

        private void SendStartPackets()
        {
            var handshake = new Handshake();
            handshake.ProtocolVersion = 753;
            handshake.ServerAddress = "127.0.0.1";
            handshake.ServerPort = 25565;
            handshake.NextState = (int)ProtocolState.Login;
            handshake.Create();
            SendPacket(handshake);

            Client.Instance.PacketHandler.SetState(ProtocolState.Login);

            var loginStart = new LoginStart();
            loginStart.Username = "Bot";
            loginStart.Create();
            SendPacket(loginStart);
        }

        public void EnableCompression(int threshold)
        {
            compressionEnabled = true;
            compressionThreshold = threshold;
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            int read = socket.EndReceive(ar);
            if (read > 0)
            {
                Array.Copy(buf, 0, DataQueue, queuePos, read);
                queuePos += read;
                CheckForFullPackets();
            }
            else
            {
                Console.WriteLine("Disconnected");
                Console.ReadLine();
                Environment.Exit(0);
            }

            socket.BeginReceive(buf, 0, buf.Length, 0, new AsyncCallback(ReceiveCallback), null);
        }

        private void CheckForFullPackets()
        {
            while (queuePos > 0)
            {
                using (var ms = manager.GetStream(DataQueue))
                {
                    if (compressionEnabled)
                    {
                        var packetLength = VarInt.ReadVarInt(ms);
                        var totalLength = VarInt.VarIntToBytes(packetLength).Length + packetLength;

                        if (queuePos >= totalLength)
                        {
                            var dataLength = VarInt.ReadVarInt(ms);
                            if (dataLength == 0)
                            {
                                var op = VarInt.ReadVarInt(ms);
                                var data = new byte[packetLength - VarInt.VarIntToBytes(dataLength).Length - VarInt.VarIntToBytes(op).Length];
                                ms.Read(data, 0, data.Length);

                                Client.Instance.PacketHandler.HandlePacket((byte)op, data);
                            }
                            else
                            {
                                var compressedOpAndData = new byte[packetLength - VarInt.VarIntToBytes(dataLength).Length];
                                ms.Read(compressedOpAndData, 0, compressedOpAndData.Length);
                                using (var zlib = new ZlibStream(new MemoryStream(compressedOpAndData), CompressionMode.Decompress))
                                {
                                    var uncompressedOpAndData = new byte[dataLength];
                                    zlib.Read(uncompressedOpAndData, 0, uncompressedOpAndData.Length);
                                    using (var stream = new MemoryStream(uncompressedOpAndData))
                                    {
                                        var op = VarInt.ReadVarInt(stream);
                                        var data = new byte[dataLength];
                                        stream.Read(data, 0, data.Length);

                                        Client.Instance.PacketHandler.HandlePacket((byte)op, data);
                                    }
                                }
                            }
                            Array.Copy(DataQueue, totalLength, DataQueue, 0, queuePos - totalLength);
                            queuePos -= totalLength;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        var length = VarInt.ReadVarInt(ms);
                        var totalLength = VarInt.VarIntToBytes(length).Length + length;

                        if (queuePos >= totalLength)
                        {
                            var op = VarInt.ReadVarInt(ms);
                            var data = new byte[length];
                            ms.Read(data, 0, data.Length);

                            Client.Instance.PacketHandler.HandlePacket((byte)op, data);

                            Array.Copy(DataQueue, totalLength, DataQueue, 0, queuePos - totalLength);
                            queuePos -= totalLength;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        public void SendPacket(PacketBase packet) //TODO: IMPLEMENT COMPRESSION IF NEEDED
        {
            var data = new List<byte>();
            if (compressionEnabled)
            {
                data.AddRange(VarInt.VarIntToBytes(VarInt.VarIntToBytes(0).Length + packet.GetLengthLessBytes().Length));
                data.AddRange(VarInt.VarIntToBytes(0));
                data.AddRange(packet.GetLengthLessBytes());
            }
            else
            {
                data.AddRange(VarInt.VarIntToBytes(packet.GetLengthLessBytes().Length));
                data.AddRange(packet.GetLengthLessBytes());
            }
            socket.Send(data.ToArray());
        }
    }
}
