using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : NetworkBehaviour
{
    [SerializeField] private List<InventorySlot> slots;

    private int _selectedSlot;
    private List<ItemType> listeObjetsInventaire = new List<ItemType>();
    int nbrElements;

    void Start()
    {
        if (slots.Count == 0)
            return;
        
        slots[0].Select();
        _selectedSlot = 0;
    }
    

    public void Scroll(float value)
    {
        if(value < 0)
            SelectSlot(GetNextToSelectedSlot());
        else if(value > 0)
            SelectSlot(GetPreviousToSelectedSlot());
    }

    public int GetNextToSelectedSlot()
    {
        return (_selectedSlot + 1) % slots.Count;
    }

    public int GetPreviousToSelectedSlot()
    {
        int previous = _selectedSlot - 1;
        if (previous < 0)
            previous = slots.Count + previous;

        return previous;
    }

    public int GetNextAvailableSlot(int start)
    {
        for (int i = start; i < slots.Count; i++)
        {
            if (slots[i].GetItemStack() == null)
                return i;
        }

        return -1;
    }
    //methode qui permet d'ajouter des objets a un stack deja existant ayant le meme type et ayant moins que le max stakc delobjet
    public int GetNextAvailableSlotWithItem(int start, ItemType type)

    {
        
        
        for (int i = start; i < slots.Count; i++)
        {
            ItemStack slotItemStack = slots[i].GetItemStack();
            if (slotItemStack != null && (slotItemStack.Type == type && slotItemStack.Size < slotItemStack.Type.MaxStack))
                return i;
            
        }

        
        return -1;
    }
    

    public void SelectSlot(int slot)
    {
        if (!SlotIsValid(slot)) return;
        
        slots[_selectedSlot].Unselect();
        _selectedSlot = slot;
        slots[_selectedSlot].Select();
    }

    public int GetSelectedSlot()
    {
        return _selectedSlot;
    }

    public int AddItemStack(ItemStack itemStack)
    {
        
        int availableSlotItem = GetNextAvailableSlotWithItem(0, itemStack.Type);

        if (availableSlotItem >= 0)
        {
            int remaining = AddItemStack(availableSlotItem, itemStack);
            if (remaining < itemStack.Size)
                return remaining;
        }

        int availableSlot = GetNextAvailableSlot(0);

        if (availableSlot >= 0)
        {
            SetItemStack(availableSlot, itemStack);
            return 0;
        }
        
        return itemStack.Size;
    }

    public int AddItemStack(int slot, ItemStack itemStack)
    {
        if (itemStack?.Type == null || itemStack.Size == 0) return 0;
        if (!SlotIsValid(slot)) return itemStack.Size;

        ItemStack slotItemStack = slots[slot].GetItemStack();
        if (slotItemStack.Type != itemStack.Type) return itemStack.Size;

        int newSize = slotItemStack.Size + itemStack.Size;
        slotItemStack.Size = newSize;
        slots[slot].UpdateVisuals();

        return newSize - slotItemStack.Size;
    }

    public void SetItemStack(int slot, ItemStack itemStack)
    {
        if (!SlotIsValid(slot)) return;
        slots[slot].SetItemStack(itemStack);
    }

    public ItemStack GetItemStack(int slot)
    {
        if (!SlotIsValid(slot)) return null;
        return slots[slot].GetItemStack();
    }

    private bool SlotIsValid(int slot)
    {
        return slot >= 0 && slot < slots.Count;
    }
}
