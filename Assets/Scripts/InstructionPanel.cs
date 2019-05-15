using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionPanel : MonoBehaviour
{
    public int dimension = 5;//it's going to be a square, so this is the dimension of both height and width
    public int Size
    {
        get { return dimension * dimension; }
        set
        {
            dimension = Mathf.FloorToInt(Mathf.Sqrt(value));
        }
    }

    public List<Instruction> alphabet;

    public BotController target;
    public Instruction defaultInstruction;
    public GameObject instructionPrefab;

    [SerializeField]
    private int cursor = 0;//where the next letter is going to be
    public int Cursor
    {
        get { return cursor; }
        set
        {
            cursor = (int)Mathf.Repeat(value, Size);
        }
    }

    public Vector2 offset;
    private List<SpriteRenderer> instSprites;

    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().size = Vector2.one * dimension;
        while (target.instructions.Count < Size)
        {
            target.instructions.Add(defaultInstruction);
        }
        //Initialize offset
        offset = Vector2.one * -Mathf.Floor(dimension / 2);
        //Initialize instSprites
        instSprites = new List<SpriteRenderer>();
        int i = 0;
        while (instSprites.Count < Size)
        {
            GameObject instSprite = Instantiate(instructionPrefab);
            instSprite.transform.parent = transform;
            instSprites.Add(instSprite.GetComponent<SpriteRenderer>());
            instSprite.transform.position = indexToPos(i);
            i++;
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
        updateDisplay();
    }

    private void addInstruction(Instruction inst)
    {
        target.instructions[cursor] = inst;
        Cursor++;
    }

    /// <summary>
    /// Returns the screen position of the instruction at the given index
    /// </summary>
    /// <param name="index"></param>
    /// <remarks>index should be at least 0 and less than Size</remarks>
    /// <returns></returns>
    public Vector2 indexToPos(int index)
    {
        int column = index % dimension;
        int row = Mathf.FloorToInt(index / dimension);
        //flip row
        row = dimension - row - 1;
        Vector2 gridPos = new Vector2(column, row);
        return (Vector2)transform.position + gridPos + offset;
    }

    private void updateDisplay()
    {
        for (int i = 0; i < Size; i++)
        {
            instSprites[i].sprite = target.instructions[i].symbol;
            instSprites[i].transform.position = indexToPos(i);
        }
    }
}
