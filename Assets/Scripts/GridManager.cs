using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public static GameObject[,] grid = new GameObject[100,100];
    public static Dictionary<GameObject, Vector2> objectPositions = new Dictionary<GameObject, Vector2>();

    private static GridManager instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public static void registerObject(GameObject obj, Vector2 pos)
    {
        objectPositions.Add(obj, pos);
    }

    public static Vector2 moveObject(GameObject obj, Vector2 moveTo)
    {
        if (!objectPositions.ContainsKey(obj))
        {
            registerObject(obj, obj.transform.position);
        }
        Vector2 oldPos = objectPositions[obj];
        if (grid[(int)moveTo.x, (int)moveTo.y] == null)
        {
            grid[(int)oldPos.x, (int)oldPos.y] = null;
            grid[(int)moveTo.x, (int)moveTo.y] = obj;
            objectPositions[obj] = moveTo;
            return moveTo;
        }
        return oldPos;
    }
    
    public static GameObject objectAtPosition(Vector2 pos)
    {
        return grid[(int)pos.x, (int)pos.y];
    }
}
