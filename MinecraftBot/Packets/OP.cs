using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBot.Packets
{
    public enum OPHandshaking : byte
    {
        Handshake = 0x00 //SB
    }

    public enum OPLogin : byte
    {
        LoginStart = 0x00, //SB
        LoginSuccess = 0x02, //CB
        SetCompression = 0x03 //CB
    }

    public enum OPPlay : byte
    {
        SpawnEntity = 0x00, //CB
        SpawnLivingEntity = 0x02, //CB
        SpawnPlayer = 0x04, //CB
        ClientSettings = 0x05, //SB
        PlayerPositionAndLookCB = 0x34, //CB
        TeleportConfirm = 0x00, //SB
        PlayerPositionAndLookSB = 0x13, //SB
        ClientStatus = 0x04, //SB
        KeepAliveCB = 0x1F, //CB
        KeepAliveSB = 0x10, //SB
        ChatMessageCB = 0x0E, //CB
        ChatMessageSB = 0x03, //SB
        ChunkData = 0x20, //CB
        PlayerBlockPlacement = 0x2E, //SB
        BlockChange = 0x0B, //CB
        EntityPosition = 0x27, //CB
        EntityPositionAndRotation = 0x28 //CB
    }
}
