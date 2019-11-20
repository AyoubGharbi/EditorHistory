using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EditorHistory
{
    private int _selectedHistoryIndex = -1;
    private Object _selectedHistoryObject = null;
    private List<Object> _historyElements = new List<Object> ();
    private List<Object> _historyFavorites = new List<Object> ();

    public int HistorySize => _historyElements.Count;
    public int FavoriteSize => _historyFavorites.Count;
    public Object HistoryElementFromIndex (int index) => _historyElements[index];
    public bool IsElementSelected (Object hElement) => _selectedHistoryObject == hElement &&
        _historyElements.IndexOf (hElement) == _selectedHistoryIndex;

    public bool IsHistoryFavorite (Object hElement) => _historyFavorites.Contains (hElement);

    public bool IsLastFavElement (Object hElement)
    {
        if (FavoriteSize > 0)
            return hElement == _historyFavorites.Last ();

        return false;
    }

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

    public void AddFavEelement (Object favElement)
    {
        if (!_historyFavorites.Contains (favElement))
        {
            _historyFavorites.Add (favElement);
            OrderByFavoriteState ();
        }
        else
        {
            Debug.LogFormat ("{0} exists already!", favElement.name);
        }
    }

    public void UpdateSelection (UnityEngine.Object hObject)
    {
        if (hObject == null) return;

        var index = _historyElements.IndexOf (hObject);

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

        var hIndex = _historyElements.IndexOf (hElement);

        _historyElements.RemoveAt (hIndex);

        if (HistorySize > 0)
            UpdateSelection (_historyElements.Last ());

        // favorite 
        RemoveFElement (hElement);
    }

    public void RemoveFElement (Object hObject)
    {
        if (!_historyFavorites.Contains (hObject))
        {
            Debug.LogFormat ("can't remove {0}..", hObject.name);
            return;
        }

        var hIndex = _historyFavorites.IndexOf (hObject);

        _historyFavorites.RemoveAt (hIndex);

        OrderByFavoriteState ();
    }

    void OrderByFavoriteState ()
    {
        var tempList = new List<Object> ();
        var favElements = _historyElements.FindAll (h => IsHistoryFavorite (h));
        var nonFavElements = _historyElements.Except (favElements);

        tempList.AddRange (favElements);
        tempList.AddRange (nonFavElements);

        _historyElements = tempList;
    }
}