using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolPanel : MonoBehaviour
{
    [SerializeField]
    internal InventoryObject playerInventory;

    [SerializeField]
    internal InventoryObject toolInventory;
    [SerializeField]
    internal ItemType toolType= ItemType.Tool;
 

    public InventorySlot[] toolInventorySlots { get { return toolInventory.Container.Slots; } }

    public InventorySlot[] playerInventorySlots { get { return playerInventory.Container.Slots; } }
    void Start()
    {
       
        //ResetDisplayedItemsToType();

    }




    public void ResetDisplayedItemsToType()
    {

        for (int i = 0; i < playerInventory.Container.Slots.Length; i++)
        {
            ItemObject tempitem = new ItemObject();
            ItemObject item = new ItemObject();
            tempitem.data = playerInventory.Container.Slots[i].item;
            for (int d = 0; d < playerInventory.database.ItemObjects.Length; d++)
            {
                if (playerInventory.database.ItemObjects[d].data.Id == tempitem.data.Id)
                {
                    item = playerInventory.database.ItemObjects[d];

                }
            }

            if (item.type == toolType)
            {

               // Debug.Log("Tool Found = " + playerInventorySlots[i].item.Name);
                toolInventory.AddItem(item.data, 1);

            }
        }




        for (int i = 0; i < toolInventory.Container.Slots.Length; i++)
        {
            if (toolInventory.Container.Slots[i].item.Id >= 0)
            {
                toolInventory.Container.Slots[i].slotDisplay.gameObject.SetActive(true);

            }
            else
            {

                {
                    toolInventory.Container.Slots[i].slotDisplay.gameObject.SetActive(false);
                }
            }
        }
    }

}

