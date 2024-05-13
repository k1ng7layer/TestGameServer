using System.Numerics;
using TestGameServer.Game.Utils;
using TestGameServer.Network.Utils;

namespace TestGameServer.Messaging.Helpers;

public static class MessageHelper
{
    public static byte[] SpawnMinionMessage(ETeam team, Vector3 position, ushort id)
    {
        var byteWriter = new ByteWriter();
        
        byteWriter.AddUshort((ushort)ENetworkMessageType.Custom);
        byteWriter.AddUshort((ushort)ECustomMessageType.Spawn);
        byteWriter.AddInt32(15);
        byteWriter.AddByte((byte)team);
        byteWriter.AddVector3(position);
        byteWriter.AddUshort(id);

        return byteWriter.Data;
    }
    
}