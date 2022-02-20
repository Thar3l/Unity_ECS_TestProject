using Unity.Entities;

namespace Code.Zombie.DataComponents
{
    [GenerateAuthoringComponent]
    public struct EnemySpawnerData : IComponentData
    {
        public Entity EnemyPrefab;
        public int SpawnCount;
    }
}
