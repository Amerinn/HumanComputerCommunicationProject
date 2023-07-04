using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class AsteroidSpawnerAuthoring : MonoBehaviour
{
    public List<GameObject> prefab;
    public float radius;
    public float amount;
}

public class AsteroidSpawnerBaker : Baker<AsteroidSpawnerAuthoring>
{
    public override void Bake(AsteroidSpawnerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new AsteroidSpawnerData
        {
            radius = authoring.radius,
            amount = authoring.amount
        });
        var buffer = AddBuffer<EntityWrapper>(entity);
        foreach (var item in authoring.prefab)
        {
            buffer.Add(new EntityWrapper {entity = GetEntity(item, TransformUsageFlags.None)});
        }
    }
}