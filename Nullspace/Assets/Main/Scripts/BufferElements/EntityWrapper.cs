using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[InternalBufferCapacity(51)]
public struct EntityWrapper : IBufferElementData
{
    public Entity entity;
}
