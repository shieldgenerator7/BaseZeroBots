using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Memory", menuName = "Instruction/Memory", order = 1)]
public class Memory : Instruction
{
    public enum Option
    {
        STORE,
        LOAD,
        ERASE
    }
    [Header("Memory")]
    public Option option;

    private int getMemoryLocation(ProcessContext context)
    {
        int[] paramIndices = getParameterIndices(context);
        int param1 = paramIndices[0];
        int memoryLocation = (int)context.instruction(param1)
            .instructionToNumber(context.context(param1));
        Debug.Log("Getting memory location["+memoryLocation+"]");
        return memoryLocation;
    }

    private bool canAccessMemory(BotController bc, int memoryLocation, Type type)
    {
        return option == Option.LOAD
            && bc.memory.ContainsKey(memoryLocation)
            && bc.memory[memoryLocation].GetType() == type
            ;
    }

    public override void doAction(ProcessContext context)
    {
        int[] paramIndices = getParameterIndices(context);
        int param1 = paramIndices[0];
        int memoryLocation = (int)context.instruction(param1)
            .instructionToNumber(context.context(param1));
        BotController bc = context.botController;
        switch (option)
        {
            case Option.STORE:
                int param2 = paramIndices[1];
                object storeObject = context.instruction(param2)
                    .getReturnObject(context.context(param2));
                if (!bc.memory.ContainsKey(memoryLocation))
                {
                    bc.memory.Add(memoryLocation, storeObject);
                }
                else
                {
                    bc.memory[memoryLocation] = storeObject;
                }
                Debug.Log("Stored["+memoryLocation+"]: " + bc.memory[memoryLocation]);
                break;
            case Option.ERASE:
                bc.memory.Remove(memoryLocation);
                break;
        }
    }

    public override bool testCondition(ProcessContext context)
    {
        int memoryLocation = getMemoryLocation(context);
        BotController bc = context.botController;
        switch (option)
        {
            case Option.STORE:
                return bc.memory.ContainsKey(memoryLocation);
            case Option.LOAD:
                return bc.memory.ContainsKey(memoryLocation)
                    && (bool)bc.memory[memoryLocation];
            case Option.ERASE:
                return !bc.memory.ContainsKey(memoryLocation);
            default:
                return false;
        }
    }

    public override float instructionToNumber(ProcessContext context)
    {
        int memoryLocation = getMemoryLocation(context);
        BotController bc = context.botController;
        if (canAccessMemory(bc, memoryLocation, typeof(float)))
        {
            return (float)bc.memory[memoryLocation];
        }
        return base.instructionToNumber(context);
    }

    public override Vector2 instructionToPosition(ProcessContext context)
    {
        int memoryLocation = getMemoryLocation(context);
        BotController bc = context.botController;
        if (canAccessMemory(bc, memoryLocation, typeof(Vector2)))
        {
            return (Vector2)bc.memory[memoryLocation];
        }
        return base.instructionToPosition(context);
    }

    public override Entity instructionToEntity(ProcessContext context)
    {
        int memoryLocation = getMemoryLocation(context);
        BotController bc = context.botController;
        if (canAccessMemory(bc, memoryLocation, typeof(BotController)))
        {
            return (Entity)bc.memory[memoryLocation];
        }
        return base.instructionToEntity(context);
    }

    public override object instructionToObject(ProcessContext context)
    {
        int memoryLocation = getMemoryLocation(context);
        BotController bc = context.botController;
        if (canAccessMemory(bc, memoryLocation, typeof(object)))
        {
            return bc.memory[memoryLocation];
        }
        return base.instructionToObject(context);
    }
}
