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

    private int getMemoryLocation(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        int[] paramIndices = getParameterIndices(currentIndex, instructions);
        int param1 = paramIndices[0];
        int memoryLocation = (int)instructions[param1].instructionToNumber(bc, param1, instructions);
        Debug.Log("Getting memory location["+memoryLocation+"]");
        return memoryLocation;
    }

    private bool canAccessMemory(BotController bc, int memoryLocation, Type type)
    {
        return option == Option.LOAD
            && bc.memory.ContainsKey(memoryLocation)
            //&& bc.memory[memoryLocation].GetType() == type
            ;
    }

    public override void doAction(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        int[] paramIndices = getParameterIndices(currentIndex, instructions);
        int param1 = paramIndices[0];
        int memoryLocation = (int)instructions[param1].instructionToNumber(bc, param1, instructions);
        switch (option)
        {
            case Option.STORE:
                int param2 = paramIndices[1];
                object storeObject = instructions[param2].getReturnObject(bc, param2, instructions);
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

    public override bool testCondition(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        int memoryLocation = getMemoryLocation(bc, currentIndex, instructions);
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

    public override float instructionToNumber(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        int memoryLocation = getMemoryLocation(bc, currentIndex, instructions);
        if (canAccessMemory(bc, memoryLocation, typeof(float)))
        {
            return (float)bc.memory[memoryLocation];
        }
        return base.instructionToNumber(bc, currentIndex, instructions);
    }

    public override Vector2 instructionToPosition(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        int memoryLocation = getMemoryLocation(bc, currentIndex, instructions);
        if (canAccessMemory(bc, memoryLocation, typeof(Vector2)))
        {
            return (Vector2)bc.memory[memoryLocation];
        }
        return base.instructionToPosition(bc, currentIndex, instructions);
    }

    public override Entity instructionToEntity(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        int memoryLocation = getMemoryLocation(bc, currentIndex, instructions);
        if (canAccessMemory(bc, memoryLocation, typeof(BotController)))
        {
            return (Entity)bc.memory[memoryLocation];
        }
        return base.instructionToEntity(bc, currentIndex, instructions);
    }

    public override object instructionToObject(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        int memoryLocation = getMemoryLocation(bc, currentIndex, instructions);
        if (canAccessMemory(bc, memoryLocation, typeof(object)))
        {
            return bc.memory[memoryLocation];
        }
        return base.instructionToObject(bc, currentIndex, instructions);
    }
}
