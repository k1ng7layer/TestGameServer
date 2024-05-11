﻿namespace TestGameServer.Game.Config.Server;

public interface IConfiguration
{
    void Add(string key, string value);
    void AddJsonFile(string file);
    string Get(string key);
    string this [string key] { get; }
}