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
        int param1 = getParameterIndex(1, currentIndex, instructions);
        Vector3 position = instructions[param1].
            instructionToPosition(bc, param1, instructions);
        switch (option)
        {
            case Option.TOWARDS:
                bc.moveDirection += (position - bc.transform.position).normalized;
                break;
            case Option.AWAY:
                bc.moveDirection += (bc.transform.position - position).normalized;
                break;
        }
    }
}
