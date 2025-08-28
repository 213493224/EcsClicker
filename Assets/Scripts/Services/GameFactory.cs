using System.Collections.Generic;
using Components;
using Core;
using Data;
using Leopotam.EcsLite;
using Systems;
using UI;
using UiAdapters;
using UnityComponents;
using UnityEngine;

namespace Services
{
    internal class GameFactory : IGameFactory
    {
        private readonly PlayerProgress _progress;
        private readonly IStaticDataService _staticDataService;
        private readonly UiClickEventSystem _uiClickEventSystem;
        private readonly UiRoot _uiRoot;
        private readonly EcsWorld _world;

        public Balance Balance { get; private set; }
        public Dictionary<int, BusinessCardView> ViewById { get; private set;}

        public GameFactory(EcsWorld world, UiRoot uiRootRoot, PlayerProgress progress,
            IStaticDataService staticDataService, UiClickEventSystem uiClickEventSystem)
        {
            _world = world;
            _uiRoot = uiRootRoot;
            _progress = progress;
            _staticDataService = staticDataService;
            _uiClickEventSystem = uiClickEventSystem;
            ViewById = new Dictionary<int, BusinessCardView>();
        }
    
        public void CreateBalance()
        {
            Balance = new Balance
            {
                Value = _progress.BalanceData.Value
            };

            var balanceView = _uiRoot.hud.GetComponentInChildren<BalanceView>();
            balanceView.BalanceLabel.text = _staticDataService.ForHud().BalanceLabel.AsLabel();
            balanceView.Value.text = Balance.Value.AsCurrency();
        }

        public void CreateBusinessCards()
        {
            var businessCards = _progress.BusinessCards;
            foreach (var businessCardFromSave in businessCards) CreateBusinessCard(businessCardFromSave);

            _uiRoot.ResetScrollToTop();
        }

        private void CreateBusinessCard(BusinessCardData businessCardFromSave)
        {
            var entity = _world.NewEntity();
            ref var businessCard = ref _world.GetPool<BusinessCard>().Add(entity);

            var businessStaticData = _staticDataService.ForBusiness(businessCardFromSave.Id);

            businessCard.EntityId = entity;
            businessCard.BusinessTypeId = businessCardFromSave.Id;
            businessCard.Level = businessCardFromSave.Level;
            businessCard.Income = businessCardFromSave.Income;
            businessCard.LevelUpPrice = businessCardFromSave.LevelUpPrice;
            businessCard.PowerUps = new List<EcsPackedEntityWithWorld>();

            businessCard.IncomeDelay = businessStaticData.IncomeDelay;

            ref var incomeTimer = ref _world.GetPool<IncomeTimer>().Add(entity);

            incomeTimer.Timer = businessCardFromSave.IncomeTimerState >= 0
                ? businessCardFromSave.IncomeTimerState
                : businessCard.IncomeDelay;

            var businessCardObj = (GameObject)Object.Instantiate(Resources.Load(AssetPath.BusinessCard),
                _uiRoot.businessCardContainer);

            var businessCardView = businessCardObj.GetComponent<BusinessCardView>();

            businessCardView.Name.text = businessStaticData.Name.InQuotes();
            businessCardView.LevelUpPriceLabel.text = businessStaticData.LevelUpPriceLabel.AsLabel();
            businessCardView.IncomeLabel.text = businessStaticData.IncomeLabel.AsLabel();
            businessCardView.LevelLabel.text = businessStaticData.LevelLabel.AsLabel();

            businessCardView.Id = businessCard.BusinessTypeId;
            businessCardView.Level.text = businessCard.Level.ToString();
            businessCardView.Income.text = businessCard.Income.AsCurrency();
            businessCardView.LevelUpPrice.text = businessCard.LevelUpPrice.AsCurrency();

            CreatePowerUps(businessCardFromSave, businessCardView, ref businessCard);

            var businessCardButtonAdapter = businessCardObj.GetComponent<BusinessCardButtonAdapter>();

            businessCardButtonAdapter.Init(_uiClickEventSystem);
            
            CacheBusinessCardView(businessCard, businessCardView);
        }

        private void CacheBusinessCardView(BusinessCard businessCard, BusinessCardView businessCardView)
        {
            ViewById.Add(businessCard.EntityId, businessCardView);
        }

        private void CreatePowerUps(BusinessCardData businessCardFromSave, BusinessCardView businessCardView,
            ref BusinessCard businessCard)
        {
            var powerUps = businessCardFromSave.PowerUps;
            
            foreach (var powerUpFromSave in powerUps)
                CreatePowerUp(businessCardView, powerUpFromSave, ref businessCard);
        }

        private void CreatePowerUp(BusinessCardView businessCardView, PowerUpData powerUpFromSave,
            ref BusinessCard businessCard)
        {
            var entity = _world.NewEntity();
            ref var powerUp = ref _world.GetPool<PowerUp>().Add(entity);

            var powerUpStaticData = _staticDataService.ForPowerUp(powerUpFromSave.Id);

            powerUp.Id = powerUpFromSave.Id;
            powerUp.BusinessTypeId = powerUpFromSave.BusinessId;
            powerUp.Unlocked = powerUpFromSave.Unlocked;
            powerUp.BusinessId = businessCard.EntityId;

            powerUp.Price = powerUpStaticData.Price;
            powerUp.IncomeMultiplyerPercent = powerUpStaticData.IncomeMultiplyerPercent;

            var powerUpObj = (GameObject)Object.Instantiate(Resources.Load(AssetPath.PowerUp),
                businessCardView.PowerUpRoot);

            var powerUpView = powerUpObj.GetComponent<PowerUpView>();

            powerUpView.Id = powerUpFromSave.Id;

            powerUpView.Name.text = powerUpStaticData.Name.InQuotes();
            powerUpView.IncomeLabel.text = powerUpStaticData.IncomeLabel.AsLabel();
            powerUpView.PriceLabel.text = powerUpStaticData.PriceLabel.AsLabel();

            powerUpView.Income.text = powerUpStaticData.IncomeMultiplyerPercent.AsIncomeBonus();
            powerUpView.Price.text = powerUpStaticData.Price.AsCurrency();

            var packed = _world.PackEntityWithWorld(entity);

            businessCard.PowerUps.Add(packed);

            if (powerUp.Unlocked)
                _world.GetPool<Unlocked>().Add(entity);

            var powerUpButtonAdapter = powerUpObj.GetComponent<PowerUpButtonAdapter>();
            powerUpButtonAdapter.Init(_uiClickEventSystem);
        }
    }
}