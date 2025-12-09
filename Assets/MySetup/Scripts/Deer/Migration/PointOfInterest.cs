using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    public string PlaceTag = "Meadow";
    public float Radius = 10f;
    public Color GizmoColor = Color.green;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = GizmoColor;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
