using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Comparator", menuName = "Instruction/Comparator", order = 1)]
public class Comparator : Instruction
{
    [Header("Comparators")]
    public bool lessThan;
    public bool greaterThan;
    public bool equalTo;

    public override void doAction(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        //Can't compare if there's not enough instructions left
        if (currentIndex >= instructions.Count - 4)
        {
            return;
        }
        if (testCondition(bc, currentIndex, instructions))
        {
            instructions[currentIndex + 3]
                .doAction(bc, currentIndex + 3, instructions);
        }
        else
        {
            instructions[currentIndex + 4]
                .doAction(bc, currentIndex + 4, instructions);
        }
    }

    public override bool testCondition(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        //Can't compare if there's not enough instructions left
        if (currentIndex >= instructions.Count - 2)
        {
            return false;
        }
        //Process the comparator
        float and1 = instructions[currentIndex + 1].instructionToNumber(bc, currentIndex + 1, instructions);
        float and2 = instructions[currentIndex + 2].instructionToNumber(bc, currentIndex + 2, instructions);
        if (lessThan)
        {
            if (and1 < and2)
            {
                return true;
            }
        }
        if (greaterThan)
        {
            if (and1 > and2)
            {
                return true;
            }
        }
        if (equalTo)
        {
            if (and1 == and2)
            {
                return true;
            }
        }
        return false;
    }
}
