using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Available,
    Resource
}

public enum AvailableType
{
    Health,
    Speed
}

[Serializable]
public class ItemDataAvailable
{
    public AvailableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]

public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Available")]
    public ItemDataAvailable[] Available;
}
