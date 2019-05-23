using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveToPosition", menuName = "Instruction/MoveToPosition", order = 1)]
public class MoveToPosition : Instruction
{

    public enum Option
    {
        TOWARDS,
        AWAY
    }
    public Option option;

    public override void doAction(ProcessContext context)
    {
        int[] paramIndices = getParameterIndices(context);
        int param1 = paramIndices[0];
        Vector3 position = context.instruction(param1).
            instructionToPosition(context.context(param1));
        Vector2 moveDir = Vector2.zero;
        BotController bc = context.botController;
        switch (option)
        {
            case Option.TOWARDS:
                moveDir = (position - bc.transform.position).normalized;
                break;
            case Option.AWAY:
                moveDir = (bc.transform.position - position).normalized;
                break;
        }
        bc.moveDirection += bc.transform.InverseTransformDirection(moveDir);
    }
}
