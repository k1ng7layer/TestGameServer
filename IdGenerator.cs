﻿namespace TestGameServer;

public static class IdGenerator
{
    private static ushort _next;
    
    public static ushort Next()
    {
        return _next++;
    }
}