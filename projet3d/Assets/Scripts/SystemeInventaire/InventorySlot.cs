using MLAPI;
using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : NetworkBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Text quantityText;
    [SerializeField] private Color unselectedColor;
    [SerializeField] private Color selectedColor;

    private ItemStack _itemStack = null;

    void Start()
    {
        UpdateVisuals();
    }
    public void Select()
    {
        backgroundImage.color = selectedColor;
    }

    public void Unselect()
    {
        backgroundImage.color = unselectedColor;
    }

    public void SetItemStack(ItemStack itemStack)
    {
        _itemStack = itemStack;
        print(itemStack.Type);
        UpdateVisuals();
    }

    public ItemStack GetItemStack()
    {
        return _itemStack;
    }

    public void UpdateVisuals()
    {

            if (_itemStack?.Type == null || _itemStack.Size == 0)
            {
                _itemStack = null;
                itemImage.gameObject.SetActive(false);
                quantityText.text = "";
                return;
            }

            //Debug.Log("Enter Host UpdateVisual");
            itemImage.gameObject.SetActive(true);
           // Debug.Log("Update Visual Server");
            itemImage.sprite = _itemStack.Type.Sprite;
            quantityText.text = _itemStack.Size.ToString();
            //SubmitUpdateVisualsClientRpc();
        }
       
}
