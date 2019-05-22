using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public float turnDelay = 1;//seconds between turns

    private float lastTurnTime = 0;

    private bool paused;
    public bool Paused
    {
        get { return paused; }
        set
        {
            if (!value && paused != value)
            {
                lastTurnTime = Time.time;
            }
            paused = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused && Time.time > lastTurnTime + turnDelay)
        {
            lastTurnTime = Time.time;
            foreach(BotController bc in FindObjectsOfType<BotController>())
            {
                bc.takeTurn();
            }
            GridManager.checkAllAreaEffects();
        }
    }
}
