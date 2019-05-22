using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Key", menuName = "Keys/Key")]
public class Key : ScriptableObject
{
    public KeyCode keyCode;
    public Sprite keySprite;
}
