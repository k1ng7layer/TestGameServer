using Scellecs.Morpeh;
using TestGameServer.Game.Ecs.Systems;
using TestGameServer.MessageDispatcher.Impl;

namespace TestGameServer.Game.Ecs.Core;

public class EcsInstaller : IDisposable
{
    private World _world;
    
    public void Initialize()
    {
        _world = World.Create();
        var inputSystems = _world.CreateSystemsGroup();
        var dispatcher = new NetworkMessageDispatcher();
        var inputSystem = new PlayerInputSystem(dispatcher);
        inputSystems.AddInitializer(inputSystem);
        _world.AddSystemsGroup(0, inputSystems);
    }

    public void Tick(float deltaTime)
    {
        _world.Update(deltaTime);
        _world.FixedUpdate(deltaTime);
        _world.LateUpdate(deltaTime);
    }

    public void Dispose()
    {
        _world.Dispose();
    }
}