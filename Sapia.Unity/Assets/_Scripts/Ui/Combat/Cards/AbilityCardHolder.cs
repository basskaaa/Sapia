using System.Collections.Generic;
using System.Linq;
using Assets._Scripts.Editor;
using Assets._Scripts.Game;
using Assets._Scripts.Ui.Combat.Abstractions;
using Nova;
using Sapia.Game.Combat.Entities;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._Scripts.Ui.Combat.Cards
{
    public class AbilityCardHolder : MonoBehaviour, ICardUsedListener
    {

        [InspectorButton(nameof(LayoutCards))] public bool LayoutCardsButton;
        [InspectorButton(nameof(AddCard))] public bool AddCardButton;

        [SerializeField] private int minSpacingPerCard = 5;
        [SerializeField] private int maxSpacingPerCard = 25;
        [SerializeField] private int spaceAfterXCards = 4;

        public GameObject CardPrefab;

        public UnityEvent<UsableAbility, CombatParticipantRef> CardUsed;

        private TopCardSetter _topCardSetter;
        private UIBlock _uiBlock;

        void Awake()
        {
            _topCardSetter = GetComponentInChildren<TopCardSetter>(true);
            _uiBlock = GetComponent<UIBlock>();
        }

        public void Show(IReadOnlyCollection<UsableAbility> abilities)
        {
            var cards = GetComponentsInChildren<CardRender>(true);

            for (var i = 0; i < abilities.Count; i++)
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

            for (var i = abilities.Count; i < cards.Length; i++)
            {
                cards[i].gameObject.SetActive(false);
            }

            _topCardSetter.FindTopCard();

            LayoutCards();
        }

        public void LayoutCards()
        {
            // Find any active child cards and determine how to set the nova autolayout
            var cards = GetComponentsInChildren<CardRender>(false);

            var spacing = minSpacingPerCard;

            if (cards.Length > spaceAfterXCards)
            {
                // Only apply relative spacing after a certain number of cards are displayed
                spacing += cards.Length * 2;
            }

            spacing = Mathf.Clamp(spacing, minSpacingPerCard, maxSpacingPerCard);

            _uiBlock.AutoLayout.Spacing = Length.Percentage(-spacing * 0.01f);
        }

        public void AddCard()
        {
            var cards = GetComponentsInChildren<CardRender>(true);

            if (cards.Any())
            {
                var card = cards.RandomElement();

                var abilities = cards.Select(x => x.Ability).Append(card.Ability).ToArray();

                Show(abilities);
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
