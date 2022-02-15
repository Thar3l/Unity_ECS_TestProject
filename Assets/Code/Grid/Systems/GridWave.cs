using Code.Grid.DataComponents;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Code.Grid.Systems
{
    public class GridWave : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref Translation translation, ref GridData gridData) =>
            {
                var y = CalculateHeight(gridData.x, gridData.y, gridData.offsetX, gridData.offsetY);
                translation.Value = new float3(translation.Value.x, y, translation.Value.z);
                gridData.offsetX += 0.01f;
                gridData.offsetY += 0.01f;
            }).Schedule();
        }

        static float CalculateHeight(int x, int y, float offsetX, float offsetY)
        {
            float scale = 0.1f;
            float height = 5;
            float xCoord = Mathf.Sin(x * scale + offsetX);
            float yCoord = Mathf.Sin(y * scale + offsetY);
            return height * xCoord * yCoord;
        }
    }
}
