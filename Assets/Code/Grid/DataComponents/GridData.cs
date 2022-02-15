using System;
using Unity.Entities;

namespace Code.Grid.DataComponents
{
    [Serializable]
    public struct GridData : IComponentData
    {
        public int x;
        public int y;
        public float offsetX;
        public float offsetY;
    }
}
