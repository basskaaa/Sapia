using System.Collections;
using System.Linq;
using Nova;
using UnityEngine;

namespace Assets._Scripts.Ui.Combat.Cards
{
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
            var cardsHover = GetComponentsInChildren<CardHover>();
        
            foreach (var card in cardsHover) 
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

            var cards = GetComponentsInChildren<CardSelect>();

            for (var i = 0; i < cards.Length; i++) 
            {
                var cardsSort = cards[i].GetComponent<SortGroup>();
                var cardsBlock = cards[i].GetComponent<UIBlock2D>();

                cardsSort.SortingOrder = i;
                cardsBlock.Position.Y = 0f;
                cardsBlock.Position.Z = 0f;
            }
        }
    }
}
