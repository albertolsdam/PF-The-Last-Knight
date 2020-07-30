using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TK, TV> : ISerializationCallbackReceiver
{
    private Dictionary<TK, TV> _Dictionary;
    [SerializeField] List<TK> _Keys;
    [SerializeField] List<TV> _Values;

    public void Create()
    {
        for(int i=0;i<_Keys.Count;i++)
        {
            _Dictionary.Add(_Keys[i], _Values[i]);
        }
    }

    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {

    }

    public TV Get(TK tk)
    {
        return _Dictionary[tk];
    }

    public int Count { get => _Dictionary.Count; }
}
