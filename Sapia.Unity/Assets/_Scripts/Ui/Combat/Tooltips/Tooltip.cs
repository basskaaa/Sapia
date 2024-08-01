using System.Collections;
using Assets._Scripts.Ui.Combat.Cards;
using Nova;
using Nova.TMP;
using Sapia.Game.Combat.Entities;
using UnityEngine;

namespace Assets._Scripts.Ui.Combat.Tooltips
{
    public class Tooltip : MonoBehaviour
    {
        public bool isDisplayed;
        [SerializeField] private float inputDelay = 0.4f;
        [SerializeField] private float xOffset = -480;

        private UIBlock2D tooltipBlock;
        private UIBlock2D tooltipHolder;

        private float tooltipXPos;

        private TextMeshProTextBlock[] tooltipText;
        private CardSelect[] cardsSelect;
        private CardHover[] cardsHover;
        private UIBlock2D[] cardsBlock;
        private TooltipRenderer tooltipRenderer;

        private void OnEnable()
        {
            tooltipRenderer = GetComponent<TooltipRenderer>();
            tooltipBlock = GetComponent<UIBlock2D>();
            tooltipHolder = GetComponentInParent<TooltipHolder>().GetComponent<UIBlock2D>();
            tooltipText = GetComponentsInChildren<TextMeshProTextBlock>();
            tooltipXPos = tooltipHolder.Position.X.Value;
        }

        private void Update()
        {
            CheckIfDisplay();
        }

        private void CheckIfDisplay()
        {
            if (!isDisplayed && CheckIfHovered() && !CheckIfSelect() && UnityEngine.Input.GetKeyDown(KeyCode.Q))
            {
                Display(GetHoveredCardDetails());
            }

            if (isDisplayed && UnityEngine.Input.GetKeyDown(KeyCode.Q))
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
            cardsHover = FindObjectsByType<CardHover>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var card in cardsHover)
            {
                if (card.IsHovered)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckIfSelect()
        {
            cardsSelect = FindObjectsByType<CardSelect>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var card in cardsSelect)
            {
                if (card.isCardSelected)
                {
                    return true;
                }
            }
            return false;
        }

        private (UsableAbility ability, float xPos)? GetHoveredCardDetails()
        {
            foreach (var card in cardsHover)
            {
                if (card.IsHovered)
                {
                    var xPos = card.GetComponentInParent<CardSelect>().GetComponent<UIBlock2D>().Position.X.Value;
                    var render = card.GetComponentInParent<CardRender>();

                    return (render.Ability, xPos);
                }
            }

            return null;
        }

        public void Display((UsableAbility ability, float xPos)? details)
        {
            if (!details.HasValue)
            {
                Hide();
                return;
            }

            StartCoroutine(TooltipDelay(true));
            
            tooltipBlock.BodyEnabled = true;
            tooltipBlock.Shadow.Enabled = true;
            
            tooltipHolder.Position.X.Value = details.Value.xPos + xOffset;

            foreach (var text in tooltipText)
            {
                text.gameObject.SetActive(true);
            }

            tooltipRenderer.Render(details.Value.ability);
        }

        public void Hide()
        {
            StartCoroutine(TooltipDelay(false));
            tooltipBlock.BodyEnabled = false;
            tooltipBlock.Shadow.Enabled = false;
            tooltipHolder.Position.X.Value = tooltipXPos;

            foreach (var text in tooltipText)
            {
                text.gameObject.SetActive(false);
            }
        }

        private IEnumerator TooltipDelay(bool displayed)
        {
            yield return new WaitForSeconds(inputDelay);
            isDisplayed = displayed;
        }
    }
}
