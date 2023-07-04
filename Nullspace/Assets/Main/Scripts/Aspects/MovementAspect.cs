using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public readonly partial struct MovementAspect : IAspect
{
    public readonly RefRW<PropulsionData> propulsionData;
    public readonly RefRW<HandlingData> handlingData;
    readonly RefRW<PhysicsVelocity> velocity;
    readonly RefRW<LocalToWorld> localToWorld;

    [BurstCompile]
    public void CalculatePropulsion(float dt)
    {
        var error = propulsionData.ValueRO.Target - propulsionData.ValueRO.Current;
        var acceleration = propulsionData.ValueRO.Change * dt;

        propulsionData.ValueRW.Current += new float3(
            Mathf.Clamp(error.x, -acceleration.x, acceleration.x),
            Mathf.Clamp(error.y, -acceleration.y, acceleration.y),
            Mathf.Clamp(error.z, -acceleration.z, acceleration.z)
        );
    }

    [BurstCompile]
    public void CalculateHandling(float dt)
    {
        var error = handlingData.ValueRO.Target - handlingData.ValueRO.Current;
        var acceleration = handlingData.ValueRO.Change * dt;

        handlingData.ValueRW.Current += new float3(
            Mathf.Clamp(error.x, -acceleration.x, acceleration.x),
            Mathf.Clamp(error.y, -acceleration.y, acceleration.y),
            Mathf.Clamp(error.z, -acceleration.z, acceleration.z)
        );
    }

    [BurstCompile]
    public void ApplyPropulsion(PhysicsMass mass, float dt)
    {
        var thrust = localToWorld.ValueRO.Forward * propulsionData.ValueRO.Current.z;
        var horizontal = localToWorld.ValueRO.Right * propulsionData.ValueRO.Current.x;
        var vertical = localToWorld.ValueRO.Up * propulsionData.ValueRO.Current.y;

        var vector = (float3)(thrust + horizontal + vertical) * dt;
        velocity.ValueRW.ApplyLinearImpulse(mass, vector);
    }

    [BurstCompile]
    public void ApplyHandling(PhysicsMass mass, float dt)
    {
        var roll = Vector3.forward * handlingData.ValueRO.Current.z;
        var pitch = Vector3.right * handlingData.ValueRO.Current.x;
        var yaw = Vector3.up * handlingData.ValueRO.Current.y;

        var vector = (float3)(roll + pitch + yaw) * dt;
        velocity.ValueRW.ApplyAngularImpulse(mass, vector);
    }
}
