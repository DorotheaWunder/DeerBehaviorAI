using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/FindTagAnchor")]
public class FindTagAnchor : SO_StateAction
{
    public string AnchorTag;
    public AnchorSelectionMode SelectionMode = AnchorSelectionMode.Closest;
    public int RandomClosestCount = 3;

    public override void ExecuteAction(DeerFSM deerFSM)
    {
        var bb = deerFSM.DeerBlackboard;
        var selfPos = deerFSM.transform.position;
        
        var targets = GameObject.FindGameObjectsWithTag(AnchorTag);
        if (targets == null || targets.Length == 0)
            return;
        
        GameObject chosen = null;

        switch (SelectionMode)
        {
            case AnchorSelectionMode.Closest:
                chosen = FindClosest(targets, selfPos);
                break;

            case AnchorSelectionMode.Random:
                chosen = targets[Random.Range(0, targets.Length)];
                break;

            case AnchorSelectionMode.RandomClosest:
                chosen = FindRandomOfClosest(targets, selfPos, RandomClosestCount);
                break;
        }

        if (chosen == null)
            return;

        bb.AnchorPosition = chosen.transform.position;
    }
        
    private GameObject FindClosest(GameObject[] targets, Vector3 selfPos)
    {
        GameObject best = null;
        float bestDist = float.MaxValue;

        foreach (var t in targets)
        {
            float d = Vector3.Distance(selfPos, t.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = t;
            }
        }
        return best;
    }

    private GameObject FindRandomOfClosest(GameObject[] targets, Vector3 selfPos, int count)
    {
        var list = new List<GameObject>(targets);
        list.Sort((a, b) =>
            Vector3.Distance(selfPos, a.transform.position)
                .CompareTo(Vector3.Distance(selfPos, b.transform.position)));

        int max = Mathf.Min(count, list.Count);
        return list[Random.Range(0, max)];
    }
}

public enum AnchorSelectionMode
{
    Closest,
    Random,
    RandomClosest
}