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
        ENTITY
    }
    public ReturnType returnType = ReturnType.NONE;

    public int[] getParameterIndices(int currentIndex, List<Instruction> instructions)
    {
        int[] paramIndices = new int[parameters.Count];
        int paramIndex = currentIndex + 1;
        for (int i = 0; i < parameters.Count; i++)
        {
            paramIndex = (int)Mathf.Repeat(paramIndex, instructions.Count);
            paramIndices[i] = paramIndex;
            if (instructions[paramIndex].returnType == parameters[i])
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
            if (instructions[lastIndex].returnType == parameters[i]
                || parameters[i] == ReturnType.NONE)
            {
                lastIndex = instructions[lastIndex].getLastParameterIndex(lastIndex, instructions);
            }
            lastIndex++;
        }
        lastIndex--;
        lastIndex = (int)Mathf.Repeat(lastIndex, instructions.Count);
        return lastIndex;
    }

    public virtual void doAction(BotController bc, int currentIndex, List<Instruction> instructions)
    {
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

    public virtual BotController instructionToEntity(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        return null;
    }
}
