using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphabetPanel : InstructionPanel
{
    public List<Instruction> alphabet;

    protected override void checkSymbolInput()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool mouseClicked = Input.GetMouseButtonDown(0);
        foreach (Instruction inst in alphabet)
        {
            if (Input.GetKeyDown(inst.keyCode))
            {
                addInstruction(inst);
                break;
            }
        }
    }

    protected override void addInstruction(Instruction inst)
    {
        target.alphabet[Cursor] = inst;
        Cursor++;
    }

    protected override void updateDisplay()
    {
        //Show instructions in alphabet
        for (int i = 0; i < Size; i++)
        {
            if (target.alphabet[i])
            {
                instSprites[i].sprite = target.alphabet[i].symbol;
            }
            instSprites[i].transform.position = indexToPos(i);
            instSprites[i].color = colorScheme.commandColor;
        }
        //Update Cursor Object
        cursorObject.transform.position = indexToPos(Cursor);
        cursorObject.GetComponent<SpriteRenderer>().color = colorScheme.selectColor;
    }

    public override void changeTarget(BotController bc)
    {
        target = bc;
        if (bc == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            Size = target.alphabet.Count;
            GetComponent<SpriteRenderer>().size = Vector2.one * dimension;
            while (target.alphabet.Count < Size)
            {
                target.alphabet.Add(defaultInstruction);
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
            if (Cursor >= Size)
            {
                Cursor = 0;
            }
            updateDisplay();            
        }
    }
}
