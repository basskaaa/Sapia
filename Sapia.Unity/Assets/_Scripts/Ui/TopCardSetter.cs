using Nova;
using NovaSamples.UIControls;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TopCardSetter : MonoBehaviour
{
    private UIBlock2D hoverBlock;

    private void Start()
    {
        FindTopCard();
    }

    public void FindTopCard()
    {
        CardScaleOnHover[] cards = GetComponentsInChildren<CardScaleOnHover>();
        
        foreach (CardScaleOnHover card in cards) 
        { 
            if (card == cards.Last())
            {
                SetHoverColliderX(card, true);
            }

            else
            {
                SetHoverColliderX(card, false);
            }
        }
    }

    private void SetHoverColliderX(CardScaleOnHover card, bool isTop)
    {
        hoverBlock = card.GetComponent<UIBlock2D>();

        hoverBlock.Size.X.Percent = 0.3f;

        if (isTop) 
        {
            hoverBlock.Size.X.Percent = 1;
        }
    }
}
