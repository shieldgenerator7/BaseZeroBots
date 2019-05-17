using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public virtual string getTypeString()
    {
        return GetType().Name;
    }
}
