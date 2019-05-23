using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DistanceBetween", menuName = "Instruction/DistanceBetween", order = 1)]
public class DistanceBetween : Instruction
{
    public override float instructionToNumber(ProcessContext context)
    {
        int[] paramIndices = getParameterIndices(context);
        int param1 = paramIndices[0];
        int param2 = paramIndices[1];
        return Vector3.Distance(
            context.instruction(param1).
                instructionToPosition(context.context(param1)),
            context.instruction(param2).
                instructionToPosition(context.context(param2))
            );
    }
}
