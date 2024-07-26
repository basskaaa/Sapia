using System.Collections;
using Assets._Scripts.Game;
using Assets._Scripts.Input;
using Assets._Scripts.Patterns;
using Nova;
using NovaSamples.UIControls;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._Scripts.Ui
{
    public class CardSelect : UIControl<ButtonVisuals>
    {
        public CombatParticipantRef CurrentTarget { get; private set; }

        public ICardUsedListener CardUsedListener;

        [SerializeField] private float highlightTime = 1f;
        public bool isCardSelected = false;

        private Transform selectedCardPivot;
        private Transform selectionScreenPivot;
        private Transform abilityCardHolder;
        public UnityEvent OnReleased = null;
        private UIBlock2D cardBlock;

        private Vector3 cardPos;
        private Vector3 pivotPos;
        private Vector3 screenPos;

        [SerializeField] private float opacityOnSelect = 0.5f;
        private Color color;
        private RadialGradient gradient;
        private Color border;
        private bool isTargetHighlighted = false;
    

        private void OnEnable()
        {
            abilityCardHolder = GetComponentInParent<AbilityCardHolder>().transform;
            View.UIBlock.AddGestureHandler<Gesture.OnRelease, ButtonVisuals>(HandleReleased);
            cardBlock = GetComponent<UIBlock2D>();
            selectedCardPivot = FindFirstObjectByType<SelectedCardPivot>().transform;
            selectionScreenPivot = FindFirstObjectByType<SelectionScreen>().transform;

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
            UnityEngine.Debug.Log("Released");

            CheckTarget();

            SetInitPos();
            SetInitColor();

            isCardSelected = false;
            transform.SetParent(abilityCardHolder, true);
      
            var render = GetComponentInChildren<CardRender>();

            if (render == null)
            {
                UnityEngine.Debug.LogWarning($"Unable to find a {nameof(CardRender)} on {gameObject}");
                return;
            }

            CardUsedListener?.OnCardUsed(render.Ability, CurrentTarget);
        }

        public void OnCardDrag()
        {
            transform.SetParent(selectedCardPivot, true);

            if (GetMousePosition.Instance.TryGetCurrentRay(out Ray ray) && GetMousePosition.Instance.TryProjectRay(ray, out Vector3 worldPos))
            {
                selectionScreenPivot = selectedCardPivot.GetComponentInParent<SelectionScreen>().transform;
                selectionScreenPivot.position = worldPos;
                SetSelectedColor();
                isCardSelected = true;
            }
        }

        public void GetInitPosData()
        {
            pivotPos = selectionScreenPivot.position;
            cardPos = cardBlock.transform.position;
        }

        private void SetInitPos()
        {
            cardBlock.transform.position = cardPos;
            selectionScreenPivot.position = pivotPos;
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
            Vector3 mousePos = UnityEngine.Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            RaycastHit raycastHit;

            bool isHit = Physics.Raycast(ray, out raycastHit);

            if (isHit && raycastHit.transform.CompareTag("Target") && !isTargetHighlighted && isCardSelected)
            {
                CurrentTarget = raycastHit.transform.GetComponentInParent<CombatParticipantRef>();
                //Debug.Log(raycastHit.transform.name);
                HighlightTarget(raycastHit);
            }

            if (!isCardSelected)
            {
                UnhighlightTarget(raycastHit);
            }
        }

        private void HighlightTarget(RaycastHit hit)
        {
            hit.transform.GetComponent<MeshRenderer>().enabled = true;
            isTargetHighlighted = true;
        }

        private void UnhighlightTarget(RaycastHit hit)
        {
            hit.transform.GetComponent<MeshRenderer>().enabled = false;
            isTargetHighlighted = false;
        }
    }
}
