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
}
