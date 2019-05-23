using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : Entity
{

    public List<Instruction> instructions;
    public List<Instruction> alphabet;
    public Instruction doNothingInstruction;
    public Instruction brokenInstruction;

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
            if (inst != null)
            {
                ProcessContext context = new ProcessContext(this, i);
                inst.doAction(context);
                i = inst.getLastParameterIndex(context);
                i = context.next(i, false);
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
                if (inst != null
                    && inst.name != "BROKEN")
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
        //Get list of undamaged instructions
        List<int> undamagedIndices = new List<int>();
        for (int i = 0; i < instructions.Count; i++)
        {
            Instruction inst = instructions[i];
            if (inst != null
                && inst.name != "BROKEN")
            {
                undamagedIndices.Add(i);
            }
        }
        //Randomly choose which indices to damage
        for (int i = 0; i < damageAmount; i++)
        {
            if (undamagedIndices.Count > 0)
            {
                int randomIndex = Random.Range(0, undamagedIndices.Count);
                int damagedIndex = undamagedIndices[randomIndex];
                undamagedIndices.RemoveAt(randomIndex);
                destroyInstruction(damagedIndex, brokenInstruction);
            }
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
            if (instructions[i] != null)
            {
                ProcessContext context = new ProcessContext(this, i);
                instructions[i].updateInstructionMap(context, ref processMap);
            }
        }
        return processMap;
    }
}
