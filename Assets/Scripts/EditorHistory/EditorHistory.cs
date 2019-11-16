using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EditorHistory
{
    private int _selectedHistoryIndex = -1;
    private Object _selectedHistoryObject = null;
    private List<Object> _historyElements = new List<Object> ();

    public int HistorySize => _historyElements.Count;
    public bool IsElementSelected (Object hElement) => _selectedHistoryObject == hElement &&
        _historyElements.FindIndex (h => h == hElement) == _selectedHistoryIndex;

    public List<Object> HistoryElements => _historyElements;

    public void AddhEelement (Object hElement)
    {
        if (!_historyElements.Contains (hElement))
        {
            _historyElements.Add (hElement);
        }
        else
        {
            Debug.LogFormat ("{0} exists already!", hElement.name);
        }
    }

    public void UpdateSelection (UnityEngine.Object hObject)
    {
        if (hObject == null) return;

        var index = _historyElements.FindIndex (el => el == hObject);

        _selectedHistoryIndex = index;
        _selectedHistoryObject = hObject;
    }

    public void RemovehEelement (Object hElement)
    {
        if (!_historyElements.Contains (hElement))
        {
            Debug.LogFormat ("can't remove {0}..", hElement.name);
            return;
        }

        var hIndex = _historyElements.FindIndex (h => h == hElement);

        _historyElements.RemoveAt (hIndex);

        UpdateSelection (_historyElements.Last ());
    }
}