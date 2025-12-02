using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerAI : MonoBehaviour
{
    [Header("References")]
    public GameObject Player;
    public DeerFSM FSM;
    public DeerSenseSuspicionManager Senses;
    
    public float TotalSuspicion => Senses.GetTotalSuspicion();

    private void Awake()
    {
        if (!FSM) FSM = GetComponent<DeerFSM>();
        if (!Senses) Senses = GetComponent<DeerSenseSuspicionManager>();
    }
}
