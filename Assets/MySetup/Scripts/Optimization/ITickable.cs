using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface  ITickable
{
    void Tick(float deltaTime, float distanceMultiplier = 1f);
}
