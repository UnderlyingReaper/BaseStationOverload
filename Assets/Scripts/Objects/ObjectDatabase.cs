using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Object/ObjectDatabase")]
public class ObjectDatabase : ScriptableObject
{
    public List<ObjectData> objectData;
}

[Serializable]
public class ObjectData
{
    [field: SerializeField] public string name { get; private set; }
    [field: SerializeField] public int iD { get; private set; }
    [field: SerializeField] public int price {get; private set; }
    [field: SerializeField] public Vector2Int size { get; private set; } = Vector2Int.one;
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public bool canRemove { get; private set; }
}
