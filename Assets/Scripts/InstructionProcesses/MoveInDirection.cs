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

    public override void doAction(ProcessContext context)
    {
        Vector3 pos = instructionToDirection(context);
        context.botController.moveDirection += pos;
    }

    public override bool testCondition(ProcessContext context)
    {
        Vector3 pos = instructionToDirection(context);
        pos = context.botController.transform.TransformDirection(pos);
        return GridManager.objectAtPosition(context.botController.transform.position + pos) == null;
    }

    public override Vector2 instructionToDirection(ProcessContext context)
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
