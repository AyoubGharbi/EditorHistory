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
    private Vector2 _scrollPos = Vector2.zero;

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
        _scrollPos = EditorGUILayout.BeginScrollView (_scrollPos);

        DrawHistory ();

        EditorGUILayout.EndScrollView ();
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
                var hObject = _editorHistory.HistoryElementFromIndex (h);
                var hRect = EditorGUILayout.BeginHorizontal ();
                var hButtonStyle = _hWindowSkin.GetStyle ("InteractableButton");

                if (_editorHistory.IsElementSelected (hObject))
                {
                    var hStyle = _hWindowSkin.customStyles[0].onHover.textColor;
                    GUI.color = hStyle;
                }

                var favRect = new Rect (hRect.x + hRect.width - 32, hRect.y + 16, 32, 32); // favorites
                Texture2D _finalFavoriteState = null;
                _finalFavoriteState = _editorHistory.IsHistoryFavorite (hObject) ? FavoriteActiveIcon : FavoriteInActiveIcon;

                HandleInputs (favRect, hRect, hObject);

                // start element

                GUI.Label (hRect, new GUIContent (), hButtonStyle);

                GUIContent hContent = new GUIContent (hObject.name, AssetPreview.GetMiniThumbnail (hObject));

                GUILayout.Label (hContent);

                GUI.color = Color.white;

                GUI.Label (favRect, _finalFavoriteState);

                // finish element

                EditorGUILayout.EndHorizontal ();

                FavoriteSeparation (hObject);
            }
        }
    }

    private void HandleInputs (Rect favrRect, Rect backRect, Object hElement)
    {
        if (IsClickingRect (favrRect, out var Id))
        {
            if (Id == 0)
            {
                _editorHistory.AddFavEelement (hElement);
                Repaint ();
            }
        }
        else
        {

            if (IsClickingRect (backRect, out var mouseId))
            {
                if (mouseId == 0)
                    UpdateHElement (hElement);
                else if (mouseId == 1)
                    RemoveHElement (hElement);
            }
        }
    }

    private void FavoriteSeparation (Object hElement)
    {
        if (_editorHistory.IsLastFavElement (hElement))
        {
            GUILayout.HorizontalSlider (0, 0, 0);
            GUILayout.Space (30);
        }
        else
        {
            GUILayout.Space (5);
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