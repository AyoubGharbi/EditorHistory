using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorHistory
{
    private int _selectedHistoryIndex = -1;
    private object _selectedHistoryObject = null;
    private List<object> _historyElements = new List<object> ();

    public int HistorySize => _historyElements.Count;
    public bool IsElementSelected (object hElement) => _selectedHistoryObject == hElement ||
        _historyElements.FindIndex (h => h == hElement) == _selectedHistoryIndex;

    public List<object> HistoryElements => _historyElements;

    public void AddhEelement (object hElement)
    {
        if (!_historyElements.Contains (hElement))
        {
            _historyElements.Add (hElement);
        }
        else
        {
            Debug.LogFormat ("{0} exists already!", hElement.GetType ().Name);
        }
    }

    public void RemovehEelement (object hElement)
    {
        if (!_historyElements.Contains (hElement))
        {
            Debug.LogFormat ("can't remove {0}..", hElement.GetType ().Name);
            return;
        }

        var hIndex = _historyElements.FindIndex(h => h == hElement);
        _historyElements.RemoveAt(hIndex);
    }
}