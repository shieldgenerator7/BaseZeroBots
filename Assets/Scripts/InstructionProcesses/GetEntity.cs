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
        SELF
    }
    public Option option;

    public enum EntityType
    {
        BOT,
        WALL,
        GOAL
    }
    public EntityType entityType;

    public override Entity instructionToEntity(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        if (option == Option.SELF)
        {
            return bc;
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
            if (fbc != bc && fbc.getTypeString() == targetTypeString)
            {
                float distance = Vector3.Distance(bc.transform.position, fbc.transform.position);
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
}
