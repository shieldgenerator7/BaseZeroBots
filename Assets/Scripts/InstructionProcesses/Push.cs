using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Push", menuName = "Instruction/Push", order = 1)]
public class Push : Instruction
{
    
    public override void doAction(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        int[] paramIndices = getParameterIndices(currentIndex, instructions);
        int param1 = paramIndices[0];
        Vector2 direction = instructions[param1].instructionToDirection(bc, param1, instructions);
        direction = bc.transform.TransformDirection(direction);
        Entity entity = GridManager.nextObjectInDirection(
            bc.transform.position,
            direction,
            true
            );
        if (!entity)
        {
            return;
        }
        //If object is within range,
        if (Vector2.Distance(bc.transform.position, entity.transform.position) <= 1)
        {
            //Push it
            Vector2 newPos = GridManager.moveObject(entity.gameObject, (Vector2)entity.transform.position + direction);
            if (entity is BotController)
            {
                entity.transform.position = newPos;
            }
        }
    }
}
