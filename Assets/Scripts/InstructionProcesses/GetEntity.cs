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

    public override BotController instructionToEntity(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        if (option == Option.SELF)
        {
            return bc;
        }
        float extreme = (option == Option.CLOSEST) ? float.MaxValue : 0;
        BotController target = null;
        foreach (BotController fbc in FindObjectsOfType<BotController>())
        {
            if (bc != fbc)
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
