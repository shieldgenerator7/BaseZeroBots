using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public ColorScheme colorScheme;

    public static GameObject[,] grid = new GameObject[100, 100];
    public static Dictionary<GameObject, Vector2> objectPositions = new Dictionary<GameObject, Vector2>();

    public static AreaTile.AreaType[,] areaGrid = new AreaTile.AreaType[100, 100];
   
    private static GridManager instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //Initialize areaGrid
        foreach (AreaTile at in FindObjectsOfType<AreaTile>())
        {
            Vector2 pos = at.transform.position;
            areaGrid[(int)pos.x, (int)pos.y] = at.type;
            at.GetComponent<SpriteRenderer>().color = colorScheme.getColor(at.type);
            if (at.type == AreaTile.AreaType.WALL)
            {
                grid[(int)pos.x, (int)pos.y] = at.gameObject;
            }
        }
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

    public static void checkAllAreaEffects()
    {
        foreach(BotController bc in FindObjectsOfType<BotController>())
        {
            checkAreaEffect(bc.gameObject, bc.transform.position);
        }
    }

    public static void checkAreaEffect(GameObject go, Vector2 movedTo)
    {
        switch (areaGrid[(int)movedTo.x, (int)movedTo.y])
        {
            case AreaTile.AreaType.GROUND:
            case AreaTile.AreaType.WALL:
                break;
            case AreaTile.AreaType.VOID:
                Destroy(go);
                break;
            case AreaTile.AreaType.TRAP:
                go.GetComponent<BotController>().damage(1);
                break;
            case AreaTile.AreaType.GOAL:
                go.GetComponent<SpriteRenderer>().color = Color.green;
                break;
        }
    }
}
