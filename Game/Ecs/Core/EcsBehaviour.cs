using Scellecs.Morpeh;

namespace TestGameServer.Game.Ecs.Core;

public class EcsBehaviour : IDisposable
{
    private readonly EcsSystemsInstaller _ecsSystemsInstaller;
    private World _world;
    private bool _started;

    public EcsBehaviour(EcsSystemsInstaller ecsSystemsInstaller)
    {
        _ecsSystemsInstaller = ecsSystemsInstaller;
    }
    
    public void Initialize()
    {
        _world = World.Create();
        var systemGroups = _ecsSystemsInstaller.CreateSystemGroup(_world);
        
        for (var i = 0; i < systemGroups.Count; i++)
        {
            var systemsGroup = systemGroups[i];
            
            _world.AddSystemsGroup(i, systemsGroup);
        }
    }

    public void Enable()
    {
        _started = true;
    }

    public void Tick(float deltaTime)
    {
        if (!_started)
            return;
        
        _world.Update(deltaTime);
        _world.FixedUpdate(deltaTime);
        _world.LateUpdate(deltaTime);
        _world.CleanupUpdate(deltaTime);
    }

    public void Dispose()
    {
        _world.Dispose();
    }
}