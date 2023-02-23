using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject toolInventory;
    public InventoryObject ingredientsInventory;
    public InventoryObject inputsInventory;

    [SerializeField]
    internal ItemObject[] startItems;
    [SerializeField]
    internal IngredientsPanel ingredientsPanel;
    [SerializeField]
    internal ToolPanel toolPanel;

    internal float discoveredIngredients;
    internal float discoveredMeals;
    internal float discoveredTools;


    internal float startIngredients;
    internal float startMeals;
    internal float startTools;
    [SerializeField]
    internal bool saveEnabled = false;
    [SerializeField]
    internal bool loadEnabled = false;


    void Start()
    {
        if (saveEnabled)
        {
            Load();
        }
        SetStarterItems();

       toolPanel.ResetDisplayedItemsToType();
        //ingredientsPanel.ToggleOther();
        //ingredientsPanel.ReorderInventory();
        //ingredientsPanel.AddItemsToIngredientsPanel();

    }

    public void SetStarterItems()
    {
        if (startItems[0] != null)
        {
            InventoryItem item = new InventoryItem();

            for (int i = 0; i < startItems.Length; i++)
            {
                if (startItems[i].data.Id > -1)
                {
                    item = startItems[i].data;
                    inventory.AddItem(item, 1);

                    switch (startItems[i].type)
                    {
                        case ItemType.Ingredient:
                            startIngredients++;
                            break;
                        case ItemType.Tool:
                            startTools++;
                            break;

                    }
                    




                }
            }

        }
    }

    private void OnApplicationQuit()
    {
        //if (saveEnabled)
        //{
        //    Save();
        //}


        inventory.Clear();
        toolInventory.Clear();
        ingredientsInventory.Clear();
        inputsInventory.Clear();
    }


    public void Save()
    {

        List<InventoryItem> items = new List<InventoryItem>();

        for (int i = 0; i < inventory.Container.Slots.Length; i++)
        {
            if (inventory.Container.Slots[i].item.Id > -1)
            {
                items.Add(inventory.Container.Slots[i].item);
            }
        }
        
        ES3.Save<List<InventoryItem>>("PlayerInventory", items);

        
        ES3.Save<float>("discoveredIngredientCount", discoveredIngredients);

       
        ES3.Save<float>("discoveredToolCount", discoveredTools);

        
        ES3.Save<float>("discoveredMealCount", discoveredMeals);

    }



    public void Load()
    {
        if (loadEnabled)
        {
            if (ES3.KeyExists("PlayerInventory"))
            {


                List<InventoryItem> newItems = new List<InventoryItem>();


                newItems = ES3.Load<List<InventoryItem>>("PlayerInventory");

                for (int i = 0; i < newItems.Count; i++)
                {
                    inventory.AddItem(newItems[i], 1);
                }


            }
            if (ES3.KeyExists("discoveredIngredientCount"))
            {
                float temp= ES3.Load<float>("discoveredIngredientCount");
                discoveredIngredients = temp;
            }

            if (ES3.KeyExists("discoveredToolCount"))
            {
                float temp = ES3.Load<float>("discoveredToolCount");
                discoveredTools = temp;
            }

            if (ES3.KeyExists("discoveredMealCount"))
            {
                float temp = ES3.Load<float>("discoveredMealCount");
                discoveredMeals = temp;
            }
        }
    }

    public void Quit()
    {



        Application.Quit();
    }
}




