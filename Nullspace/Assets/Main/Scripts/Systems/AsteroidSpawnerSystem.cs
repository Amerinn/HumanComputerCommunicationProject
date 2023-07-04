using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct AsteroidSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((var asteroidSpawner, var buffer) in SystemAPI.Query<AsteroidSpawnerData, DynamicBuffer<EntityWrapper>>())
        {
            for (int i = 0; i < asteroidSpawner.amount; i++)
            {
                var asteroid = state.EntityManager.Instantiate(buffer[UnityEngine.Random.Range(0, 51)].entity);
                var position = new float3(
                    UnityEngine.Random.Range(-asteroidSpawner.radius, asteroidSpawner.radius),
                    UnityEngine.Random.Range(-asteroidSpawner.radius, asteroidSpawner.radius),
                    UnityEngine.Random.Range(-asteroidSpawner.radius, asteroidSpawner.radius)
                );
                var scale = UnityEngine.Random.Range(100, 10000);
                state.EntityManager.SetComponentData(asteroid, LocalTransform.FromPositionRotationScale(position, UnityEngine.Random.rotation, scale));
            }
        }
        state.Enabled = false;
    }
}
