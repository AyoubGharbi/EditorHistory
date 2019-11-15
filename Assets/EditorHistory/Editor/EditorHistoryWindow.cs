using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorHistoryWindow : EditorWindow
{
    private static EditorHistoryWindow _hWindow = null;

    [MenuItem ("Window/GhAyoub/Editor History _F1")]
    static void OpenhWindow ()
    {
        _hWindow = (EditorHistoryWindow) EditorWindow.GetWindow (typeof (EditorHistoryWindow),
            utility : false, "Editor History", focus : true);
    }

    public GUISkin _hWindowSkin;

    private bool _isInitialized = false;
    private EditorHistory _editorHistory = new EditorHistory ();

    void OnGUI ()
    {
        if (!_isInitialized)
        {
            Selection.selectionChanged += OnSelectionChanged;
            _isInitialized = true;
        }

        EditorGUILayout.BeginHorizontal ();

        GUILayout.Space (15);

        var rect = EditorGUILayout.BeginVertical ();

        DrawHistory ();

        EditorGUILayout.EndVertical ();

        GUILayout.Space (15);

        EditorGUILayout.EndHorizontal ();

    }

    private void OnSelectionChanged ()
    {
        var activeObject = Selection.activeGameObject;
        if (activeObject == null) return;

        DrawHElement (activeObject);
    }

    void DrawHistory ()
    {
        for (int h = 0; h < _editorHistory.HistorySize; h++)
        {
            Debug.Log ("Draw!");
            var hObject = _editorHistory.HistoryElements[h];
            var buttonStyle = _hWindowSkin.GetStyle ("InteractableButton");
            GUILayout.Label (hObject.name, buttonStyle);

            GUILayout.Space (15);
        }
    }

    void DrawHElement (UnityEngine.Object hObject)
    {
        _editorHistory.AddhEelement (hObject);
    }
}