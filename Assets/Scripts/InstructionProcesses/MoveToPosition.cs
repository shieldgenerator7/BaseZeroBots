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

    public override void doAction(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        int[] paramIndices = getParameterIndices(currentIndex, instructions);
        int param1 = paramIndices[0];
        Vector3 position = instructions[param1].
            instructionToPosition(bc, param1, instructions);
        Vector2 moveDir = Vector2.zero;
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
