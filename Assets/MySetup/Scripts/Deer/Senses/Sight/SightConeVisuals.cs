using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SightConeVisuals : MonoBehaviour
{
    // public DeerSight sight;
    // public int segments = 35;
    // public float lineWidth = 0.05f;
    // public Color colorNear = new Color(1f, 1f, 1f, 0.5f);
    // public Color colorFar  = new Color(1f, 1f, 1f, 0f);
    //
    // private LineRenderer lr;
    //
    // void Awake()
    // {
    //     lr = GetComponent<LineRenderer>();
    //     lr.positionCount = segments + 2;
    //     lr.widthMultiplier = lineWidth;
    //     lr.useWorldSpace = true;
    // }
    //
    // void LateUpdate()
    // {
    //     if (sight == null || sight.profile == null)
    //         return;
    //
    //     DrawCone();
    // }
    //
    // private void DrawCone()
    // {
    //     SO_SightconeProfile p = sight.profile;
    //
    //     float halfFov = p.FOV * 0.5f;
    //     lr.positionCount = segments + 2;
    //     
    //     Gradient gradient = new Gradient();
    //     gradient.SetKeys(
    //         new GradientColorKey[] {
    //             new GradientColorKey(colorNear, 0f),
    //             new GradientColorKey(colorFar, 1f)
    //         },
    //         new GradientAlphaKey[] {
    //             new GradientAlphaKey(colorNear.a, 0f),
    //             new GradientAlphaKey(colorFar.a, 1f)
    //         }
    //     );
    //     lr.colorGradient = gradient;
    //
    //     Vector3 origin = transform.position;
    //     lr.SetPosition(0, origin);
    //
    //     int index = 1;
    //     for (int i = 0; i <= segments; i++)
    //     {
    //         float t = (float)i / segments;
    //         float angle = Mathf.Lerp(-halfFov, halfFov, t);
    //
    //         Vector3 dir = Quaternion.Euler(0, angle, 0) * transform.forward;
    //         Vector3 pos = origin + dir * p.MaxRange;
    //
    //         lr.SetPosition(index, pos);
    //         index++;
    //     }
    // } 
}
