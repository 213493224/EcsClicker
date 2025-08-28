using Components;
using Data;
using Leopotam.EcsLite;
using Services;
using Systems;
using UnityComponents;
using UnityEngine;

namespace Core
{
    internal class EcsStartup : MonoBehaviour
    {
        [SerializeField] private UiRoot uiRoot;
        private IGameFactory _factory;
        private PlayerProgress _playerProgress;
        private ISaveLoadService _saveLoadService;

        private IStaticDataService _staticDataService;
        private IEcsSystems _systems;

        private EcsWorld _world;
        private UiClickEventSystem _uiClickEventSystem;

        private void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

            InitServices();

            InitSystems();
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
            }

            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }

        private void InitServices()
        {
            _staticDataService = new StaticDataService();
            _staticDataService.Load();


            _saveLoadService = new SaveLoadService(_staticDataService);
            _playerProgress =
                _saveLoadService.Load()
                ?? _saveLoadService.NewProgress();

            _uiClickEventSystem = new UiClickEventSystem();
            _factory = new GameFactory(_world, uiRoot, _playerProgress, _staticDataService, _uiClickEventSystem);
        }

        private void InitSystems()
        {
            _systems
                .Add(_uiClickEventSystem)
                .Add(new InitGameWorldSystem(_factory))
                .Add(new IncomeSystem(_world, _factory, _staticDataService))
                .Add(new BuyPowerUpSystem(_world, _factory, _staticDataService))
                .Add(new BuyLevelUpSystem(_world, _factory, _staticDataService))
                .Add(new UpdateBalanceViewSystem(_world, _factory, uiRoot))
                .Add(new UpdateBusinessCardViewSystem(_world, uiRoot))
                .Add(new UpdatePowerUpViewSystem(_world, _factory, uiRoot))
                .Add(new UpdateSliderSystem(_world, _factory))
                .Add(new SaveGameSystem(_world, _saveLoadService, _factory))

                .Add(new ClearEventsSystem<Clicked>())
                .Add(new ClearEventsSystem<UpdateComponentViewEvent>())
                .Add(new ClearEventsSystem<UpdateBalanceViewEvent>())
                .Add(new ClearEventsSystem<BuyPowerUpEvent>())
                .Add(new ClearEventsSystem<BuyLevelUpEvent>())
                .Add(new ClearEventsSystem<SaveEvent>())

                .Init();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SendSaveEvent();
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                SendSaveEvent();
            }
        }

        private void SendSaveEvent()
        {
            var world = _world;
            if (world == null) return;

            int entity = world.NewEntity();
            var pool = world.GetPool<SaveEvent>();
            pool.Add(entity);
        }
    }
}
