using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemType
{
    [SerializeField] private string id;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int maxStack;

    public string Id => id;
    public Sprite Sprite => sprite;
    public int MaxStack => maxStack;
}
