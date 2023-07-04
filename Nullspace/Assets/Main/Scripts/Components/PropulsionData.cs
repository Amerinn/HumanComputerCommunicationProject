using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct PropulsionData : IComponentData
{
    public float3 Current;
    public float3 Target;
    public float3 Limit;
    public float3 Change;
}
