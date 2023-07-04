using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class FollowEntity : MonoBehaviour
{
    private Entity target;
    void LateUpdate()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        if(target != Entity.Null)
        {
            var entityTransform = entityManager.GetComponentData<LocalTransform>(target);
            transform.position = entityTransform.Position;
            transform.rotation = entityTransform.Rotation;
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
