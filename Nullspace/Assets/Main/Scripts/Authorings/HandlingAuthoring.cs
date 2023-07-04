using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class HandlingAuthoring : MonoBehaviour
{
    public float3 Current;
    public float3 Target;
    public float3 Limit;
    public float3 Change;
}

public class HandlingBaker : Baker<HandlingAuthoring>
{
  public override void Bake(HandlingAuthoring authoring)
  {
    var entity = GetEntity(TransformUsageFlags.None);
    AddComponent(entity, new HandlingData
    {
      Current = authoring.Current,
      Target = authoring.Target,
      Limit = authoring.Limit,
      Change = authoring.Change,
    });
  }
}