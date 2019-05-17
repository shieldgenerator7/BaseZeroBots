using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTile : MonoBehaviour
{
    public enum AreaType
    {
        GROUND,
        WALL,
        VOID,
        TRAP,
        GOAL
    }
    public AreaType type;
}
