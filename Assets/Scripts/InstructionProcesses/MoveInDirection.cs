using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveInDirection", menuName = "Instruction/MoveInDirection", order = 1)]
public class MoveInDirection : Instruction
{
    public enum Direction
    {
        FORWARD,
        BACKWARD,
        LEFT,
        RIGHT
    }
    public Direction direction;

    public override void doAction(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        Vector3 pos = Vector3.zero;
        switch (direction)
        {
            case Direction.FORWARD: pos.y += 1; break;
            case Direction.BACKWARD: pos.y -= 1; break;
            case Direction.LEFT: pos.x -= 1; break;
            case Direction.RIGHT: pos.x += 1; break;
        }
        pos = bc.transform.TransformDirection(pos);
        bc.transform.position = GridManager.moveObject(bc.gameObject, bc.transform.position + pos);
    }
}
