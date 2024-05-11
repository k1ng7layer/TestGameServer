using Newtonsoft.Json;

namespace TestGameServer.Game.Config.Server.Impl;

public class InMemoryConfiguration : IConfiguration
{
    private readonly Dictionary<string, string> _configurationTable = new();
        
    public void Add(string key, string value)
    {
        if(!_configurationTable.ContainsKey(key))
            _configurationTable.Add(key, value);
        else
        {
            throw new InvalidOperationException(
                $"[{nameof(InMemoryConfiguration)}] configuration record with same key {key} already exists");
        }
    }

    public void AddJsonFile(string file)
    {
        var jsonString = File.ReadAllText(@$"D:\GITRepository\GameServer\GameServer\Server\Core\Configuration\FileConfiguration\{file}");
        var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);

        foreach (var resultKey in result.Keys)
        {
            if(!_configurationTable.ContainsKey(resultKey))
                _configurationTable.Add(resultKey, result[resultKey]);
            else
            {
                _configurationTable[resultKey] = result[resultKey];
            }
        }
    }

    public string Get(string key)
    {
        if (_configurationTable.TryGetValue(key, out var value))
            return value;
            
        throw new InvalidOperationException(
            $"[{nameof(InMemoryConfiguration)}] could not find configuration with key {key}");
    }

    public string this[string key]
    {
        get
        {
            if (_configurationTable.TryGetValue(key, out var value))
                return value;
                
            throw new InvalidOperationException(
                $"[{nameof(InMemoryConfiguration)}] could not find configuration with key {key}");
        }
    }
}