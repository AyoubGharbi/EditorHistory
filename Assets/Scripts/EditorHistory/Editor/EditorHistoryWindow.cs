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

    public GUISkin _hInfoSkin;
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
        var activeObject = Selection.activeObject;
        if (activeObject == null) return;

        DrawHElement (activeObject);

        UpdateHElement(activeObject);

        Repaint ();
    }

    void DrawHistory ()
    {
        // no history
        if (_editorHistory.HistorySize <= 0)
        {
            var infoStyle = _hInfoSkin.GetStyle ("Infos");
            GUILayout.FlexibleSpace ();
            EditorGUILayout.BeginHorizontal ();
            GUILayout.FlexibleSpace ();

            GUILayout.Label ("No History!", infoStyle);

            GUILayout.FlexibleSpace ();
            EditorGUILayout.EndHorizontal ();
            GUILayout.FlexibleSpace ();
        }
        else
        {
            for (int h = 0; h < _editorHistory.HistorySize; h++)
            {
                var hRect = EditorGUILayout.BeginHorizontal ();
                var hObject = _editorHistory.HistoryElements[h];
                var hButtonStyle = _hWindowSkin.GetStyle ("InteractableButton");

                if (IsClickingRect (hRect)) // ping it and remove it from the history list (just for testing atm)
                    UpdateHElement (hObject);

                if (_editorHistory.IsElementSelected (hObject))
                {
                    var hStyle = _hWindowSkin.customStyles[0].onHover.textColor;
                    GUI.contentColor = hStyle;
                }

                GUIContent hContent = new GUIContent ();
                hContent.image = AssetPreview.GetMiniThumbnail (hObject);
                hContent.text = hObject.name;

                GUILayout.Label (hContent, hButtonStyle);

                GUILayout.EndHorizontal ();

                GUILayout.Space (15);

                GUI.contentColor = Color.white;
            }
        }
    }

    void DrawHElement (UnityEngine.Object hObject)
    {
        _editorHistory.AddhEelement (hObject);
        Repaint (); // force repaint
    }

    void UpdateHElement (UnityEngine.Object hObject)
    {
        EditorGUIUtility.PingObject (hObject);

        _editorHistory.UpdateSelection (hObject);

        Repaint (); // force repaint
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