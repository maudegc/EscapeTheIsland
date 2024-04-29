using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    [SerializeField] private List<ItemType> itemTypes;

    private readonly Dictionary<string, ItemType> _itemTypesById = new Dictionary<string, ItemType>();
    
    void Awake()
    {
        foreach (ItemType itemType in itemTypes)
        {
            _itemTypesById[itemType.Id] = itemType;
        }
    }

    public ItemType GetItemType(string id)
    {
        
        return _itemTypesById.ContainsKey(id) ? _itemTypesById[id] : null;
    }
}
