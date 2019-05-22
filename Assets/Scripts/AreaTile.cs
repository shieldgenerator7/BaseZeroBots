using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTile : Entity
{
    public enum AreaType
    {
        GROUND,
        WALL,
        VOID,
        TRAP,
        GOAL,
        GROUND_ELECTRIC
    }
    public AreaType type;

    public SpriteRenderer overlaySR;

    public override string getTypeString()
    {
        return base.getTypeString() + "." + type;
    }
}
