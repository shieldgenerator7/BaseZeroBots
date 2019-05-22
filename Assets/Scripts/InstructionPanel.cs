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
            dimension = Mathf.CeilToInt(Mathf.Sqrt(value));
            dimension = Mathf.Max(1, dimension);
        }
    }
    
    public List<KeyScheme> keySchemes;

    public BotController target;
    public Instruction defaultInstruction;
    public GameObject keyButtonPrefab;
    public GameObject instructionPrefab;
    public GameObject cursorObject;
    public GameObject selectorPrefabSmall;
    public ColorScheme colorScheme;

    private List<GameObject> highlightFrames = new List<GameObject>();
    private List<KeyButton> keyButtons = new List<KeyButton>();

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
    private int prevCursor = -1;

    public Vector2 offset;
    protected List<SpriteRenderer> instSprites = new List<SpriteRenderer>();

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //Check for adding an instruction
        checkSymbolInput();
        //Check for navigating the grid
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Cursor--;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Cursor++;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Cursor -= dimension;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Cursor += dimension;
        }
        if (Input.GetKeyDown(KeyCode.Tab) && !Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                Cursor--;
                //Shift it backward
                Instruction first = target.instructions[0];
                for (int i = 0; i < target.instructions.Count - 1; i++)
                {
                    target.instructions[i] = target.instructions[i + 1];
                }
                target.instructions[target.instructions.Count - 1] = first;
            }
            else
            {
                Cursor++;
                //Shift it forward
                Instruction last = target.instructions[target.instructions.Count - 1];
                for (int i = target.instructions.Count - 1; i > 0; i--)
                {
                    target.instructions[i] = target.instructions[i - 1];
                }
                target.instructions[0] = last;
            }
        }
        updateDisplay();
    }

    protected virtual void checkSymbolInput()
    {
        //Check for adding an instruction
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool mouseClicked = Input.GetMouseButtonDown(0);
        for (int i = 0; i < keyButtons.Count; i++)
        {
            if (Input.GetKeyDown(keyButtons[i].key.keyCode)
                || (mouseClicked && keyButtons[i].isMouseOver(mousePos)))
            {
                addInstruction(target.alphabet[i]);
                break;
            }
        }
        //Highlight the key buttons
        mouseClicked = Input.GetMouseButton(0);
        foreach (KeyButton kb in keyButtons)
        {
            kb.updateHighlight(mousePos, mouseClicked, colorScheme);
        }
    }

    public bool processClick(Vector2 mousePos)
    {
        int newIndex = posToIndex(mousePos);
        if (newIndex >= 0)
        {
            Cursor = newIndex;
            return true;
        }
        foreach (KeyButton kb in keyButtons)
        {
            if (kb.isMouseOver(mousePos))
            {
                return true;
            }
        }
        return false;
    }

    protected virtual void addInstruction(Instruction inst)
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

    /// <summary>
    /// Returns the index in the instruction list that corresponds with the given world coordinates
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public int posToIndex(Vector2 pos)
    {
        if (!sr.bounds.Contains(pos))
        {
            return -1;
        }
        Vector2 gridPos = pos - (Vector2)transform.position - offset;
        int column = Mathf.RoundToInt(gridPos.x);
        int row = Mathf.RoundToInt(gridPos.y);
        row = dimension - row - 1;
        int index = row * dimension;
        index += column;
        return index;
    }

    protected virtual void updateDisplay()
    {
        Instruction.ProcessedAs[] processMap = target.getInstructionMap();
        for (int i = 0; i < Size; i++)
        {
            instSprites[i].sprite = target.instructions[i].symbol;
            instSprites[i].transform.position = indexToPos(i);
            instSprites[i].color = colorScheme.getColor(processMap[i]);
        }
        if (prevCursor != Cursor)
        {
            //Update prev cursor
            prevCursor = cursor;
            //Update Cursor Object
            cursorObject.transform.position = indexToPos(Cursor);
            cursorObject.GetComponent<SpriteRenderer>().color = colorScheme.selectColor;
            //Highlight direct parameters
            foreach (GameObject go in highlightFrames)
            {
                Destroy(go);
            }
            highlightFrames.Clear();
            if (processMap[Cursor] != Instruction.ProcessedAs.CONSTANT)
            {
                int[] paramIndices = target.instructions[Cursor].getParameterIndices(Cursor, target.instructions);
                for (int i = 0; i < paramIndices.Length; i++)
                {
                    GameObject newHighlight = Instantiate(selectorPrefabSmall);
                    newHighlight.transform.parent = transform;
                    newHighlight.transform.position = indexToPos(paramIndices[i]);
                    newHighlight.GetComponent<SpriteRenderer>().color = colorScheme.parameterHighlightColor;
                    highlightFrames.Add(newHighlight);
                }
            }
        }
    }

    public virtual void changeTarget(BotController bc)
    {
        target = bc;
        if (bc == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            Size = target.instructions.Count;
            GetComponent<SpriteRenderer>().size = Vector2.one * dimension;
            while (target.instructions.Count < Size)
            {
                target.instructions.Add(defaultInstruction);
            }
            //Initialize offset
            float scalar = Mathf.Floor(dimension / 2);
            if (dimension % 2 == 0)
            {
                scalar -= 0.5f;
            }
            offset = Vector2.one * -scalar;
            //Initialize instSprites
            while (instSprites.Count >= Size)
            {
                SpriteRenderer removeSR = instSprites[instSprites.Count - 1];
                instSprites.Remove(removeSR);
                Destroy(removeSR.gameObject);
            }
            int i = 0;
            while (instSprites.Count < Size)
            {
                GameObject instSprite = Instantiate(instructionPrefab);
                instSprite.transform.parent = transform;
                instSprites.Add(instSprite.GetComponent<SpriteRenderer>());
                instSprite.transform.position = indexToPos(i);
                i++;
            }
            prevCursor = -1;
            if (Cursor >= Size)
            {
                Cursor = 0;
            }
            updateDisplay();

            //
            //Show keys
            //
            Vector2 basePosition = indexToPos(target.instructions.Count);
            //Select key scheme
            KeyScheme keyScheme = keySchemes[keySchemes.Count - 1];
            foreach (KeyScheme ks in keySchemes)
            {
                if (ks.amountOfKeys == target.alphabet.Count)
                {
                    keyScheme = ks;
                    break;
                }
            }
            //Remove previous key buttons
            foreach (KeyButton kb in keyButtons)
            {
                Destroy(kb.gameObject);
            }
            keyButtons.Clear();
            //Generate new key buttons
            for (i = 0; i < target.alphabet.Count; i++)
            {
                GameObject keyButton = Instantiate(keyButtonPrefab);
                keyButton.transform.parent = transform;
                keyButton.transform.position =
                    basePosition
                    + new Vector2(
                        i % dimension,
                        -(int)(i / dimension) * 0.5f
                        )
                    ;
                KeyButton kb = keyButton.GetComponent<KeyButton>();
                kb.keySR.sprite = keyScheme.keys[i].keySprite;
                kb.symbolSR.sprite = target.alphabet[i].symbol;
                kb.key = keyScheme.keys[i];
                keyButtons.Add(kb);
            }
        }
    }
}
