using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Instruction", menuName = "Instruction/Instruction", order = 1)]
public class Instruction : ScriptableObject
{
    public Sprite symbol;
    public new string name;
    public int parameters = 1;
    public KeyCode keyCode;

    public bool lifeInstruction = true;//true: this instruction allows for the bot to be alive

    public bool command = true;//true: can be used as a command
    public bool query = true;//true: can be used as a condition

    /// <summary>
    /// Returns the instruction that will serve as the parameter for this instruction
    /// </summary>
    /// <param name="index">Base 0, starting from the current index</param>
    /// <returns></returns>
    public Instruction getParameter(int index, int currentIndex, List<Instruction> instructions)
    {
        int paramIndex = (int)Mathf.Repeat(index + currentIndex, instructions.Count);
        return instructions[paramIndex];
    }

    public virtual void doAction(BotController bc, int currentIndex, List<Instruction> instructions)
    {
    }

    public virtual bool testCondition(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        return false;
    }

    public virtual int instructionToNumber(BotController bc, Instruction inst)
    {
        return bc.alphabet.IndexOf(inst);
    }

    public virtual Vector2 instructionToVector2(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        return Vector2.zero;
    }

    public virtual BotController instructionToEntity(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        return null;
    }
}
