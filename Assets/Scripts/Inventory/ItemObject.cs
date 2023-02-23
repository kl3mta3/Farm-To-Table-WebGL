using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ItemType
{
    Ingredient,
    Meal,
    Tool,
    Recipe,

}
[System.Serializable]
public enum IngredientType
{
    Dairy,
    Meat,
    Seafood,
    Vegetable,
    Fruit,
    Grain,
    Seed,
    Alcohol,
    Spice,
    Other,
    Na,
}
[System.Serializable]
public enum MealType
{
	Bake,
    Fry,
    Boil,
    Steam,
    Grill,
    Roast,
    Saute,
    Simmer,
    Broil,
    Poach,
    Other,
    Na,
}






//[CreateAssetMenu(fileName = "New Item", menuName = "Items/ItemObject")]
public class ItemObject : ScriptableObject
{

	public Sprite uiDisplay;

	public ItemType type;
    public IngredientType ingredientType;
    public bool isMeal;
    public MealType mealType;
    
    [TextArea(5, 5)]
	public string description;
	public InventoryItem data = new InventoryItem();

  
	public InventoryItem CreateItem()
	{
		InventoryItem newItem = new InventoryItem(this);
		return newItem;
	}


}

[System.Serializable]
public class InventoryItem
{
    
	public string Name;
	public int Id = -1;
  
	public InventoryItem()
	{
		Name = "";
		Id = -1;
	}
	public InventoryItem(ItemObject item)
	{
        
		Name = item.name;
		Id = item.data.Id;

	}

}







