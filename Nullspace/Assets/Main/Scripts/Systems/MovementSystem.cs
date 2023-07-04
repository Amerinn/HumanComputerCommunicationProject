using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial struct MovementSystem : ISystem
{
    public void OnCreate(ref SystemState state) { }
    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((var movementAspect, var massData) in SystemAPI.Query<MovementAspect, PhysicsMass>())
        {
            movementAspect.CalculatePropulsion(SystemAPI.Time.DeltaTime);
            movementAspect.CalculateHandling(SystemAPI.Time.DeltaTime);

            movementAspect.ApplyPropulsion(massData, SystemAPI.Time.DeltaTime);
            movementAspect.ApplyHandling(massData, SystemAPI.Time.DeltaTime);
        }
    }
}
