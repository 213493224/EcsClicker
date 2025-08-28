using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using Leopotam.EcsLite;
using Services;
using UnityComponents;
using UnityEngine;

namespace Systems
{
    public sealed class UpdateSliderSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private EcsFilter _businessCardFilter;
        private EcsPool<IncomeTimer> _incomeTimerPool;
        private Dictionary<int, BusinessCardView> _viewById;
        private EcsPool<BusinessCard> _bussinessCardPool;

        public UpdateSliderSystem(EcsWorld world, IGameFactory gameFactory)
        {
            _world = world;
            _viewById = gameFactory.ViewById;
        }


        public void Init(IEcsSystems systems) {
            _businessCardFilter = _world.Filter<BusinessCard>().Inc<IncomeTimer>().End();
            _incomeTimerPool = _world.GetPool<IncomeTimer>();
            _bussinessCardPool = _world.GetPool<BusinessCard>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var index in _businessCardFilter)
            {
                ref var incomeTimer = ref _incomeTimerPool.Get(index);
                ref var businessCard = ref _bussinessCardPool.Get(index);

                if (businessCard.Level <= 0) continue;

                if (_viewById.TryGetValue(businessCard.EntityId, out var view))
                {
                    view.Slider.value = CalculateSliderValue(incomeTimer, businessCard);
                }
            }
        }

        private float CalculateSliderValue(IncomeTimer incomeTimer, BusinessCard businessCard)
        {
            float clampedTimer = Mathf.Clamp(incomeTimer.Timer, 0f, businessCard.IncomeDelay);
            float progress = (businessCard.IncomeDelay - clampedTimer) / businessCard.IncomeDelay;
            
            return Mathf.Clamp01(progress);
        }
    }
}