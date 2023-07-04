using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

public class HUDController : MonoBehaviour
{
    public RectTransform Aim;
    public RectTransform Center;
    public RectTransform Direction;

    public TMP_Text VelocityTargetX;
    public TMP_Text VelocityTargetY;
    public TMP_Text VelocityTargetZ;
    public TMP_Text VelocityCurrentX;
    public TMP_Text VelocityCurrentY;
    public TMP_Text VelocityCurrentZ;
    public TMP_Text RotationTargetX;
    public TMP_Text RotationTargetY;
    public TMP_Text RotationTargetZ;
    public TMP_Text RotationCurrentX;
    public TMP_Text RotationCurrentY;
    public TMP_Text RotationCurrentZ;
    private Entity target;

    void Start()
    {
        Cursor.visible = false;
    }
    void LateUpdate()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Aim.position = mousePosition;

        Center.position = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);

        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        if (target != Entity.Null)
        {
            var propulsion = entityManager.GetComponentData<PropulsionData>(target);
            var handling = entityManager.GetComponentData<HandlingData>(target);
            var localToWorld = entityManager.GetComponentData<LocalToWorld>(target);

            Direction.position = Camera.main.WorldToScreenPoint((float3)Camera.main.transform.position + localToWorld.Forward);

            VelocityTargetX.text = $"{Math.Round(propulsion.Target.x, 1)}";
            VelocityTargetY.text = $"{Math.Round(propulsion.Target.y, 1)}";
            VelocityTargetZ.text = $"{Math.Round(propulsion.Target.z, 1)}";
            VelocityCurrentX.text = $"{Math.Round(propulsion.Current.x, 1)}";
            VelocityCurrentY.text = $"{Math.Round(propulsion.Current.y, 1)}";
            VelocityCurrentZ.text = $"{Math.Round(propulsion.Current.z, 1)}";
            RotationTargetX.text = $"{Math.Round(handling.Target.x, 1)}";
            RotationTargetY.text = $"{Math.Round(handling.Target.y, 1)}";
            RotationTargetZ.text = $"{Math.Round(handling.Target.z, 1)}";
            RotationCurrentX.text = $"{Math.Round(handling.Current.x, 1)}";
            RotationCurrentY.text = $"{Math.Round(handling.Current.y, 1)}";
            RotationCurrentZ.text = $"{Math.Round(handling.Current.z, 1)}";
        }

        else
            target = GetTarget(entityManager);
    }
    private Entity GetTarget(EntityManager entityManager)
    {
        EntityQuery query = entityManager.CreateEntityQuery(typeof(PlayerControlledTag));
        NativeArray<Entity> array = query.ToEntityArray(Allocator.Temp);

        return array.Length > 0 ? array[0] : Entity.Null;
    }
}
