using Code.Zombie.DataComponents;
using Code.Zombie.Tags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Code.Zombie.Systems
{
    // [UpdateAfter(typeof(ZombieSpawnerSystem))]
    public class ZombieSetPropertiesSystem : SystemBase
    {
        BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;
    
        protected override void OnCreate()
        {
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }
        
        protected override void OnUpdate()
        {
            var commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            Entities.ForEach((Entity entity, int entityInQueryIndex, in FreshEnemyTag enemyTag) =>
            {
                var minimumPosition = new float3(-20, 0, -20);
                var maximumPosition = new float3(20, 0, 20);
                var randomPos = Random.CreateFromIndex((uint) (entity.Index))
                    .NextFloat3(minimumPosition, maximumPosition);

                commandBuffer.SetComponent(entityInQueryIndex, entity,
                    new Translation { Value = randomPos});
                commandBuffer.RemoveComponent<FreshEnemyTag>(entityInQueryIndex, entity);
            }).ScheduleParallel();
            m_EntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
