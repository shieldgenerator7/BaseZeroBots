using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{

    public List<Instruction> instructions;
    public float turnDelay = 1;//seconds between turns
    public bool alive = true;

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
            for (int i = 0; i < instructions.Count; i++)
            {
                Instruction inst = instructions[i];
                if (inst)
                {
                    inst.doAction(this, i, instructions);
                }
            }
            //If it has been destroyed,
            if (!alive)
            {
                //Destroy it
                //this allows for simultaneous destruction
                Destroy(gameObject);
            }
        }
    }
}
