using Data;
using Leopotam.EcsLite;
using Services;
using UnityComponents;

namespace Systems
{
    internal class InitGameWorldSystem : IEcsInitSystem
    {
        private readonly IGameFactory _factory;

        public InitGameWorldSystem(IGameFactory factory)
        {
            _factory = factory;
        }

        public void Init(IEcsSystems systems)
        {
            _factory.CreateBalance();
            _factory.CreateBusinessCards();
        }
    }
}