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

    public GUISkin _hInfoSkin = null;
    public GUISkin _hWindowSkin = null;

    public Texture2D FavoriteActiveIcon;
    public Texture2D FavoriteInActiveIcon;

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

        UpdateHElement (activeObject);

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
                var hObject = _editorHistory.HistoryElementFromIndex (h);
                var hButtonStyle = _hWindowSkin.GetStyle ("InteractableButton");

                if (_editorHistory.IsElementSelected (hObject))
                {
                    var hStyle = _hWindowSkin.customStyles[0].onHover.textColor;
                    GUI.color = hStyle;
                }

                Texture2D _finalFavoriteState = null;
                _finalFavoriteState = _editorHistory.IsHistoryFavorite (hObject) ? FavoriteActiveIcon : FavoriteInActiveIcon;

                if (IsClickingRect (hRect, out var mouseId)) // ping it and remove it from the history list (just for testing atm)
                {
                    if (mouseId == 0)
                        UpdateHElement (hObject);
                    else if (mouseId == 1)
                        RemoveHElement (hObject);
                }

                // background
                GUI.Label (hRect, new GUIContent (), hButtonStyle);

                GUIContent hContent = new GUIContent (hObject.name, AssetPreview.GetMiniThumbnail (hObject));

                GUILayout.Label (hContent);

                var favRect = new Rect (hRect.x + hRect.width - 32, hRect.y - hRect.height / 2 - 32, 32, 32);

                if (IsClickingRect (favRect, out var Id))
                {
                    _editorHistory.AddFavEelement (hObject);
                    Repaint();
                }

                GUI.Label (favRect, _finalFavoriteState);
                Debug.Log ("Draw Number : " + h);

                GUILayout.EndHorizontal ();

                GUILayout.Space (15);

                GUI.color = Color.white;
            }
        }
    }

    private void RemoveHElement (UnityEngine.Object hObject)
    {
        _editorHistory.RemovehEelement (hObject);
        Repaint (); // force repaint
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

    bool IsClickingRect (Rect rect, out int mouseId)
    {
        var currentEvent = Event.current;

        switch (currentEvent.type)
        {
            case EventType.MouseDown:
                mouseId = currentEvent.button;
                return rect.Contains (currentEvent.mousePosition);

            default:
                mouseId = -1;
                return false;
        }
    }
}