using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Instruction", menuName = "Instruction/Instruction", order = 1)]
public class Instruction : ScriptableObject
{
    public Sprite symbol;
    public string name;

    public bool lifeInstruction = true;//true: this instruction allows for the bot to be alive

    public bool command = true;//true: can be used as a command
    public bool condition = true;//true: can be used as a condition

    public virtual void doAction(BotController bc, int currentIndex, List<Instruction> instructions)
    {
    }

    public virtual bool testCondition(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        return false;
    }
}
