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

    public int[] getParameterIndices(ProcessContext context)
    {
        int[] paramIndices = new int[parameters.Count];
        int paramIndex = context.next();
        for (int i = 0; i < parameters.Count; i++)
        {
            paramIndices[i] = paramIndex;
            if (validParameter(parameters[i], context.instruction(paramIndex)))
            {
                paramIndex = context.getLastParameterIndex(paramIndex);
            }
            paramIndex = context.next(paramIndex);
        }
        return paramIndices;
    }

    public int getLastParameterIndex(ProcessContext context)
    {
        int lastIndex = context.next();
        for (int i = 0; i < parameters.Count; i++)
        {
            if (validParameter(parameters[i], context.instruction(lastIndex)))
            {
                lastIndex = context.getLastParameterIndex(lastIndex);
            }
            lastIndex = context.next(lastIndex);
        }
        lastIndex = context.prev(lastIndex);
        return lastIndex;
    }

    public void updateInstructionMap(ProcessContext context, ref ProcessedAs[] processMap)
    {
        if (context.instruction() == null)
        {
            return;
        }
        ProcessedAs currentPA = processMap[context.Index];
        if (currentPA == ProcessedAs.CONSTANT)
        {
            return;
        }
        if (currentPA == ProcessedAs.DO_NOTHING)
        {
            if (command)
            {
                currentPA = processMap[context.Index] = ProcessedAs.COMMAND;
            }
            if (returnTypes.Count > 0)
            {
                currentPA = processMap[context.Index] = ProcessedAs.QUERY;
            }
        }
        int lastIndex = context.next(context.Index, false);
        for (int i = 0; i < parameters.Count; i++)
        {
            if (lastIndex >= context.Count)
            {
                //don't re-paint-process the instructions at the beginning
                break;
            }
            if (validParameter(parameters[i], context.instruction(lastIndex)))
            {
                if (parameters[i] == ReturnType.NONE
                    && context.instruction(lastIndex).command)
                {
                    processMap[lastIndex] = ProcessedAs.COMMAND;
                }
                else
                {
                    processMap[lastIndex] = ProcessedAs.QUERY;
                }
                lastIndex = context.getLastParameterIndex(lastIndex);
            }
            else
            {
                processMap[lastIndex] = ProcessedAs.CONSTANT;
            }
            lastIndex = context.next(lastIndex);
        }
    }

    public bool validParameter(ReturnType parameter, Instruction inst)
    {
        return inst.returnTypes.Contains(parameter)
            || (parameter == ReturnType.OBJECT && inst.returnTypes.Count > 0)
            || parameter == ReturnType.NONE;
    }

    public virtual void doAction(ProcessContext context)
    {
    }

    public object getReturnObject(ProcessContext context)
    {
        switch (returnTypes[0])
        {
            case ReturnType.BOOL:
                return testCondition(context);
            case ReturnType.NUMBER:
                return instructionToNumber(context);
            case ReturnType.POSITION:
                return instructionToPosition(context);
            case ReturnType.DIRECTION:
                return instructionToDirection(context);
            case ReturnType.ENTITY:
                return instructionToEntity(context);
            case ReturnType.OBJECT:
                return instructionToObject(context);
            default:
                return null;
        }
    }

    public virtual bool testCondition(ProcessContext context)
    {
        return false;
    }

    public virtual float instructionToNumber(ProcessContext context)
    {
        return context.botController.alphabet.IndexOf(
            context.instruction()
            );
    }

    public virtual Vector2 instructionToPosition(ProcessContext context)
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
    public virtual Vector2 instructionToDirection(ProcessContext context)
    {
        return Vector2.zero;
    }

    public virtual Entity instructionToEntity(ProcessContext context)
    {
        return null;
    }

    public virtual object instructionToObject(ProcessContext context)
    {
        return null;
    }
}
