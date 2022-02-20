using Code.Zombie.DataComponents;
using Code.Zombie.Tags;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace Code.Zombie.Systems
{
    public class SearchForPlayerSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimECBufferSystem;
        private EntityQuery _playerQuery;
        
        protected override void OnCreate()
        {
            _endSimECBufferSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
            _playerQuery = GetEntityQuery(ComponentType.ReadOnly<PlayerTag>(), ComponentType.ReadOnly<Translation>());
        }
        
        protected override void OnUpdate()
        {
            var playerPosition = _playerQuery.ToComponentDataArrayAsync<Translation>(Allocator.TempJob, out JobHandle getPositionHandle);
            var ecb = _endSimECBufferSystem.CreateCommandBuffer().AsParallelWriter();
            var targetPositionFromEntity = GetComponentDataFromEntity<TargetPositionData>(false);
            
            var lookHandle = Entities
                .WithReadOnly(playerPosition)
                .WithNativeDisableParallelForRestriction(targetPositionFromEntity)
                .WithDisposeOnCompletion(playerPosition)
                .ForEach((Entity enemyEntity, int entityInQueryIndex, in EnemyTag enemy) =>
                {
                    if (playerPosition.Length < 1)
                        return;

                    targetPositionFromEntity[enemyEntity] = new TargetPositionData {Value = playerPosition[0].Value};
                }).ScheduleParallel(getPositionHandle);;
            
            _endSimECBufferSystem.AddJobHandleForProducer(lookHandle);
            Dependency = lookHandle;
        }
    }
}
