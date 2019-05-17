using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Instruction", menuName = "Instruction/Instruction", order = 1)]
public class Instruction : ScriptableObject
{
    public Sprite symbol;
    public new string name;
    public List<ReturnType> parameters;
    public KeyCode keyCode;

    public bool lifeInstruction = true;//true: this instruction allows for the bot to be alive

    public bool command = true;//true: can be used as a command
    public enum ReturnType
    {
        NONE,
        BOOL,
        NUMBER,
        POSITION,
        DIRECTION,
        ENTITY,
        OBJECT
    }
    public List<ReturnType> returnTypes;

    public enum ProcessedAs
    {
        DO_NOTHING,
        COMMAND,
        QUERY,
        CONSTANT
    }

    public int[] getParameterIndices(int currentIndex, List<Instruction> instructions)
    {
        int[] paramIndices = new int[parameters.Count];
        int paramIndex = currentIndex + 1;
        for (int i = 0; i < parameters.Count; i++)
        {
            paramIndex = (int)Mathf.Repeat(paramIndex, instructions.Count);
            paramIndices[i] = paramIndex;
            if (validParameter(parameters[i], instructions[paramIndex]))
            {
                paramIndex = instructions[paramIndex].getLastParameterIndex(paramIndex, instructions);
            }
            paramIndex++;
        }
        return paramIndices;
    }

    public int getLastParameterIndex(int currentIndex, List<Instruction> instructions)
    {
        int lastIndex = currentIndex + 1;
        for (int i = 0; i < parameters.Count; i++)
        {
            lastIndex = (int)Mathf.Repeat(lastIndex, instructions.Count);
            if (validParameter(parameters[i], instructions[lastIndex]))
            {
                lastIndex = instructions[lastIndex].getLastParameterIndex(lastIndex, instructions);
            }
            lastIndex++;
        }
        lastIndex--;
        lastIndex = (int)Mathf.Repeat(lastIndex, instructions.Count);
        return lastIndex;
    }

    public void updateInstructionMap(int currentIndex, List<Instruction> instructions, ref ProcessedAs[] processMap)
    {
        ProcessedAs currentPA = processMap[currentIndex];
        if (currentPA == ProcessedAs.CONSTANT)
        {
            return;
        }
        if (currentPA == ProcessedAs.DO_NOTHING)
        {
            if (command)
            {
                currentPA = processMap[currentIndex] = ProcessedAs.COMMAND;
            }
            if (returnTypes.Count > 0)
            {
                currentPA = processMap[currentIndex] = ProcessedAs.QUERY;
            }
        }
        int lastIndex = currentIndex + 1;
        for (int i = 0; i < parameters.Count; i++)
        {
            if (lastIndex >= instructions.Count)
            {
                //don't re-paint-process the instructions at the beginning
                break;
            }
            if (validParameter(parameters[i], instructions[lastIndex]))
            {
                if (parameters[i] == ReturnType.NONE
                    && instructions[lastIndex].command)
                {
                    processMap[lastIndex] = ProcessedAs.COMMAND;
                }
                else
                {
                    processMap[lastIndex] = ProcessedAs.QUERY;
                }
                lastIndex = instructions[lastIndex].getLastParameterIndex(lastIndex, instructions);
            }
            else
            {
                processMap[lastIndex] = ProcessedAs.CONSTANT;
            }
            lastIndex++;
        }
    }

    public bool validParameter(ReturnType parameter, Instruction inst)
    {
        return inst.returnTypes.Contains(parameter)
            || (parameter == ReturnType.OBJECT && inst.returnTypes.Count > 0)
            || parameter == ReturnType.NONE;
    }

    public virtual void doAction(BotController bc, int currentIndex, List<Instruction> instructions)
    {
    }

    public object getReturnObject(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        switch (returnTypes[0])
        {
            case ReturnType.BOOL:
                return testCondition(bc, currentIndex, instructions);
            case ReturnType.NUMBER:
                return instructionToNumber(bc, currentIndex, instructions);
            case ReturnType.POSITION:
                return instructionToPosition(bc, currentIndex, instructions);
            case ReturnType.DIRECTION:
                return instructionToDirection(bc, currentIndex, instructions);
            case ReturnType.ENTITY:
                return instructionToEntity(bc, currentIndex, instructions);
            case ReturnType.OBJECT:
                return instructionToObject(bc, currentIndex, instructions);
            default:
                return null;
        }
    }

    public virtual bool testCondition(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        return false;
    }

    public virtual float instructionToNumber(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        return bc.alphabet.IndexOf(instructions[currentIndex]);
    }

    public virtual Vector2 instructionToPosition(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        return Vector2.zero;
    }

    /// <summary>
    /// Local direction, i.e. "Vector2.up" is actually "forward" depending on rotation
    /// </summary>
    /// <param name="bc"></param>
    /// <param name="currentIndex"></param>
    /// <param name="instructions"></param>
    /// <returns></returns>
    public virtual Vector2 instructionToDirection(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        return Vector2.zero;
    }

    public virtual Entity instructionToEntity(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        return null;
    }

    public virtual object instructionToObject(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        return null;
    }
}
