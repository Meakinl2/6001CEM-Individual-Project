using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct Vehicle : IComponentData
{
    public float speed;
    public int currentNodeIndex;
    public int nextNodeIndex;
    public float2 position;
}
