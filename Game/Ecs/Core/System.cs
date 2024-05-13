using Scellecs.Morpeh;

namespace TestGameServer.Game.Ecs.Core;

public abstract class System
{
    public World World { get; set; }
    
    public virtual void Dispose()
    { }

    public abstract void OnAwake();
}