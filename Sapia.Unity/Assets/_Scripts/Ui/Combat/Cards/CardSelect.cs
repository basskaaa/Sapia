using Assets._Scripts.Game;
using Assets._Scripts.Input;
using Assets._Scripts.Ui.Combat.Abstractions;
using Nova;
using NovaSamples.UIControls;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._Scripts.Ui.Combat.Cards
{
    public class CardSelect : UIControl<ButtonVisuals>
    {
        public CombatParticipantRef CurrentTarget { get; private set; }

        public ICardUsedListener CardUsedListener;

        [SerializeField] private float highlightTime = 1f;
        public bool isCardSelected = false;

        private RectTransform selectedCardPivot;
        private RectTransform selectionScreenPivot;
        private RectTransform abilityCardHolder;
        public UnityEvent OnReleased = null;
        private UIBlock2D cardBlock;
        private UIBlock2D pivotBlock;

        private Vector3 cardPos;
        private Vector3 cardSize;
        private Vector3 pivotPos;
        private Vector3 screenPos;

        [SerializeField] private float opacityOnSelect = 0.5f;
        private Color color;
        private RadialGradient gradient;
        private Color border;

        private bool isReleased = false;
        private bool hasTarget = false;
    

        private void OnEnable()
        {
            abilityCardHolder = GetComponentInParent<AbilityCardHolder>().GetComponent<RectTransform>();
            View.UIBlock.AddGestureHandler<Gesture.OnRelease, ButtonVisuals>(HandleReleased);
            cardBlock = GetComponent<UIBlock2D>();
            selectedCardPivot = FindFirstObjectByType<SelectedCardPivot>().GetComponent<RectTransform>();
            selectionScreenPivot = FindFirstObjectByType<SelectionScreen>().GetComponent<RectTransform>();
            pivotBlock = selectionScreenPivot.GetComponent<UIBlock2D>();
            pivotPos = pivotBlock.transform.position;

            GetInitPosData();
            GetInitColorData();
        }

        private void OnDisable()
        {
            View.UIBlock.RemoveGestureHandler<Gesture.OnRelease, ButtonVisuals>(HandleReleased);
        }

        private void HandleReleased(Gesture.OnRelease evt, ButtonVisuals visuals) => OnReleased?.Invoke();

        private void Update()
        {
            //CheckTarget();

            if (!isCardSelected)
            {
                return;
            }

            if (isCardSelected) 
            {
                OnCardDrag();
                CheckTarget();
            }
        }

        public void OnCardRelease()
        {
            if (!isReleased)
            {
                isReleased = true;

                SetInitPos();
                SetInitColor();

                isCardSelected = false;
                ReturnToDeck();

                var render = GetComponentInChildren<CardRender>();

                if (render == null)
                {
                    UnityEngine.Debug.LogWarning($"Unable to find a {nameof(CardRender)} on {gameObject}");
                    return;
                }

                CheckTarget();
                CardUsedListener?.OnCardUsed(render.Ability, CurrentTarget);
            }
        }

        private void ReturnToDeck()
        {
            transform.SetParent(abilityCardHolder, false);
            transform.localScale = Vector3.one;
            transform.rotation = Quaternion.identity;
        }

        public void OnCardDrag()
        {
            isReleased = false;
            transform.SetParent(selectedCardPivot, true);

            cardBlock.Position.XY.Value = new Vector2(0,0);

            if (GetMousePosition.TryGetCurrentRay(out var ray) && GetMousePosition.TryProjectRay(ray, out var worldPos))
            {
                selectionScreenPivot = selectedCardPivot.GetComponentInParent<SelectionScreen>().GetComponent<RectTransform>();
                selectionScreenPivot.position = worldPos;
                SetSelectedColor();
                isCardSelected = true;
            }
        }

        public void GetInitPosData()
        {
            cardPos = cardBlock.Position.Value;
            cardSize = cardBlock.Size.Value;
        }

        private void SetInitPos()
        {
            pivotBlock.transform.position = pivotPos;
            cardBlock.Position.Y.Value = cardPos.y;
            cardBlock.Position.Z.Value = cardPos.z;
            cardBlock.Size.Value = cardSize;
        }

        public void GetInitColorData()
        {
            color = cardBlock.Color;
            gradient = cardBlock.Gradient;
            border = cardBlock.Border.Color;
        }

        private void SetSelectedColor()
        {
            cardBlock.Color = new Color(color.r, color.g, color.b, opacityOnSelect);
        }

        private void SetInitColor()
        {
            cardBlock.Color = color;
        }

        private void CheckTarget()
        {
            var mousePos = UnityEngine.Input.mousePosition;

            var ray = Camera.main.ScreenPointToRay(mousePos);

            RaycastHit raycastHit;

            var isHit = Physics.Raycast(ray, out raycastHit);

            if (isHit && raycastHit.transform.CompareTag("Target") && isCardSelected && !hasTarget)
            {
                CurrentTarget = raycastHit.transform.GetComponentInParent<CombatParticipantRef>();
                //Debug.Log(raycastHit.transform.name);
                HighlightTarget(raycastHit);
            }

            if (CurrentTarget != null && !raycastHit.transform.CompareTag("Target"))
            {
                UnhighlightTargets();
                CurrentTarget = null;
            }

            if (raycastHit.transform.CompareTag("Target") && !isCardSelected)
            {
                UnhighlightTargets();
            }
        }

        private void HighlightTarget(RaycastHit hit)
        {
            hit.transform.GetComponent<MeshRenderer>().enabled = true;
            hasTarget = true;
        }

        private void UnhighlightTargets()
        {
            hasTarget = false;
            var targets = FindObjectsOfType<TargetCollider>();
            foreach (var target in targets)
            {
                target.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
