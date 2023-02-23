using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    [SerializeField]
    internal GameObject toolTip;
    [SerializeField]
    internal TextMeshProUGUI tTItemName;
    [SerializeField]
    internal TextMeshProUGUI tTDescription;
    [SerializeField]
    internal Image tTSprite;

    internal bool dragging;
}
