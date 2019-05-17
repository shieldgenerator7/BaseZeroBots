using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{

    public List<Instruction> instructions;
    public List<Instruction> alphabet;
    public Instruction doNothingInstruction;

    public Dictionary<int, object> memory = new Dictionary<int, object>();

    private Dictionary<int, Instruction> indicesToDestroy = new Dictionary<int, Instruction>();

    public Vector3 moveDirection = Vector3.zero;

    // Update is called once per frame
    public void takeTurn()
    {
        //Process instructions
        moveDirection = Vector3.zero;
        int i = 0;
        while (i < instructions.Count)
        {
            Instruction inst = instructions[i];
            if (inst)
            {
                inst.doAction(this, i, instructions);
                i = inst.getLastParameterIndex(i, instructions) + 1;
            }
        }
        moveDirection.x = (moveDirection.x == 0) ? 0 : Mathf.Sign(moveDirection.x);
        moveDirection.y = (moveDirection.y == 0) ? 0 : Mathf.Sign(moveDirection.y);
        moveDirection = transform.TransformDirection(moveDirection);
        transform.position = GridManager.moveObject(gameObject, transform.position + moveDirection);
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

    public void damage(int damageAmount)
    {
        for (int i = 0; i < damageAmount; i++)
        {
            int randomIndex = Random.Range(0, instructions.Count);
            destroyInstruction(randomIndex, doNothingInstruction);
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
    
    /// <summary>
    /// Returns a list that shows how each instruction will be processed
    /// </summary>
    /// <returns></returns>
    public Instruction.ProcessedAs[] getInstructionMap()
    {
        Instruction.ProcessedAs[] processMap = new Instruction.ProcessedAs[instructions.Count];
        for (int i = 0; i < instructions.Count; i++)
        {
            instructions[i].updateInstructionMap(i, instructions, ref processMap);
        }
        return processMap;
    }
}
