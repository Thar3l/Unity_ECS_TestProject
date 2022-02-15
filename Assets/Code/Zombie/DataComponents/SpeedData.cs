using Unity.Entities;

namespace Code.Zombie.DataComponents
{
    [GenerateAuthoringComponent]
    public struct SpeedData : IComponentData
    {
        public float speed;
    }
}
