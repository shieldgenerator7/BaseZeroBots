using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Instruction", menuName = "Instruction/Instruction", order = 1)]
public class Instruction : ScriptableObject
{
    public Sprite symbol;
    
    public virtual void doAction(BotController bc, int currentIndex, List<Instruction> instructions)
    {
    }
}
