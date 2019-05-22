using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyButton : MonoBehaviour
{
    public SpriteRenderer keySR;
    public SpriteRenderer symbolSR;
    public Key key;

    private SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public bool isMouseOver(Vector2 mousePos)
    {
        return sr.bounds.Contains(mousePos);
    }

    public void updateHighlight(Vector2 mousePos, bool mouseClicked, ColorScheme colorScheme)
    {
        if (isMouseOver(mousePos))
        {
            if (mouseClicked)
            {
                updateColor(colorScheme.clickedColor);
            }
            else
            {
                updateColor(colorScheme.hoverColor);
            }
        }
        else
        {
            updateColor(colorScheme.idleColor);
        }
        if (Input.GetKey(key.keyCode))
        {
            updateColor(colorScheme.clickedColor);
        }
    }

    public void updateColor(Color c)
    {
        keySR.color = c;
        symbolSR.color = c;
    }
}
