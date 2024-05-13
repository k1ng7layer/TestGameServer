using Scellecs.Morpeh;
using TestGameServer.Game.Utils;

namespace TestGameServer.Game.Ecs.Components;

public struct GameStateComponent : IComponent
{
    public EGameState Value;
}