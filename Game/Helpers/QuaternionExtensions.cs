using System.Numerics;

namespace TestGameServer.Game.Helpers;

public static class QuaternionExtensions
{
    public static Vector3 Eulers(this Quaternion quaternion)
    {
        var result = new Vector3();
        
        result.Y = (float)Math.Atan2(2f * quaternion.X * quaternion.W + 2f * quaternion.Y * quaternion.Z, 1 - 2f * (MathF.Sqrt(quaternion.Z)  + quaternion.W));     // Yaw 
        result.X = (float)Math.Asin(2f * ( quaternion.X * quaternion.Z - quaternion.W * quaternion.Y ) );                             // Pitch 
        result.Z = (float)Math.Atan2(2f * quaternion.X * quaternion.Y + 2f * quaternion.Z * quaternion.W, 1 - 2f * (MathF.Sqrt(quaternion.Y) + MathF.Sqrt(quaternion.Z)));

        return result; // Roll
    }
}