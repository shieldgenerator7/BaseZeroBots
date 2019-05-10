using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Comparator", menuName = "Instruction/Comparator", order = 1)]
public class Comparator : Instruction
{
    public enum Comparison
    {
        LESS_THAN,
        GREATER_THAN,
        NEITHER
    }
    public Comparison comparison;
    public bool orEqualTo;
}
