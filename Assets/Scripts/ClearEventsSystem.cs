using Leopotam.EcsLite;

namespace Systems
{
    public class ClearEventsSystem<T> : IEcsRunSystem where T : struct
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var pool = world.GetPool<T>();

            foreach (var entity in world.Filter<T>().End())
            {
                pool.Del(entity);
            }
        }
    }
}
