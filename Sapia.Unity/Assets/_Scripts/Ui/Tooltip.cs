using Nova;
using NovaSamples.UIControls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Nova.Gesture;

public class Tooltip : MonoBehaviour
{
    public bool isDisplayed;
    [SerializeField] private float inputDelay = 0.4f;
    [SerializeField] private float xOffset = -480;

    private UIBlock2D tooltipBlock;
    private UIBlock2D tooltipHolder;

    private float tooltipXPos;

    private CardSelect[] cardsSelect;
    private CardHover[] cardsHover;
    private UIBlock2D[] cardsBlock;

    private void OnEnable()
    {
        tooltipBlock = GetComponent<UIBlock2D>();
        tooltipHolder = GetComponentInParent<TooltipHolder>().GetComponent<UIBlock2D>();
        tooltipXPos = tooltipHolder.Position.X.Value;

        Hide();

        cardsSelect = FindObjectsOfType<CardSelect>();
        cardsHover = FindObjectsOfType<CardHover>();
    }

    private void Update()
    {
        CheckIfDisplay();
    }

    private void CheckIfDisplay()
    {
        if (!isDisplayed && CheckIfHovered() && !CheckIfSelect() && Input.GetKeyDown(KeyCode.Q))
        {
            Display(GetXPos());
        }

        if (isDisplayed && Input.GetKeyDown(KeyCode.Q))
        {
            Hide();
        }

        if (isDisplayed && CheckIfSelect())
        {
            Hide();
        }

        if (isDisplayed && !CheckIfHovered())
        {
            Hide();
        }
    }

    private bool CheckIfHovered()
    {
        foreach (CardHover card in cardsHover)
        {
            if (card.isHover)
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckIfSelect()
    {
        foreach (CardSelect card in cardsSelect)
        {
            if (card.isCardSelected)
            {
                return true;
            }
        }
        return false;
    }

    private float GetXPos()
    {
        foreach (CardHover card in cardsHover)
        {
            if (card.isHover)
            {
                return card.GetComponent<UIBlock2D>().Position.X.Value;
            }
        }

        return 0;
    }

    public void Display(float xPos)
    {
        StartCoroutine(TooltipDelay(true));
        tooltipBlock.BodyEnabled = true;
        tooltipBlock.Shadow.Enabled = true;
        tooltipHolder.Position.X.Value = xPos + xOffset;
    }

    public void Hide()
    {
        StartCoroutine(TooltipDelay(false));
        tooltipBlock.BodyEnabled = false;
        tooltipBlock.Shadow.Enabled = false;
        tooltipHolder.Position.X.Value = tooltipXPos;
    }

    private IEnumerator TooltipDelay(bool displayed)
    {
        yield return new WaitForSeconds(inputDelay);
        isDisplayed = displayed;
    }
}
