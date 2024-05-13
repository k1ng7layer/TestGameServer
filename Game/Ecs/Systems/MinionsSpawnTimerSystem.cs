using Scellecs.Morpeh;
using TestGameServer.Game.Ecs.Components;
using TestGameServer.Game.Ecs.Core;
using TestGameServer.Game.Utils;

namespace TestGameServer.Game.Ecs.Systems;

public class MinionsSpawnTimerSystem : UpdateSystem
{
    private Filter _filter;
    
    public override void OnAwake()
    {
        _filter = World.Filter.With<SpawnTimerComponent>().Build();
    }
    
    public override void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter)
        {
            ref var timer = ref entity.GetComponent<SpawnTimerComponent>();
            
            timer.Value -= deltaTime;
            Console.WriteLine($"timer.Value");
            if (timer.Value <= 0f)
            {
                World.CreateEntity().SetComponent(new SpawnMinionsComponent{Team = ETeam.Blue});
                World.CreateEntity().SetComponent(new SpawnMinionsComponent{Team = ETeam.Red});
                entity.RemoveComponent<SpawnTimerComponent>();
                World.Commit();
            }

           
        }
    }
}