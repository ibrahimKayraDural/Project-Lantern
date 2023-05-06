using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SetWallCollision))]
public class CustomWallCollisionScript : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SetWallCollision SWC = (SetWallCollision)target;

        if (GUILayout.Button("Refresh"))
        {
            SWC.Refresh();
        }
    }
}
