using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PositionOf", menuName = "Instruction/PositionOf", order = 1)]
public class PositionOf : Instruction
{
    public override Vector2 instructionToPosition(ProcessContext context)
    {
        int[] paramIndices = getParameterIndices(context);
        int param1 = paramIndices[0];
        Entity entity = context.instruction(param1)
            .instructionToEntity(context.context(param1));
        if (entity)
        {
            return entity.transform.position;
        }
        return base.instructionToPosition(context);
    }
}
