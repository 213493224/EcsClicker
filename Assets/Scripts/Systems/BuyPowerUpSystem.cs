using Components;
using Core;
using Leopotam.EcsLite;
using Services;

namespace Systems
{
    internal sealed class BuyPowerUpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IGameFactory _factory;
        private readonly IStaticDataService _staticDataService;
        private readonly EcsWorld _world;
        private EcsFilter _businessFilter;
        private EcsPool<BusinessCard> _businessPool;
        private EcsFilter _eventFilter;
        private EcsPool<BuyPowerUpEvent> _eventPool;
        private EcsPool<PowerUp> _powerUpPool;
        private EcsPool<Unlocked> _unlockedPool;
        private EcsPool<UpdateComponentViewEvent> _updateComponentViewPool;

        public BuyPowerUpSystem(EcsWorld world, IGameFactory factory, IStaticDataService staticDataService)
        {
            _world = world;
            _factory = factory;
            _staticDataService = staticDataService;
        }

        public void Init(IEcsSystems systems)
        {
            _eventFilter = _world.Filter<PowerUp>().Inc<BuyPowerUpEvent>().End();
            _businessFilter = _world.Filter<BusinessCard>().End();
            _powerUpPool = _world.GetPool<PowerUp>();
            _businessPool = _world.GetPool<BusinessCard>();
            _updateComponentViewPool = _world.GetPool<UpdateComponentViewEvent>();
            _unlockedPool = _world.GetPool<Unlocked>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var index in _eventFilter)
            {
                ref var powerUp = ref _powerUpPool.Get(index);

                var balance = _factory.Balance;

                if (!IsBusinessBought(powerUp.BusinessId))
                {
                    continue; 
                }

                if(!Extensions.HasEnoughBalance(balance, powerUp.Price))
                {
                    continue;
                };

                balance.SpendBalance(powerUp.Price);

                powerUp.Unlocked = true;
                _unlockedPool.Add(index);

                PowerUpBusinessIncome(ref powerUp);
            }
        }

        private bool IsBusinessBought(int businessId)
        {
            foreach (var index in _businessFilter)
            {
                ref var businessCard = ref _businessPool.Get(index);
                if (businessCard.EntityId == businessId)
                {
                    return businessCard.Level > 0;
                }
            }
            
            return false;
        }

        private void PowerUpBusinessIncome(ref PowerUp powerUp)
        {
            foreach (var index in _businessFilter)
            {
                ref var businessCard = ref _businessPool.Get(index);
                if (powerUp.BusinessTypeId == businessCard.BusinessTypeId)
                {
                    businessCard.Income = businessCard.CalculateBusinessIncome(_staticDataService);

                    _updateComponentViewPool.SendUpdateComponentEvent(businessCard.EntityId);
                }
            }
        }
    }
}