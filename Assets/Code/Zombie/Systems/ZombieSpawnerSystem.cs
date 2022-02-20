using Code.Zombie.DataComponents;
using Code.Zombie.Tags;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UIElements;
using Random = Unity.Mathematics.Random;

namespace Code.Zombie.Systems
{
    public class ZombieSpawnerSystem : SystemBase
    {
        BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;
    
        protected override void OnCreate()
        {
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var isInput = Input.GetKeyDown(KeyCode.V);
            if (!isInput)
                return;
            
            var commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            Entities.ForEach((Entity entity, int entityInQueryIndex, in EnemySpawnerData spawner) =>
            {
                for (int i = 0; i < spawner.SpawnCount; i++)
                {
                    var instance = commandBuffer.Instantiate(entityInQueryIndex, spawner.EnemyPrefab);
                    commandBuffer.AddComponent<FreshEnemyTag>(entityInQueryIndex, instance);
                }
            }).ScheduleParallel();
            m_EntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
