using Unity.Entities;
using Unity.Mathematics;

namespace Code.Zombie.DataComponents
{
    public struct TargetPositionData : IComponentData
    {
        public float3 Value;
    }
}
