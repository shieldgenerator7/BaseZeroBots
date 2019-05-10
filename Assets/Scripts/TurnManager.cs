using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public float turnDelay = 1;//seconds between turns

    private float lastTurnTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > lastTurnTime + turnDelay)
        {
            lastTurnTime = Time.time;
            foreach(BotController bc in FindObjectsOfType<BotController>())
            {
                bc.takeTurn();
            }
        }
    }
}
