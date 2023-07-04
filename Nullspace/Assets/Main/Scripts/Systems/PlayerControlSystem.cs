using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial class PlayerControlSystem : SystemBase
{
    Camera camera;
    PlayerInput InputControl;
    bool isMouseLocked;
    protected override void OnCreate()
    {
        InputControl = new PlayerInput();
        InputControl.Enable();
        InputControl.InShip.Enable();
        isMouseLocked = true;
    }
    protected override void OnUpdate()
    {
        if(camera == null)
            camera = Camera.main;

        foreach (var (playerTag, movementAspect) in SystemAPI.Query<PlayerControlledTag, MovementAspect>())
        {

            float dt = SystemAPI.Time.DeltaTime;
            var propulsionData = movementAspect.propulsionData.ValueRO;
            var handlingData = movementAspect.handlingData.ValueRO;
            var inShip = InputControl.InShip;

            isMouseLocked ^= inShip.MouseLock.ReadValue<float>() != 0 && inShip.MouseLock.WasPressedThisFrame();
            var handbrake = inShip.Handbrake.ReadValue<float>();

            var thrust = inShip.Thrust.ReadValue<float>();
            var vertical = inShip.Vertical.ReadValue<float>();
            var horizontal = inShip.Horizontal.ReadValue<float>();

            var roll = inShip.Roll.ReadValue<float>();
            var mousePos = inShip.PitchYaw.ReadValue<Vector2>();

            var target = new Vector2(Screen.width /2.0f, Screen.height/2.0f);
            var direction = (mousePos - target).normalized;
            var distance = Vector2.Distance(target, mousePos);
            var limit = Mathf.InverseLerp(0f, 500f, distance);

            movementAspect.propulsionData.ValueRW.Target = handbrake != 0 ? 0 : new float3(
                horizontal == 0 ? 0 : Mathf.Clamp(propulsionData.Target.x + horizontal * dt, -propulsionData.Limit.x, propulsionData.Limit.x),
                vertical == 0 ? 0 : Mathf.Clamp(propulsionData.Target.y + vertical * dt, -propulsionData.Limit.y, propulsionData.Limit.y),
                Mathf.Clamp(propulsionData.Target.z + thrust * dt, -propulsionData.Limit.z, propulsionData.Limit.z)
            );
            movementAspect.handlingData.ValueRW.Target = handbrake != 0 ? 0 : new float3(
                isMouseLocked ? 0 : Mathf.Clamp(handlingData.Target.x + -direction.y * limit * dt, -handlingData.Limit.x, handlingData.Limit.x),
                isMouseLocked ? 0 : Mathf.Clamp(handlingData.Target.y + direction.x * limit * dt, -handlingData.Limit.y, handlingData.Limit.y),
                roll == 0 ? 0 : Mathf.Clamp(handlingData.Target.z + roll * dt, -handlingData.Limit.z, handlingData.Limit.z)
            );
        }
    }
}
