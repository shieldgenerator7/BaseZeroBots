using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PositionOf", menuName = "Instruction/PositionOf", order = 1)]
public class PositionOf : Instruction
{
    public override Vector2 instructionToVector2(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        return getParameter(1, currentIndex, instructions).
            instructionToEntity(bc, currentIndex + 1, instructions).transform.position;
    }
}
