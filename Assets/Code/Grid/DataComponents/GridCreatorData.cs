using System;
using Unity.Entities;
using UnityEngine;

namespace Code.Grid.DataComponents
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct GridCreatorData : IComponentData
    {
        public Entity prefab;
        public Vector2 gridSize;
    }
}