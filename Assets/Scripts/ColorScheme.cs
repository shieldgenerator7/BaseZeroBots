using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ColorScheme", menuName ="ColorScheme")]
public class ColorScheme : ScriptableObject
{
    [Header("Selectors")]
    public Color selectColor = Color.white;
    public Color parameterHighlightColor = Color.white;

    [Header("Instructions")]
    public Color commandColor = Color.white;
    public Color queryColor = Color.white;
    public Color constantColor = Color.white;
    public Color doNothingColor = Color.white;

    [Header("Tiles")]
    public Color groundColor = Color.white;
    public Color wallColor = Color.white;
    public Color voidColor = Color.white;
    public Color trapColor = Color.white;
    public Color goalColor = Color.white;

    public Color getColor(Instruction.ProcessedAs pa)
    {
        switch (pa)
        {
            case Instruction.ProcessedAs.COMMAND:
                return commandColor;
            case Instruction.ProcessedAs.CONSTANT:
                return constantColor;
            case Instruction.ProcessedAs.DO_NOTHING:
                return doNothingColor;
            case Instruction.ProcessedAs.QUERY:
                return queryColor;
            default:
                return doNothingColor;
        }
    }

    public Color getColor(AreaTile.AreaType areaType)
    {
        switch (areaType)
        {
            case AreaTile.AreaType.GROUND:
                return groundColor;
            case AreaTile.AreaType.WALL:
                return wallColor;
            case AreaTile.AreaType.VOID:
                return voidColor;
            case AreaTile.AreaType.TRAP:
                return trapColor;
            case AreaTile.AreaType.GOAL:
                return goalColor;
            default:
                return voidColor;
        }
    }
}
