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
}
