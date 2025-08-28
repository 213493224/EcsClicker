using Components;
using Leopotam.EcsLite;
using UnityComponents;

namespace Systems {
    public class UiClickEventSystem : IEcsRunSystem, IEcsInitSystem {
        private EcsWorld _world;

        private EcsFilter _businessCardFilter;
        private EcsFilter _powerUpFilter;

        private EcsPool<BusinessCard> _businessCardPool;
        private EcsPool<PowerUp> _powerUpPool;
        private EcsPool<Clicked> _clickedPool;
        private EcsPool<BuyLevelUpEvent> _buyLevelUpEventPool;
        private EcsPool<BuyPowerUpEvent> _buyPowerUpEventPool;

        public void Init(IEcsSystems systems) {
            _world = systems.GetWorld();

            _businessCardFilter = _world.Filter<BusinessCard>().End();
            _powerUpFilter = _world.Filter<PowerUp>().End();

            _businessCardPool = _world.GetPool<BusinessCard>();
            _powerUpPool = _world.GetPool<PowerUp>();
            _clickedPool = _world.GetPool<Clicked>();
            _buyLevelUpEventPool = _world.GetPool<BuyLevelUpEvent>();
            _buyPowerUpEventPool = _world.GetPool<BuyPowerUpEvent>();
        }

        public void Run(IEcsSystems systems) {
            
        }

        public void OnBusinessCardClicked(BusinessCardView businessCardView) {
            foreach (var index in _businessCardFilter) {
                ref var entity = ref _businessCardPool.Get(index);
                if (entity.BusinessTypeId == businessCardView.Id) {
                    _clickedPool.Add(index);

                    ref var evt = ref _buyLevelUpEventPool.Add(index);
                    evt.Id = entity.BusinessTypeId;
                }
            }
        }

        public void OnPowerUpClicked(PowerUpView powerUpView) {
            foreach (var index in _powerUpFilter) {
                ref var entity = ref _powerUpPool.Get(index);
                if (entity.Id == powerUpView.Id && !entity.Unlocked) {
                    _clickedPool.Add(index);

                    ref var evt = ref _buyPowerUpEventPool.Add(index);
                    evt.Id = entity.Id;
                }
            }
        }
    }
}
