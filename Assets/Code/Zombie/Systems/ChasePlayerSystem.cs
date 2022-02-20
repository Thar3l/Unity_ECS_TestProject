using Code.Zombie.DataComponents;
using Code.Zombie.Tags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Code.Zombie.Systems
{
    public class ChasePlayerSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .ForEach((ref Translation enemyPosition, in SpeedData speedData, in EnemyTag enemyTag, in TargetPositionData targetPosition) =>
                {
                    var vectorToTarget = targetPosition.Value - enemyPosition.Value;
                    if (math.length(vectorToTarget) > CharacterUtility.StopMovingToTargetDistance)
                    {
                        var direction = math.normalize(vectorToTarget);
                        enemyPosition.Value = new float3(
                            enemyPosition.Value.x + direction.x * speedData.speed,
                            0f,
                            enemyPosition.Value.z + direction.z * speedData.speed);
                    }
                    
                }).ScheduleParallel();
        }
    }
}
