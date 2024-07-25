using System.Collections.Generic;
using System.Linq;
using Sapia.Game.Combat.Entities;
using UnityEngine;

namespace Assets._Scripts.Ui
{
    public class AbilityCardHolder : MonoBehaviour
    {
        public GameObject CardPrefab;

        public void Show(IReadOnlyCollection<UsableAbility> abilities)
        {
            var cards = GetComponentsInChildren<CardRender>(true);

            for (int i = 0; i < abilities.Count; i++)
            {
                var ability = abilities.ElementAt(i);

                CardRender card;

                if (i >= cards.Length)
                {
                    card = InstantiateCard();
                }
                else
                {
                    card = cards[i];
                }

                card.gameObject.SetActive(true);
                card.Render(ability);
            }

            for (int i = abilities.Count; i < cards.Length; i++)
            {
                cards[i].gameObject.SetActive(false);
            }
        }

        private CardRender InstantiateCard()
        {
            var card = Instantiate(CardPrefab);

            card.transform.SetParent(transform);

            return card.GetComponent<CardRender>();
        }
    }
}
