﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInterface : UserInterface
{
	public GameObject[] slots;

	public override void CreateSlots()
	{
		slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
		for (int i = 0; i < inventory.GetSlots.Length; i++)
		{
			var obj = slots[i];

            
			//AddEvent(obj, EventTriggerType.PointerEnter, (data) => { OnEnter(obj, (PointerEventData)data); });
            AddEvent(obj, EventTriggerType.PointerExit, (data) => { OnExit(obj, (PointerEventData)data); });
           AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            //AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
			AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
			AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            AddEvent(obj, EventTriggerType.PointerDown, delegate { OnClick(obj); });
            AddEvent(obj, EventTriggerType.PointerUp, delegate { OnClickEnd(obj); });
            inventory.GetSlots[i].slotDisplay = obj;
			slotsOnInterface.Add(obj, inventory.GetSlots[i]);

		}
	}
}