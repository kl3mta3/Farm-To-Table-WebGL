using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using System.Runtime.CompilerServices;

public abstract class UserInterface : MonoBehaviour
{

	public InventoryObject inventory;
	[SerializeField]
	internal ToolTip toolTip;

	private bool tempItemCreated = false;
    
    public SoundManager soundManager;

    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
	void Start()
	{
		for (int i = 0; i < inventory.GetSlots.Length; i++)
		{
			inventory.GetSlots[i].parent = this;
			inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;

		}
		CreateSlots();
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        //toolTip = GameObject.FindGameObjectWithTag("ToolTip").GetComponent<ToolTip>();
        //AddEvent(gameObject, EventTriggerType.PointerEnter, (data) => { OnEnterInterface(gameObject, (PointerEventData)data); });
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        //AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, (data) => { OnExitInterface(gameObject, (PointerEventData)data); });


    }

	public void OnSlotUpdate(InventorySlot _slot)
	{
		if (_slot != null )
		{

			if (_slot.item.Id >= 0 && _slot.slotDisplay.transform.childCount > 0)
			{
				_slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.ItemObject.uiDisplay;
				_slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
			    _slot.slotDisplay.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = _slot.item.Name;

			}
			else
			{
				_slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
				_slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
				_slot.slotDisplay.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "";
			}
            
		}
	}


	public abstract void CreateSlots();




    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener((data) => { action((PointerEventData)data); });
        trigger.triggers.Add(eventTrigger);
    }

	public void OnClick(GameObject obj)
	{
        soundManager.PlayButton1Sound();
        //Debug.Log("OnClick");
    }

	public void OnClickEnd(GameObject obj)
	{
        soundManager.PlayButton2Sound();
    }
	public void OnEnter(GameObject obj)
	{
		MouseData.slotHoveredOver = obj;

        /*Start ToolTip*/
        if (!toolTip.dragging)
        {
            InventorySlot hoveringItem = slotsOnInterface[obj];
            if (hoveringItem.item.Id >= 0)
            {
                CreateDescription(hoveringItem);

            }
        }
        /*End ToolTip*/


    }
    public void OnExit(GameObject obj, PointerEventData data)
    {
		if (data.fullyExited)
		{ 
			MouseData.slotHoveredOver = null;

            /*Start ToolTip*/
            if (toolTip != null)
                toolTip.toolTip.SetActive(false);
            /*End ToolTip*/
        }

    }
	public void OnEnterInterface(GameObject obj)
	{
		MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();
		if (MouseData.interfaceMouseWasOver == null)
			MouseData.interfaceMouseWasOver = MouseData.interfaceMouseIsOver;

        
	}
	public void OnExitInterface(GameObject obj, PointerEventData data)
	{
		if (data.fullyExited)
		{
			MouseData.interfaceMouseWasOver = MouseData.interfaceMouseIsOver;
			MouseData.interfaceMouseIsOver = null;
		}
	}
	public void OnDragStart(GameObject obj)
	{
		MouseData.tempItemBeingDragged = CreateTempItem(obj);
		MouseData.firstSlot = slotsOnInterface[obj];
        
	}
	public GameObject CreateTempItem(GameObject obj)
	{
		GameObject tempItem = null;
     
		if (slotsOnInterface[obj].item.Id >= 0 )
		{
			tempItemCreated = true;
			tempItem = new GameObject();
			var rt = tempItem.AddComponent<RectTransform>();
			rt.sizeDelta = new Vector2(50, 50);
			tempItem.transform.SetParent(transform.parent);

			var img = tempItem.AddComponent<Image>();
			img.sprite = slotsOnInterface[obj].ItemObject.uiDisplay;
			img.raycastTarget = false;
		}
        
		return tempItem;
  
       
	}
	public void OnDragEnd(GameObject obj)
	{

		Destroy(MouseData.tempItemBeingDragged);
		if (MouseData.tempItemBeingDragged == null)
			return;

		if (slotsOnInterface[obj] == null)
			return;

		if (MouseData.tempItemBeingDragged == null)
			return;
        
        
		if (MouseData.interfaceMouseIsOver == null)
		{

            if (slotsOnInterface[obj].parent.inventory.type==InterfaceType.Inputs)
			{ 
				slotsOnInterface[obj].RemoveItem(); 
			
			
			}


			return;
		}
       
		if (MouseData.slotHoveredOver)
		{

			if (MouseData.firstSlot == MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver])
			{

				return;
			}

				InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
				inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);

            /*ToolTip Start*/
            if (toolTip != null)
            {
                toolTip.toolTip.SetActive(false);
            }
            /*ToolTip End*/


        }

    }

    /*ToolTip Start*/
    void CreateDescription(InventorySlot hoveringItem)
    {

        toolTip.toolTip.SetActive(true);


        toolTip.tTSprite.sprite = hoveringItem.ItemObject.uiDisplay;
        toolTip.tTItemName.text = hoveringItem.item.Name;
        toolTip.tTDescription.text = hoveringItem.ItemObject.description;

    }


    /*ToolTip End*/



    public void OnDrag(GameObject obj)
	{
		if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1) || Input.GetMouseButtonUp(1))
			Destroy(MouseData.tempItemBeingDragged);


		if (MouseData.tempItemBeingDragged != null)
			MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
	}

	public Sprite GetItemImage(InventorySlot _slot)
	{
		if (_slot.item.Id >= 0)
		{

			return _slot.ItemObject.uiDisplay;
		}
		else
		{
			return null;
		}

	}
}
public static class MouseData
{
	public static UserInterface interfaceMouseIsOver;
	public static GameObject tempItemBeingDragged;
	public static GameObject slotHoveredOver;
	public static InventorySlot firstSlot;
	public static UserInterface interfaceMouseWasOver;

}


public static class ExtensionMethods
{
	public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlot> _slotsOnInterface)
	{

		foreach (KeyValuePair<GameObject, InventorySlot> _slot in _slotsOnInterface)
		{


			if (_slot.Value.item.Id >= 0 && _slot.Key.transform.childCount > 0)
			{
				_slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.Value.ItemObject.uiDisplay;
				_slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
				_slot.Key.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.ItemObject.data.Name;
				
			}
			else
			{
				_slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
				_slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
				_slot.Key.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "";
			}
		}
	}



}
