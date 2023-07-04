using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct AsteroidSpawnerData : IComponentData
{
    public float radius;
    public float amount;
}
