using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IfCondition", menuName = "Instruction/IfCondition", order = 1)]
public class IfCondition : Instruction
{

    public override void doAction(ProcessContext context)
    {
        int[] paramIndices = getParameterIndices(context);
        if (testCondition(context))
        {
            int param2 = paramIndices[1];
            context.instruction(param2)
               .doAction(context.context(param2));
        }
        else
        {
            int param3 = paramIndices[2];
            context.instruction(param3)
                .doAction(context.context(param3));
        }
    }

    public override bool testCondition(ProcessContext context)
    {
        int[] paramIndices = getParameterIndices(context);
        int param1 = paramIndices[0];
        return context.instruction(param1)
            .testCondition(context.context(param1));
    }
}
