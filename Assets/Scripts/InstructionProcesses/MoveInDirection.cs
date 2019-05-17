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
        Vector3 pos = instructionToDirection(bc, currentIndex, instructions);
        bc.moveDirection += pos;
    }

    public override bool testCondition(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        Vector3 pos = instructionToDirection(bc, currentIndex, instructions);
        pos = bc.transform.TransformDirection(pos);
        return GridManager.objectAtPosition(bc.transform.position + pos) == null;
    }

    public override Vector2 instructionToDirection(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        switch (direction)
        {
            case Direction.FORWARD:
                return Vector2.up;
            case Direction.BACKWARD:
                return Vector2.down;
            case Direction.LEFT:
                return Vector2.left;
            case Direction.RIGHT:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }
}
