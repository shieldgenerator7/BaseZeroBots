using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveToPosition", menuName = "Instruction/MoveToPosition", order = 1)]
public class MoveToPosition : Instruction
{
    public override void doAction(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        int param1 = getParameterIndex(1, currentIndex, instructions);
        Vector2 position = instructions[param1].
            instructionToPosition(bc, param1, instructions);
        bc.moveDirection = position - (Vector2)bc.transform.position;
    }
}
