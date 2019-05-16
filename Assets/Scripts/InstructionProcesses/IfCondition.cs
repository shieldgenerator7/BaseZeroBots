using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IfCondition", menuName = "Instruction/IfCondition", order = 1)]
public class IfCondition : Instruction
{

    public override void doAction(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        int[] paramIndices = getParameterIndices(currentIndex, instructions);
        if (testCondition(bc, currentIndex, instructions))
        {
            int param2 = paramIndices[1];
            instructions[param2]
               .doAction(bc, param2, instructions);
        }
        else
        {
            int param3 = paramIndices[2];
            instructions[param3]
                .doAction(bc, param3, instructions);
        }
    }

    public override bool testCondition(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        int[] paramIndices = getParameterIndices(currentIndex, instructions);
        int param1 = paramIndices[0];
        return instructions[param1].testCondition(bc, param1, instructions);
    }
}
