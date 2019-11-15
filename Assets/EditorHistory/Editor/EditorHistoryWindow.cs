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

    private EditorHistory _editorHistory = new EditorHistory ();

    void OnEnable ()
    {
        Selection.selectionChanged += OnSelectionChanged;
    }

    void OnDisable ()
    {
        Selection.selectionChanged -= OnSelectionChanged;
    }

    void OnGUI ()
    {
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
            var hRect = EditorGUILayout.BeginHorizontal ();
            var hObject = _editorHistory.HistoryElements[h];
            var buttonStyle = _hWindowSkin.GetStyle ("InteractableButton");

            GUILayout.Label (hObject.name, buttonStyle);

            if (IsClickingRect (hRect))
                EditorGUIUtility.PingObject (hObject);

            GUILayout.EndHorizontal ();

            GUILayout.Space (15);
        }
    }

    void DrawHElement (UnityEngine.Object hObject)
    {
        _editorHistory.AddhEelement (hObject);
    }

    bool IsClickingRect (Rect rect)
    {
        var currentEvent = Event.current;

        switch (currentEvent.type)
        {
            case EventType.MouseDown:
                return rect.Contains (currentEvent.mousePosition);

        }

        return false;
    }
}