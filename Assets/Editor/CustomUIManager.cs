using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIManager))]
public class CustomUIManager : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UIManager uim = (UIManager)target;

        if (GUILayout.Button("Refresh"))
        {
            uim.Refresh();
        }
    }
}
