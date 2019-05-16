﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PositionOf", menuName = "Instruction/PositionOf", order = 1)]
public class PositionOf : Instruction
{
    public override Vector2 instructionToPosition(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        int[] paramIndices = getParameterIndices(currentIndex, instructions);
        int param1 = paramIndices[0];
        return instructions[param1].
            instructionToEntity(bc, param1, instructions).transform.position;
    }
}
