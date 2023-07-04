using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PropulsionAuthoring : MonoBehaviour
{
    public float3 Current;
    public float3 Target;
    public float3 Limit;
    public float3 Change;
}

public class PropulsionBaker : Baker<PropulsionAuthoring>
{
  public override void Bake(PropulsionAuthoring authoring)
  {
    var entity = GetEntity(TransformUsageFlags.None);
    AddComponent(entity, new PropulsionData
    {
      Current = authoring.Current,
      Target = authoring.Target,
      Limit = authoring.Limit,
      Change = authoring.Change,
    });
  }
}