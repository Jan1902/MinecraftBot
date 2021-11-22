using CaptiveAire.EndianUtil.Conversion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets
{
    public class PacketBase
    {
        public List<byte> bytes = new List<byte>();

        public PacketBase(OPHandshaking op)
        {
            AddVarInt((byte)op);
        }

        public PacketBase(OPLogin op)
        {
            AddVarInt((byte)op);
        }

        public PacketBase(OPPlay op)
        {
            AddVarInt((byte)op);
        }

        public PacketBase(byte[] data)
        {
            bytes = data.ToList();
        }

        public void AddVarInt(int value)
        {
            bytes.AddRange(VarInt.VarIntToBytes(value));
        }

        public int GetVarInt()
        {
            var value = VarInt.ReadVarInt(new MemoryStream(bytes.ToArray()));
            bytes.RemoveRange(0, VarInt.VarIntToBytes(value).Length);
            return value;
        }

        public void AddBytes(byte[] value)
        {
            bytes.AddRange(value);
        }

        public byte[] GetBytes(int count)
        {
            var value = bytes.GetRange(0, count).ToArray();
            bytes.RemoveRange(0, count);
            return value;
        }

        public void AddUShort(ushort value)
        {
            bytes.AddRange(EndianBitConverter.Big.GetBytes(value));
        }

        public ushort GetUShort()
        {
            var value = EndianBitConverter.Big.ToUInt16(bytes.ToArray(), 0);
            bytes.RemoveRange(0, sizeof(ushort));
            return value;
        }

        public void AddShort(short value)
        {
            bytes.AddRange(EndianBitConverter.Big.GetBytes(value));
        }

        public short GetShort()
        {
            var value = EndianBitConverter.Big.ToInt16(bytes.ToArray(), 0);
            bytes.RemoveRange(0, sizeof(short));
            return value;
        }

        public void AddString(string value)
        {
            AddVarInt(Encoding.UTF8.GetBytes(value).Length);
            bytes.AddRange(Encoding.UTF8.GetBytes(value));
        }

        public string GetString()
        {
            var length = GetVarInt();
            var value = Encoding.UTF8.GetString(bytes.GetRange(0, length).ToArray());
            bytes.RemoveRange(0, length);
            return value;
        }

        public string GetString(int length)
        {
            var value = Encoding.UTF8.GetString(bytes.GetRange(0, length).ToArray());
            bytes.RemoveRange(0, length);
            return value;
        }

        public void AddByte(byte value)
        {
            bytes.Add(value);
        }

        public byte GetByte()
        {
            var value = bytes[0];
            bytes.RemoveRange(0, 1);
            return value;
        }

        public bool GetBool()
        {
            var value = bytes[0] == 1 ? true : false;
            bytes.RemoveRange(0, 1);
            return value;
        }

        public void AddBool(bool value)
        {
            bytes.Add(value ? (byte)1 : (byte)0);
        }

        public void AddDouble(double value)
        {
            bytes.AddRange(EndianBitConverter.Big.GetBytes(value));
        }

        public double GetDouble()
        {
            var value = EndianBitConverter.Big.ToDouble(bytes.ToArray(), 0);
            bytes.RemoveRange(0, sizeof(double));
            return value;
        }

        public float GetFloat()
        {
            var value = EndianBitConverter.Big.ToSingle(bytes.ToArray(), 0);
            bytes.RemoveRange(0, sizeof(float));
            return value;
        }

        public void AddFloat(float value)
        {
            bytes.AddRange(EndianBitConverter.Big.GetBytes(value));
        }

        public void AddLong(long value)
        {
            bytes.AddRange(EndianBitConverter.Big.GetBytes(value));
        }

        public long GetLong()
        {
            var value = EndianBitConverter.Big.ToInt64(bytes.ToArray(), 0);
            bytes.RemoveRange(0, sizeof(long));
            return value;
        }

        public ulong GetULong()
        {
            var value = EndianBitConverter.Big.ToUInt64(bytes.ToArray(), 0);
            bytes.RemoveRange(0, sizeof(ulong));
            return value;
        }

        public int GetInt()
        {
            var value = EndianBitConverter.Big.ToInt32(bytes.ToArray(), 0);
            bytes.RemoveRange(0, sizeof(int));
            return value;
        }

        public void AddInt(int value)
        {
            bytes.AddRange(EndianBitConverter.Big.GetBytes(value));
        }

        public byte[] GetLengthLessBytes()
        {
            return bytes.ToArray();
        }
    }
}
