using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GetEntity", menuName = "Instruction/GetEntity", order = 1)]
public class GetEntity : Instruction
{
    public enum Option
    {
        CLOSEST,
        FURTHEST,
        SELF,
        IN_DIRECTION
    }
    public Option option;

    public enum EntityType
    {
        BOT,
        WALL,
        GOAL
    }
    public EntityType entityType;

    public override void doAction(ProcessContext context)
    {
        instructionToEntity(context);
    }

    public override Entity instructionToEntity(ProcessContext context)
    {
        if (option == Option.SELF)
        {
            return context.botController;
        }
        if (option == Option.IN_DIRECTION)
        {
            int[] paramIndices = getParameterIndices(context);
            int param1 = paramIndices[0];
            Vector2 direction = context.instruction(param1)
                .instructionToDirection(context.context(param1));
            Entity entity = GridManager.nextObjectInDirection(
                context.botController.transform.position,
                context.botController.transform.TransformDirection(direction),
                true
                );
            return entity;
        }
        //Get the target type string
        string targetTypeString = typeof(BotController).Name;
        if (entityType != EntityType.BOT)
        {
            targetTypeString = typeof(AreaTile).Name + "." + entityType;
        }
        //
        float extreme = (option == Option.CLOSEST) ? float.MaxValue : 0;
        Entity target = null;
        foreach (Entity fbc in FindObjectsOfType<Entity>())
        {
            if (fbc != context.botController
                && fbc.getTypeString() == targetTypeString)
            {
                float distance = Vector3.Distance(
                    context.botController.transform.position,
                    fbc.transform.position
                    );
                switch (option)
                {
                    case Option.CLOSEST:
                        if (distance < extreme)
                        {
                            extreme = distance;
                            target = fbc;
                        }
                        break;
                    case Option.FURTHEST:
                        if (distance > extreme)
                        {
                            extreme = distance;
                            target = fbc;
                        }
                        break;
                }
            }
        }
        return target;
    }

    public override Vector2 instructionToPosition(ProcessContext context)
    {
        Entity entity = instructionToEntity(context);
        if (entity)
        {
            return entity.transform.position;
        }
        return base.instructionToPosition(context);
    }

    public override Vector2 instructionToDirection(ProcessContext context)
    {
        Entity entity = instructionToEntity(context);
        if (entity)
        {
            return entity.transform.up;
        }
        return base.instructionToPosition(context);
    }
}
