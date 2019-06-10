using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelManager))]
[CanEditMultipleObjects]
public class LevelManagerEditor : Editor
{

#if UNITY_EDITOR
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Fill Level List from Build Settings"))
        {
            ((LevelManager)target).fillLevelsArray();
        }
    }
#endif
}
