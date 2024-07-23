using Nova;
using NovaSamples.UIControls;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TopCardSetter : MonoBehaviour
{
    [SerializeField] private float setSortOrderDelay = 0.1f; 
    [SerializeField] private float topCardHoverWidth = 1f; 
    [SerializeField] private float belowCardHoverWidth = 0.3f; 

    private UIBlock2D hoverBlock;

    private void Start()
    {
        FindTopCard();
    }

    public void FindTopCard()
    {
        CardHover[] cardsHover = GetComponentsInChildren<CardHover>();
        
        foreach (CardHover card in cardsHover) 
        { 
            if (card == cardsHover.Last())
            {
                SetHoverColliderX(card, true);
            }

            else
            {
                SetHoverColliderX(card, false);
            }
        }

        StartCoroutine(SetSortingOrder());
    }

    private void SetHoverColliderX(CardHover card, bool isTop)
    {
        hoverBlock = card.GetComponent<UIBlock2D>();

        hoverBlock.Size.X.Percent = belowCardHoverWidth;

        if (isTop) 
        {
            hoverBlock.Size.X.Percent = topCardHoverWidth;
        }
    }

    private IEnumerator SetSortingOrder()
    {
        yield return new WaitForSeconds(setSortOrderDelay);

        CardSelect[] cards = GetComponentsInChildren<CardSelect>();

        for (int i = 0; i < cards.Length; i++) 
        {
            SortGroup cardsSort = cards[i].GetComponent<SortGroup>();
            UIBlock2D cardsBlock = cards[i].GetComponent<UIBlock2D>();

            cardsSort.SortingOrder = i;
            cardsBlock.Position.Y = 0f;
            cardsBlock.Position.Z = 0f;
        }
    }
}
