using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AreaTile))]
public class AreaTileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Update All Visuals"))
        {
            ColorScheme cs = FindObjectOfType<GridManager>().colorScheme;
            foreach(AreaTile at in FindObjectsOfType<AreaTile>())
            {
                at.GetComponent<SpriteRenderer>().color = cs.getColor(at.type);
            }
        }
    }
}
