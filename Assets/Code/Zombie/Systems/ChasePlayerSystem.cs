using Code.Zombie.DataComponents;
using Code.Zombie.Tags;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Code.Zombie.Systems
{
    public class ChasePlayerSystem : SystemBase
    {
        // private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;
        private EntityQuery _playerQuery;

        protected override void OnCreate()
        {
            // _endSimulationEntityCommandBufferSystem =
            //     World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
            _playerQuery = GetEntityQuery(ComponentType.ReadOnly<PlayerTag>(), ComponentType.ReadOnly<Translation>());
        }

        protected override void OnUpdate()
        {
            // todo: fix leak errors, separate "serach for player" system and "move to target" system
            var playerPosition = _playerQuery.ToComponentDataArrayAsync<Translation>(Allocator.TempJob, out JobHandle getPositionHandle);
            Entities
                .WithReadOnly(playerPosition)
                // .WithDisposeOnCompletion(playerPosition)
                .ForEach((Entity enemyEntity, ref Translation enemyPosition, in SpeedData speedData, in EnemyTag enemyTag) =>
                {
                    var direction = math.normalize(playerPosition[0].Value - enemyPosition.Value);
                    var currentPosition = enemyPosition.Value;
                    enemyPosition.Value = new float3(
                        currentPosition.x + direction.x * speedData.speed,
                        0f,
                        currentPosition.z + direction.z * speedData.speed);
                }).ScheduleParallel();
        }
    }
}
