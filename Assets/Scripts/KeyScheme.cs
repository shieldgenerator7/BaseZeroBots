using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyScheme", menuName = "Keys/KeyScheme")]
public class KeyScheme : ScriptableObject
{
    public int amountOfKeys;
    public List<Key> keys;
}
