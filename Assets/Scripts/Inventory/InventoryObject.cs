using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;
using System.Linq;
using Unity.VisualScripting;

public enum InterfaceType
{
	Player,
	Ingredients,
	Inputs,
	Tools,
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory Object")]
public class InventoryObject : ScriptableObject
{
	[SerializeField]
	internal GameObject[] inventoryTextNotifications;
	//public string savePath;
	public ItemDatabaseObject database;
	public InterfaceType type;
	public Inventory Container;
    //public SoundManager soundManager;
    public InventorySlot[] GetSlots { get { return Container.Slots; } }

    public void Awake()
	{

       

    }
   
	public bool AddItem(InventoryItem _item, int _amount)
	{
		if (EmptySlotCount <= 0)
			return false;
		InventorySlot slot = FindItemOnInventory(_item);
		if ( slot == null)
		{
			SetEmptySlot(_item, _amount);

			return true;
		}
		slot.AddAmount(_amount);

		return true;
	}



	public int EmptySlotCount
	{
		get
		{
			int counter = 0;
			for (int i = 0; i < GetSlots.Length; i++)
			{
				if (GetSlots[i].item.Id <= -1)
				{
					counter++;
				}
			}
			return counter;
		}
	}
	public InventorySlot FindItemOnInventory(InventoryItem _item)
	{
		for (int i = 0; i < GetSlots.Length; i++)
		{
			if (GetSlots[i].item.Id == _item.Id )
			{
				return GetSlots[i];
			}
		}
		return null;
	}

	public InventorySlot FindInventoryItemForCrafting(InventoryItem _item, int neededAmount)
	{
		for (int i = 0; i < GetSlots.Length; i++)
		{
			if (GetSlots[i].item.Id == _item.Id && GetSlots[i].amount >= neededAmount)
			{
				return GetSlots[i];
			}
		}
		return null;
	}

    public bool ItemTypeCheck(InventoryItem _item,ItemType _type)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == _item.Id && GetSlots[i].ItemObject.type == _type)
            {
                return true;
            }
        }
        return false;
    }

    public bool IngredientTypeCheck(InventoryItem _item, IngredientType _type)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == _item.Id && GetSlots[i].ItemObject.ingredientType == _type)
            {
                return true;
            }
        }
        return false;
    }



    public InventorySlot SetEmptySlot(InventoryItem _item, int _amount)
	{
		for (int i = 0; i < GetSlots.Length; i++)
		{
			if (GetSlots[i].item.Id <= -1)
			{
				GetSlots[i].UpdateSlot(_item, _amount);
				return GetSlots[i];
			}
		}
		return null;
	}
	

	public void SwapItems(InventorySlot item1, InventorySlot item2)
	{
        //Debug.Log("Swapping");
        //Debug.Log("Item1"+item1.item.Name);
        //Debug.Log("item1.parent.name" + item1.parent.name);
        //Debug.Log("inventory type item1 = "+item1.parent.inventory.type.ToString());
        //Debug.Log("inventory type item2 = " + item2.parent.inventory.type.ToString());

        if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject))
        {

			if (item1.parent.inventory.type == InterfaceType.Ingredients || item1.parent.inventory.type == InterfaceType.Tools)
			{
				if (item2.parent.inventory.type == InterfaceType.Inputs)
				{

						//for (int i = 0; i < item2.parent.inventory.Container.Slots.Length; i++)
						//{
						//	if (item2.parent.inventory.Container.Slots[i].item.Id == item1.item.Id)
						//	{
						//		item1.UpdateSlot(item1.item, 1);
						//		return;
						//	}
						//}
					
						item2.UpdateSlot(item1.item, 1);
						item1.UpdateSlot(item1.item, 1);
					
                }
				else if (item2.parent.inventory.type == InterfaceType.Ingredients || item2.parent.inventory.type == InterfaceType.Tools)
				{

                        item1.UpdateSlot(item1.item, 1);
                    
                }
                

            }
			else if (item1.parent.inventory.type == InterfaceType.Inputs)
			{
                if (item2.parent.inventory.type == InterfaceType.Inputs)
				{
                    //for (int i = 0; i < item2.parent.inventory.Container.Slots.Length; i++)
                    //{
                    //	if (item2.parent.inventory.Container.Slots[i].item.Id == item2.item.Id)
                    //	{


                    //	}
                    
                    
					
					item1.UpdateSlot(item1.item, 1);
						item2.UpdateSlot(item2.item, 1);
					//}
                }
				else
				{
                    item1.UpdateSlot(new InventoryItem(), 0);
					item2.UpdateSlot(item2.item, 1);
                    
                }

            }

        }

	}





	[ContextMenu("Save")]
	public void Save()
	{
		Inventory _inventory = Container;
        ES3.Save("PlayerInventory", _inventory);
       

	}
	[ContextMenu("Load")]
	public void Load()
	{
   
		if (ES3.KeyExists("PlayerInventory"))
		{
			
			Inventory newContainer = ES3.Load<Inventory>("PlayerInventory");
            for (int i = 0; i < newContainer.Slots.Length; i++)
            {
                Container.Slots[i].UpdateSlot(newContainer.Slots[i].item, newContainer.Slots[i].amount);
            }
   //         for (int i = 0; i < GetSlots.Length; i++)
			//{

			//	GetSlots[i].UpdateSlot(newContainer.Slots[i].item, newContainer.Slots[i].amount);
                
			//}
           
		}
	}
	[ContextMenu("Clear")]
	public void Clear()
	{
		if (Container != null)
		{
			Container.Clear();
		}
	}

	public void Refresh()
	{
		Container.Refresh();
	}
}
[System.Serializable]
public class Inventory
{
    
	public InventorySlot[] Slots = new InventorySlot[50];
	public void Clear()
	{
		for (int i = 0; i < Slots.Length; i++)
		{
			//if (Slots[i].item.Id >=0)
			//{
			//    Slots[i].RemoveItem();
			//}
			Slots[i].RemoveItem();
		}
	}

	public void Refresh()
	{
		InventoryItem item= new InventoryItem();
		int amount=0;
       
		for (int i = 0; i < Slots.Length; i++)
		{
			item = Slots[i].item;
			amount = Slots[i].amount;
           

			Slots[i].UpdateSlot(item, amount);

		}

	}
}

public delegate void SlotUpdated(InventorySlot _slot);

[System.Serializable]
public class InventorySlot
{
	public ItemType[] AllowedItems = new ItemType[0];
	[System.NonSerialized]
	public UserInterface parent;
	[System.NonSerialized]
	public GameObject slotDisplay;
	[System.NonSerialized]
	public SlotUpdated OnAfterUpdate;
	[System.NonSerialized]
	public SlotUpdated OnBeforeUpdate;
	public InventoryItem item = new InventoryItem();
	public int amount;

	public ItemObject ItemObject
	{
		get
		{
            //Debug.Log("ItemObject Search with item id is " + item.Id);
            //Debug.Log("Parent Interface name =" + parent.gameObject.name);
            if (item.Id >= 0)
			{
				return parent.inventory.database.ItemObjects[item.Id];
			}
			return null;
		}
	}

    

	public InventorySlot()
	{
		UpdateSlot(new InventoryItem(), 0);
	}
	public InventorySlot(InventoryItem _item, int _amount)
	{
		UpdateSlot(_item, _amount);
	}
	public void UpdateSlot(InventoryItem _item, int _amount)
	{
		if (OnBeforeUpdate != null)
			OnBeforeUpdate.Invoke(this);
		item = _item;
		amount = _amount;
		if (OnAfterUpdate != null)
			OnAfterUpdate.Invoke(this);
	}
	public void RemoveItem()
	{
		UpdateSlot(new InventoryItem(), 0);
	}
	public void AddAmount(int value) 
	{
		UpdateSlot(item, amount += value);
	}
	public bool CanPlaceInSlot(ItemObject _itemObject)
	{
		if (AllowedItems.Length <= 0 || _itemObject == null || _itemObject.data.Id < 0)
			return true;
		for (int i = 0; i < AllowedItems.Length; i++)
		{
			if (_itemObject.type == AllowedItems[i])
				return true;
		}
		return false;
	}
}
