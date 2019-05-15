using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveToPosition", menuName = "Instruction/MoveToPosition", order = 1)]
public class MoveToPosition : Instruction
{
    public override void doAction(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        Vector2 position = getParameter(1, currentIndex, instructions).
            instructionToVector2(bc, currentIndex + 1, instructions);
        bc.moveDirection = position - (Vector2)bc.transform.position;
    }
}
