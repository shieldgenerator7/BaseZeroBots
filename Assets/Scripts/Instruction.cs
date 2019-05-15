﻿using System.Collections;
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

    /// <summary>
    /// Returns the instruction that will serve as the parameter for this instruction
    /// </summary>
    /// <param name="index">Base 0, starting from the current index</param>
    /// <returns></returns>
    public int getParameterIndex(int index, int currentIndex, List<Instruction> instructions)
    {
        int paramIndex = (int)Mathf.Repeat(currentIndex + 1, instructions.Count);
        for (int i = 1; i < index; i++)
        {
            paramIndex = instructions[paramIndex].getLastParameterIndex(paramIndex, instructions) + 1;
            paramIndex = (int)Mathf.Repeat(paramIndex, instructions.Count);
        }
        return paramIndex;
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
                //lastIndex = instructions[lastIndex].getLastParameterIndex(lastIndex, instructions);
            }
            lastIndex++;
        }
        lastIndex--;
        lastIndex = (int)Mathf.Repeat(lastIndex, instructions.Count);
        Debug.Log("gLPI: cur: " + currentIndex + ", last: " + lastIndex + ", inst: " + name);
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
