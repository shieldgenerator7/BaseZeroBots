using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rotate", menuName = "Instruction/Rotate", order = 1)]
public class Rotate : Instruction
{
    public float angle = 90;

    public override void doAction(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        bc.transform.Rotate(-Vector3.forward, angle);
    }
}
