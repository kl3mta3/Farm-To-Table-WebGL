using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "New Recipe", menuName = "Crafting System/Recipes")]
public class RecipeItem : ItemObject
{

	[Header("Input Materials")]
   
    public List<ItemObject> matsList = new List<ItemObject>();


    [Header("Output Material")]
	public ItemObject output;
    //public int outputAmount;




    

}


