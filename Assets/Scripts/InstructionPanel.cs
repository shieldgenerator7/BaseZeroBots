using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionPanel : MonoBehaviour
{
    public int dimension = 5;//it's going to be a square, so this is the dimension of both height and width

    public List<Instruction> alphabet;

    public BotController target;
    public Instruction defaultInstruction;

    [SerializeField]
    private int cursor = 0;//where the next letter is going to be
    public int Cursor
    {
        get { return cursor; }
        set
        {
            cursor = (int)Mathf.Repeat(value, dimension * dimension);
        }
    }

    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().size = Vector2.one * dimension;
        while (target.instructions.Count < dimension * dimension)
        {
            target.instructions.Add(defaultInstruction);
        }
    }

    private void Update()
    {
        foreach (Instruction inst in alphabet)
        {
            if (Input.GetKeyDown(inst.keyCode))
            {
                addInstruction(inst);
                break;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Cursor--;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Cursor++;
        }
    }

    private void addInstruction(Instruction inst)
    {
        target.instructions[cursor] = inst;
        Cursor++;
    }
}
