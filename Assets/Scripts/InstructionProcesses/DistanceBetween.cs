using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DistanceBetween", menuName = "Instruction/DistanceBetween", order = 1)]
public class DistanceBetween : Instruction
{
    public override float instructionToNumber(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        return Vector3.Distance(
            getParameter(1, currentIndex, instructions).
                instructionToVector2(bc, currentIndex + 1, instructions),
            getParameter(2, currentIndex, instructions).
                instructionToVector2(bc, currentIndex + 2, instructions)
            );
    }
}
