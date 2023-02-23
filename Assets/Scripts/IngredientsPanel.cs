using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
//using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

public class IngredientsPanel : MonoBehaviour
{
    [SerializeField]
    internal InventoryObject playerInventory;

    [SerializeField]
    internal GameObject menuPanel;

    [SerializeField]
    internal GameObject helpPanel;


    [SerializeField]
    internal InventoryObject ingredientsInventory;

    internal ItemType itemType = ItemType.Ingredient;

    public InventorySlot[] ingredientInventoryArray { get { return ingredientsInventory.Container.Slots; } }

    public InventorySlot[] playerInventoryArray { get { return playerInventory.Container.Slots; } }


    internal bool dairyOn;
    internal bool fruitOn;
    internal bool grainOn;
    internal bool meatOn;
    internal bool seafoodOn;
    internal bool seedOn;
    internal bool spiceOn;
    internal bool vegetablesOn;
    internal bool alcoholOn;
    internal bool otherOn;


    internal string ingredienttoAddTypeString;
    internal string ingredienttoRemoveTypeString;

    public void Start()
    {

       //ToggleOther();



    }

    public void AddItemsToIngredientsPanel()
    {
        IngredientType ingredientType = new IngredientType();

        switch (ingredienttoAddTypeString)
        {

            case "Dairy":
                ingredientType = IngredientType.Dairy;
                break;

            case "Fruit":
                ingredientType = IngredientType.Fruit;
                break;

            case "Grain":
                ingredientType = IngredientType.Grain;
                break;

            case "Meat" :
                ingredientType = IngredientType.Meat;
                break;

            case "Seafood":
                ingredientType = IngredientType.Seafood;
                break;

            case "Seed":
                ingredientType = IngredientType.Seed;
                break;

            case "Spice":
                ingredientType = IngredientType.Spice;
                break;

            case "Vegetable":
                ingredientType = IngredientType.Vegetable;
                break;

            case "Alcohol":
                ingredientType = IngredientType.Alcohol;
                break;

            case "Other":
                ingredientType = IngredientType.Other;
                break;
                

        }
        //sets ingredient type for search

       // Debug.Log("ingredientType" + ingredientType.ToString());
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

                if (item.type == itemType)
                {
                    if (item.ingredientType == ingredientType)
                    {
                        ingredientsInventory.AddItem(item.data, 1);
                    }
                }
            }
        }
        ReorderInventory();
    }

    public void OpenMenu()
    {



        menuPanel.SetActive(true);
    }



    public void OpenHelp()
    {


        helpPanel.SetActive(true);
    }
    public void ReorderInventory()
    {
        List<InventoryItem> items = new List<InventoryItem>();

        for (int i = 0; i < ingredientsInventory.Container.Slots.Length; i++)
        {
            if (ingredientsInventory.Container.Slots[i].item.Id>=0)
            {
                items.Add(ingredientsInventory.Container.Slots[i].item);
            }
        }

        ingredientsInventory.Clear();

        foreach (InventoryItem item in items)
        {
            ingredientsInventory.AddItem(item, 1);
        }

        for (int i = 0; i < ingredientsInventory.Container.Slots.Length; i++)
        {
            if(ingredientsInventory.Container.Slots[i].item.Id >= 0)
            {
                ingredientsInventory.Container.Slots[i].slotDisplay.gameObject.SetActive(true);

            }
            else
            {

                {
                    ingredientsInventory.Container.Slots[i].slotDisplay.gameObject.SetActive(false);
                }
            }
        }
    }

    public void RemoveItemsToIngredientsPanel(string typestring)
    {

        IngredientType ingredientType = new IngredientType();
        //Debug.Log("ingredientTypeString = " + ingredienttoRemoveTypeString);

        switch (typestring)
        {


            case "Dairy":
                ingredientType = IngredientType.Dairy;
                break;

            case "Fruit":
                ingredientType = IngredientType.Fruit;
                break;

            case "Grain":
                ingredientType = IngredientType.Grain;
                break;

            case "Meat":
                ingredientType = IngredientType.Meat;
                break;

            case "Seafood":
                ingredientType = IngredientType.Seafood;
                break;

            case "Seed":
                ingredientType = IngredientType.Seed;
                break;

            case "Spice":
                ingredientType = IngredientType.Spice;
                break;

            case "Vegetable":
                ingredientType = IngredientType.Vegetable;
                break;

            case "Alcohol":
                ingredientType = IngredientType.Alcohol;
                break;

            case "Other":
                ingredientType = IngredientType.Other;
                break;


        }//sets ingredient type for search


        for (int i = 0; i < ingredientsInventory.Container.Slots.Length; i++)
        {

            ItemObject item = new ItemObject();

            for (int d = 0; d < ingredientsInventory.database.ItemObjects.Length; d++)
             {
                
                if (ingredientsInventory.database.ItemObjects[d].data.Id == ingredientsInventory.Container.Slots[i].item.Id)
                {
                    item = ingredientsInventory.database.ItemObjects[d];
                }

                    if (item.ingredientType == ingredientType)
                    {
                        ingredientsInventory.Container.Slots[i].UpdateSlot(new InventoryItem(), 0);
                    }
             }
                
         }
        
        ReorderInventory();
    }


    public void ToggleDairy()
    {
        ingredienttoAddTypeString = "Dairy";
        dairyOn = !dairyOn;
        if (dairyOn)
        {

            ingredienttoAddTypeString = "Dairy";
            AddItemsToIngredientsPanel();
        }
        else if (!dairyOn)
        {
            ingredienttoRemoveTypeString = "Dairy";
            Debug.Log("Boop");
            RemoveItemsToIngredientsPanel("Dairy");
        }

    }

    public void ToggleFruit()
    {
        ingredienttoAddTypeString = "Fruit";
        fruitOn = !fruitOn;
        if(fruitOn)
        { 
        
        ingredienttoAddTypeString = "Fruit";
        AddItemsToIngredientsPanel();
        }
        else if(!fruitOn)
        {
            ingredienttoRemoveTypeString = "Fruit";
            RemoveItemsToIngredientsPanel("Fruit");
        }
    }

    public void ToggleGrain()
    {

        ingredienttoAddTypeString = "Grain";
        grainOn = !grainOn;
        if (grainOn)
        {

            ingredienttoRemoveTypeString = "Grain";
            AddItemsToIngredientsPanel();
        }
        else if (!grainOn)
        {
            ingredienttoRemoveTypeString = "Grain";
            RemoveItemsToIngredientsPanel("Grain");
        }




    }

    public void ToggleMeat()
    {
        ingredienttoAddTypeString = "Meat";
        meatOn = !meatOn;
        if (meatOn)
        {

            ingredienttoAddTypeString = "Meat";
            AddItemsToIngredientsPanel();
        }
        else if (!meatOn)
        {
            ingredienttoRemoveTypeString = "Meat";
            RemoveItemsToIngredientsPanel("Meat");
        }
    }

    public void ToggleSeafood()
    {

        ingredienttoAddTypeString = "Seafood";
        seafoodOn = !seafoodOn;
        if (seafoodOn)
        {

            ingredienttoAddTypeString = "Seafood";
            AddItemsToIngredientsPanel();
        }
        else if (!seafoodOn)
        {
            ingredienttoRemoveTypeString = "Seafood";
            RemoveItemsToIngredientsPanel("Seafood");
        }

    }

    public void ToggleSeed()
    {
        ingredienttoAddTypeString = "Seed";
       seedOn = !seedOn;
        if (seedOn)
        {

            ingredienttoAddTypeString = "Seed";
            AddItemsToIngredientsPanel();
        }
        else if (!seedOn)
        {
            ingredienttoRemoveTypeString = "Seed";
            RemoveItemsToIngredientsPanel("Seed");
        }

    }

    public void ToggleSpice()
    {
        ingredienttoAddTypeString = "Spice";
        spiceOn = !spiceOn;
        if (spiceOn)
        {

            ingredienttoAddTypeString = "Spice";
            AddItemsToIngredientsPanel();
        }
        else if (!spiceOn)
        {
            ingredienttoRemoveTypeString = "Spice";
            RemoveItemsToIngredientsPanel("Spice");
        }


    }

    public void ToggleVegetable()
    {

        ingredienttoAddTypeString = "Vegetable";
        vegetablesOn = !vegetablesOn;
        if (vegetablesOn)
        {

            ingredienttoAddTypeString = "Vegetable";
            AddItemsToIngredientsPanel();
        }
        else if (!vegetablesOn)
        {
            ingredienttoRemoveTypeString = "Vegetable";
            RemoveItemsToIngredientsPanel("Vegetable");
        }
    }

    public void ToggleAlcohol()
    {

        ingredienttoAddTypeString = "Alcohol";

       alcoholOn = !alcoholOn;
        if (alcoholOn)
        {

            ingredienttoAddTypeString = "Alcohol";
            AddItemsToIngredientsPanel();
        }
        else if (!alcoholOn)
        {
            ingredienttoRemoveTypeString = "Alcohol";
            RemoveItemsToIngredientsPanel("Alcohol");
        }
    }

    public void ToggleOther()
    {
        ingredienttoAddTypeString = "Other";
        otherOn = !otherOn;
        if (otherOn)
        {

            ingredienttoAddTypeString = "Other";
            AddItemsToIngredientsPanel();
        }
        else if (!otherOn)
        {
            ingredienttoRemoveTypeString = "Other";
            RemoveItemsToIngredientsPanel("Other");
        }

    }


}
