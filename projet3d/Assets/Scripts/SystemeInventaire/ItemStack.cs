using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemStack
{
    public ItemType Type { get; }

    private int _size;

    public int Size
    {
        get => _size;
        set
        {
            if (value < 0)
                _size = 0;
            else if (value > Type.MaxStack)
                _size = Type.MaxStack;
            else
                _size = value;
        }
    }

    public ItemStack(ItemType type, int size)
    {
        
        Type = type;
        Size = size;
        
    }
}
