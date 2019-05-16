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

    //public override void doAction(BotController bc, int currentIndex, List<Instruction> instructions)
    //{
    //    int[] paramIndices = getParameterIndices(currentIndex, instructions);
    //    if (testCondition(bc, currentIndex, instructions))
    //    {
    //        int param3 = paramIndices[2];
    //        instructions[param3]
    //           .doAction(bc, param3, instructions);
    //    }
    //    else
    //    {
    //        int param4 = paramIndices[3];
    //        instructions[param4]
    //            .doAction(bc, param4, instructions);
    //    }
    //}

    public override bool testCondition(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        int[] paramIndices = getParameterIndices(currentIndex, instructions);
        //Process the comparator
        int param1 = paramIndices[0];
        float and1 = instructions[param1].instructionToNumber(bc, param1, instructions);
        int param2 = paramIndices[1];
        float and2 = instructions[param2].instructionToNumber(bc, param2, instructions);
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
