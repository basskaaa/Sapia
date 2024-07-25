using System.Collections.Generic;
using System.Linq;
using Assets._Scripts.Game;
using Sapia.Game.Combat.Entities;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._Scripts.Ui
{
    public class AbilityCardHolder : MonoBehaviour, ICardUsedListener
    {
        public GameObject CardPrefab;

        public UnityEvent<UsableAbility, CombatParticipantRef> CardUsed;

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
            var card = Instantiate(CardPrefab, transform);

            var select = card.GetComponentInChildren<CardSelect>();

            select.CardUsedListener = this;

            return card.GetComponent<CardRender>();
        }

        public void OnCardUsed(UsableAbility ability, CombatParticipantRef target)
        {
            CardUsed.Invoke(ability, target);
        }
    }
}
