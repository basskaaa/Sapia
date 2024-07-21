using Nova;
using NovaSamples.UIControls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private CardSelect cardSelect;
    private CardHover cardHover;
    private Transform cardTooltip;
    private UIBlock2D tooltipBlock;

    private void Start()
    {
        cardSelect = GetComponent<CardSelect>();
        cardHover = GetComponentInChildren<CardHover>();
        cardTooltip = FindObjectOfType<Tooltip>().transform;
        tooltipBlock = cardTooltip.GetComponent<UIBlock2D>();
        tooltipBlock.BodyEnabled = false;
    }

    private void Update()
    {
        if (cardHover.isHover && !cardSelect.isCardSelected && Input.GetKeyDown(KeyCode.Q))
        {
            DisplayTooltip();
        }
    }

    private void DisplayTooltip()
    {
        Debug.Log("Hover " + transform.name);
        tooltipBlock.BodyEnabled = true;
    }
}
