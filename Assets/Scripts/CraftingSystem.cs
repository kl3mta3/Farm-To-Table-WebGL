using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.RuleTile.TilingRuleOutput;
using System.Linq;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] 
    internal InventoryObject inputInventory;
    [SerializeField]
    internal InventoryObject playerInventory;

    [SerializeField]
    internal ItemDatabaseObject recipeDatabase;
    [SerializeField]
    internal IngredientsPanel ingredientsPanel;

    [SerializeField]
    internal ToolPanel toolPanel;

    [SerializeField]
    internal Player player;

    [SerializeField]
    internal SoundManager soundManager;

    [SerializeField]
    internal TextMeshProUGUI outputText;

    [SerializeField]
    internal TextMeshProUGUI ingredientProgressText;

    [SerializeField]
    internal TextMeshProUGUI toolProgressText;
    
    [SerializeField]
    internal TextMeshProUGUI mealProgressText;
    //[SerializeField]
    //internal InventorySlot[] inputSlots { get { return inputInventory.Container.Slots; } }

    [SerializeField]
    internal Image outputSprite;
    [SerializeField]
    internal GameObject outputPanel;

    [SerializeField]
    internal GameObject inputPanel;

    [SerializeField]
    internal int postTime=10;
    [SerializeField]
    internal float imageRiseRate;

    [SerializeField]
    internal float imageScaleUpRate;
    [SerializeField]
    internal float postWaitTime;
    internal int postIndex=0;

    internal float totalMeals;
    internal float totalIngredients;
    internal float totalTools;
    internal bool postStarted;
    internal bool itemCrafting = false;
    internal Vector3 defaultPostPanelLocation;
    internal Vector3 defaultOutputSpriteScale;
    [SerializeField]
    internal List<ItemObject> inputsList = new List<ItemObject>();
    public void Start()
    {
       // defaultPostPanelLocation = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        defaultOutputSpriteScale = outputSprite.transform.localScale;

        for (int i = 0; i < playerInventory.database.ItemObjects.Length; i++)
        {
            if (playerInventory.database.ItemObjects[i].type == ItemType.Meal)
            {
                totalMeals++;
            }
            else if (playerInventory.database.ItemObjects[i].type == ItemType.Ingredient)
            {
                totalIngredients++;
            }
            else if (playerInventory.database.ItemObjects[i].type == ItemType.Tool)
            {
                totalTools++;
            }


        }

        
    }
    public void Update()
    {

       //CanCraft();
      // CraftItem();
        DisplayProgress();
    }

    public void CraftItem()
    {
        if (CanCraft()&&!itemCrafting)
        {
            //sets current items in input to list
            //defaultPostPanelLocation = outputSprite.transform.position;
            //defaultPostPanelScale = outputSprite.transform.localScale;

            SetInputList();

            ///loop through recipe database
            for (int i = 0; i < recipeDatabase.ItemObjects.Length; i++)
            {
                RecipeItem item = ScriptableObject.CreateInstance<RecipeItem>();
                item = (RecipeItem)recipeDatabase.ItemObjects[i]; // get recipe item from Database

                if (CompairInputsToRecipe(inputsList, item.matsList)) //compare Lists
                                                                      //if (CompareLists(inputsList, item.matsList))
                {
                    itemCrafting = true;
                    postIndex = 0;
                    //Debug.Log("Crafted " + item.output.name);
                    //outputPanel.transform.position = defaultPostPanelLocation;
                    outputSprite.transform.localScale = defaultOutputSpriteScale;
                    outputSprite.sprite = item.output.uiDisplay;
                    outputSprite.color = new Color(1, 1, 1, 1);
                    playerInventory.AddItem(item.output.data, 1);
                    ingredientsPanel.AddItemsToIngredientsPanel();
                    //ingredientsPanel.ReorderInventory();

                    StartCoroutine(CraftSound());


                    if (item.output.type == ItemType.Meal)
                    {
                        player.discoveredMeals++;
                    }
                    else if (item.output.type == ItemType.Ingredient)
                    {
                        player.discoveredIngredients++;
                    }
                    else if (item.output.type == ItemType.Tool)
                    {
                        player.discoveredTools++;
                    }

                    else
                    {
                        player.discoveredIngredients++;
                    }
                    outputPanel.SetActive(true);
                    outputText.text = "You Have Made " + item.output.name + "!";
                    inputPanel.SetActive(false);
                    if (player.saveEnabled)
                        {
                            player.Save();
                        }
                    StartCoroutine(PostOutput());

                    return;
                }
                
                

            }
                    Debug.Log("No Matches Found");
                    return;


            

        }

    }
    //public bool CompareLists<ItemObject>(List<ItemObject> inputs, List<ItemObject> recipes)
    //{
        

    //    if (inputs == null || recipes == null || inputs.Count != recipes.Count)
    //    {
            
    //        Debug.Log("CompareLists = false (null or no count match)");
    //        return false;
    //    }

    //    for (int i = 0; i < inputs.Count; i++)
    //    {

    //        if (!recipes.Contains(inputs[i]))
    //        {
    //            Debug.Log("CompareLists = false (Mis Match Item found)");
    //            return false;
    //        }
    //        else
    //        {
    //           // recipes.Remove(inputs[i]);
    //        }
            
            
    //    }
    //    Debug.Log("CompareLists = True");
    //    return true;
    //}
    public void SetInputList()
    {
        inputsList.Clear();
        for (int i = 0; i < inputInventory.Container.Slots.Length; i++)
        {

            if (inputInventory.Container.Slots[i].item.Id >= 0)
            {
                inputsList.Add(inputInventory.Container.Slots[i].ItemObject);
            }



        }


    }

    
    public bool CanCraft()
    {
        for (int i = 0; i < inputInventory.Container.Slots.Length; i++)
        {
            if (inputInventory.Container.Slots[i].item.Id >= 0)
            {
                //Debug.Log("Can Craft");
                return true;
            }  
         }
                 //Debug.Log("Can't Craft");
                return false;


    }

    public void DisplayProgress()
    {


        ingredientProgressText.text = /*"Discovered " + */player.discoveredIngredients + "/" + ((totalIngredients-player.startIngredients)) + " Ingredients";
        mealProgressText.text = /*"Discovered " +*/ player.discoveredMeals + "/" + (totalMeals) + " Meals";
        toolProgressText.text = /*"Discovered " +*/ player.discoveredTools + "/" + ((totalTools-player.startTools)) + " Tools";

    }
    IEnumerator PostOutput()
    {
        

       while(postIndex<postTime)
        {
            //outputPanel.transform.position += new Vector3(0, imageRiseRate, 0);
            outputSprite.transform.localScale += new Vector3(imageScaleUpRate, imageScaleUpRate, 0);
            //outputSprite.color -= new Color(0, 0, 0, .05f);//turn sprite color back off slowly
            postIndex++;
            yield return new WaitForSeconds(postWaitTime);



        }
        yield return new WaitForSeconds(.3f);
        inputInventory.Clear();
        inputsList.Clear();
        //ingredientsPanel.ReorderInventory();
        toolPanel.ResetDisplayedItemsToType();
        DisplayProgress();
        outputSprite.color = new Color(1, 1, 1, 0);//turn sprite color back off totally
        outputSprite.transform.localScale = defaultOutputSpriteScale;
       // outputPanel.transform.localPosition = defaultPostPanelLocation;
        outputPanel.SetActive(false);
        inputPanel.SetActive(true);
        itemCrafting = false;
        StopCoroutine(PostOutput());
    }


    IEnumerator CraftSound()
    {
        
        soundManager.PlaySound(soundManager.ticTic);
        yield return new WaitForSeconds(soundManager.ticTic.clip.length);
        soundManager.PlaySound(soundManager.ticTic);
        yield return new WaitForSeconds(soundManager.ticTic.clip.length);
        soundManager.PlaySound(soundManager.ding);

        StopCoroutine(CraftSound());
    }

    public static bool CompairInputsToRecipe<ItemObject>(IEnumerable<ItemObject> list1, IEnumerable<ItemObject> list2)
    {
        var cnt = new Dictionary<ItemObject, int>();
        foreach (ItemObject s in list1)
        {
            if (cnt.ContainsKey(s))
            {
                cnt[s]++;
            }
            else
            {
                cnt.Add(s, 1);
            }
        }
        foreach (ItemObject s in list2)
        {
            if (cnt.ContainsKey(s))
            {
                cnt[s]--;
            }
            else
            {
                return false;
            }
        }
        return cnt.Values.All(c => c == 0);
    }



}
