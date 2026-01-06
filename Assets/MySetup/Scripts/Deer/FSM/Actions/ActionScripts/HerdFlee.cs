using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/HerdFlee")]
public class HerdFlee : SO_StateAction
{
    [Header("Flee Settings")]
    public float FleeDistance = 20f;
    public float RepathThreshold = 5f;

    public override void ExecuteAction(DeerFSM deerFSM)
    {
        if (deerFSM == null || deerFSM.DeerAI == null)
            return;

        var bb = deerFSM.DeerBlackboard;
        var deerAI = deerFSM.DeerAI;

        if (deerAI.Player == null || deerAI.Herd == null || deerAI.Herd.CohesionManager == null)
            return;

        var herdManager = deerAI.Herd.CohesionManager;

        Vector3 herdCenter = herdManager.HerdCenter.position;
        Vector3 playerPos = deerAI.Player.transform.position;

        Vector3 herdFleeDir = (herdCenter - playerPos).normalized;
        Vector3 finalDir = deerAI.ApplyHerdDirection(herdFleeDir);

        Vector3 targetPos = deerFSM.transform.position + finalDir * FleeDistance;
        
        float offsetRadius = 2f;
        Vector2 randomCircle = Random.insideUnitCircle * offsetRadius;
        targetPos += new Vector3(randomCircle.x, 0f, randomCircle.y);

        if (!bb.HasGoal || Vector3.Distance(bb.GoalPosition, targetPos) > RepathThreshold)
        {
            bb.GoalPosition = targetPos;
            bb.HasGoal = true;
            bb.HasDestination = true;
            bb.MovementIntent = MovementIntent.MoveToPosition;
        }
    }
}
