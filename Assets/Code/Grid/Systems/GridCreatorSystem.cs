using Code.Grid.DataComponents;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Code.Grid.Systems
{
    public class GridCreatorSystem : SystemBase
    {
        BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;
    
        protected override void OnCreate()
        {
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }
        protected override void OnUpdate()
        {
            var commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            Entities
                .WithBurst(FloatMode.Default, FloatPrecision.Standard, true)
                .ForEach((Entity entity, int entityInQueryIndex, in GridCreatorData gridCreator) =>
                {
                    for (int x = 0; x < gridCreator.gridSize.x; x++)
                    {
                        for (int y = 0; y < gridCreator.gridSize.y; y++)
                        {
                            var instance = commandBuffer.Instantiate(entityInQueryIndex, gridCreator.prefab);
                            commandBuffer.SetComponent(entityInQueryIndex, instance,
                                new Translation { Value = new float3(x, 0, y)});
                            commandBuffer.AddComponent(entityInQueryIndex, instance, new GridData {x=x, y=y});
                        }
                    }
                    commandBuffer.DestroyEntity(entityInQueryIndex, entity);
                }).ScheduleParallel();
            m_EntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
