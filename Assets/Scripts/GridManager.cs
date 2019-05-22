using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public ColorScheme colorScheme;
    public const int LENGTH = 100;
    public const int WIDTH = 100;

    public static GameObject[,] grid = new GameObject[LENGTH, WIDTH];
    public static Dictionary<GameObject, Vector2> objectPositions = new Dictionary<GameObject, Vector2>();

    public static AreaTile[,] areaGrid = new AreaTile[LENGTH, WIDTH];

    private static GridManager instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        FindObjectOfType<LevelManager>().onSceneLoaded += onSceneLoaded;
    }

    public void onSceneLoaded()
    {
        grid = new GameObject[LENGTH, WIDTH];
        areaGrid = new AreaTile[LENGTH, WIDTH];
        //Initialize areaGrid
        foreach (AreaTile at in FindObjectsOfType<AreaTile>())
        {
            Vector2 pos = at.transform.position;
            areaGrid[(int)pos.x, (int)pos.y] = at;
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
        //Area Tile processing
        AreaTile at = obj.GetComponent<AreaTile>();
        if (at)
        {
            AreaTile otherTile = null;
            foreach (AreaTile aTile in FindObjectsOfType<AreaTile>())
            {
                if ((Vector2)aTile.transform.position == moveTo)
                {
                    otherTile = aTile;
                    break;
                }
            }
            AreaTile.AreaType prevType = at.type;
            switch (at.type)
            {
                case AreaTile.AreaType.GROUND:
                    at.type = AreaTile.AreaType.VOID;
                    break;
                case AreaTile.AreaType.WALL:
                case AreaTile.AreaType.TRAP:
                case AreaTile.AreaType.GOAL:
                    at.type = AreaTile.AreaType.GROUND;
                    break;
                case AreaTile.AreaType.VOID:
                    return obj.transform.position;
            }
            at.GetComponent<SpriteRenderer>().color = instance.colorScheme.getColor(at.type);
            if (otherTile != null)
            {
                switch (otherTile.type)
                {
                    case AreaTile.AreaType.GROUND:
                        otherTile.type = prevType;
                        break;
                    case AreaTile.AreaType.WALL:
                    case AreaTile.AreaType.TRAP:
                    case AreaTile.AreaType.GOAL:
                        if (prevType == AreaTile.AreaType.GROUND)
                        {
                            //do nothing
                        }
                        else
                        {

                        }
                        return obj.transform.position;
                    case AreaTile.AreaType.VOID:
                        if (prevType == AreaTile.AreaType.GROUND)
                        {
                            otherTile.type = prevType;
                        }
                        break;
                }
                otherTile.GetComponent<SpriteRenderer>().color = instance.colorScheme.getColor(otherTile.type);
                return obj.transform.position;
            }
            return obj.transform.position;
        }
        //Regular game object processing
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

    /// <summary>
    /// Returns the next object in the given world direction, starting from the position, 
    /// but not including the position itself
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    public static Entity nextObjectInDirection(Vector2 pos, Vector2 dir, bool checkTiles = false)
    {
        if (pos == null || dir == null || dir == Vector2.zero)
        {
            return null;
        }
        dir.Normalize();
        if (dir.magnitude < 1)
        {
            return null;
        }
        pos += dir;
        pos.x = Mathf.Round(pos.x);
        pos.y = Mathf.Round(pos.y);
        while (pos.x >= 0 && pos.x < grid.GetLength(0)
            && pos.y >= 0 && pos.y < grid.GetLength(1))
        {
            GameObject go = objectAtPosition(pos);
            if (go != null)
            {
                return go.GetComponent<Entity>();
            }
            if (checkTiles)
            {
                AreaTile tile = areaGrid[(int)pos.x, (int)pos.y];
                if (tile.type != AreaTile.AreaType.GROUND)
                {
                    return tile;
                }
            }

            pos += dir;
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);
        }
        return null;
    }

    public static void checkAllAreaEffects()
    {
        foreach (BotController bc in FindObjectsOfType<BotController>())
        {
            checkAreaEffect(bc.gameObject, bc.transform.position);
        }
    }

    public static void checkAreaEffect(GameObject go, Vector2 movedTo)
    {
        switch (areaGrid[(int)movedTo.x, (int)movedTo.y].type)
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
