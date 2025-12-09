using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Conditions/AtPlace")]
public class AtPlace : SO_StateCondition
{
    public string PlaceTagCompare;
    
    public override bool EvaluateCondition(DeerFSM deerFSM)
    {
        var pois = GameObject.FindObjectsOfType<PointOfInterest>();

        foreach (var poi in pois)
        {
            if(poi.PlaceTag != PlaceTagCompare) continue;

            float sqrDistance = (deerFSM.transform.position - poi.transform.position).sqrMagnitude;
            float sqrRadius = poi.Radius * poi.Radius;

            if (sqrDistance <= sqrRadius) return true;
        }

        return false;
    }
}
