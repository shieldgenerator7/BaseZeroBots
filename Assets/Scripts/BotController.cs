using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{

    public List<Instruction> instructions;

    private Dictionary<int, Instruction> indicesToDestroy = new Dictionary<int, Instruction>();

    // Update is called once per frame
    public void takeTurn()
    {
        for (int i = 0; i < instructions.Count; i++)
        {
            Instruction inst = instructions[i];
            if (inst)
            {
                inst.doAction(this, i, instructions);
            }
        }
        //If there are instructions to destroy,
        if (indicesToDestroy.Count > 0)
        {
            //Destroy them
            //doing it at this spot in the code
            //allows for simultaneous destruction
            foreach (int index in indicesToDestroy.Keys)
            {
                instructions[index] = indicesToDestroy[index];
            }
            //If this bot has no more instructions,
            bool instFound = false;
            foreach (Instruction inst in instructions)
            {
                if (inst.lifeInstruction)
                {
                    instFound = true;
                    break;
                }
            }
            if (!instFound)
            {
                //Destroy this bot
                Destroy(gameObject);
            }
            indicesToDestroy.Clear();
        }
    }

    public void destroyInstruction(int index, Instruction replacement)
    {
        if (indicesToDestroy.ContainsKey(index))
        {
            indicesToDestroy[index] = replacement;
        }
        else
        {
            indicesToDestroy.Add(index, replacement);
        }
    }
}
