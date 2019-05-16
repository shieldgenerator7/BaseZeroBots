using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DistanceBetween", menuName = "Instruction/DistanceBetween", order = 1)]
public class DistanceBetween : Instruction
{
    public override float instructionToNumber(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        int[] paramIndices = getParameterIndices(currentIndex, instructions);
        int param1 = paramIndices[0];
        int param2 = paramIndices[1];
        return Vector3.Distance(
            instructions[param1].
                instructionToPosition(bc, param1, instructions),
            instructions[param2].
                instructionToPosition(bc, param2, instructions)
            );
    }
}
