using Code.Zombie.DataComponents;
using Code.Zombie.Tags;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


namespace Code.Zombie.Systems
{
    [AlwaysSynchronizeSystem]
    public class PlayerMovementSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var deltaTime = Time.DeltaTime;
            var curInput = new float2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Entities.WithoutBurst().ForEach((ref Translation translation, in SpeedData speedData, in PlayerTag player) =>
            {
                var currentPosition = translation.Value;
                translation.Value = new float3(
                    currentPosition.x + curInput.x * speedData.speed,
                    0f,
                    currentPosition.z + curInput.y * speedData.speed);
            }).Run();
            return default;
        }
    }
}
