using System.Collections.Generic;
using System.Linq;
using Assets._Scripts.TypeData;
using Assets._Scripts.Ui;
using Assets._Scripts.Ui.Test;
using Sapia.Game.Characters;
using Sapia.Game.Characters.Configuration;
using Sapia.Game.Combat;
using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Steps;
using Sapia.Game.Structs;
using UnityEngine;

namespace Assets._Scripts.Game
{
    public class CombatRunner : MonoBehaviour
    {
        private DebugText _debug;

        private Combat _combat;
        private HashSet<ICombatListener> _listeners = new();

        void Start()
        {
            _debug = FindFirstObjectByType<DebugText>();

            _combat = CreateCombat();
            Step();

            ShowDebugText("Combat runner has started");
        }

        private Combat CreateCombat()
        {
            var participantRefs = FindObjectsByType<CombatParticipantRef>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            var combat = CombatStarter.CreateCombat(participantRefs);

            foreach (var combatParticipantRef in participantRefs)
            {
                combatParticipantRef.JoinCombat(this, combat);
            }

            return combat;
        }

        public void Step()
        {
            _combat.ExecuteAi();

            if (_combat.Step())
            {
                ShowDebugText();
            }
            else
            {
                ShowDebugText("Combat finished");
            }

            foreach (var combatListener in _listeners)
            {
                combatListener.StepChanged(_combat, _combat.CurrentStep);
            }
        }

        public bool Move(string participantId, Coord coord)
        {
            if (_combat.CurrentStep is TurnStep turn && participantId == turn.Participant.ParticipantId)
            {
                if (turn.TryMove(coord))
                {
                    Step();
                    return true;
                }
                else
                {
                    UnityEngine.Debug.Log("Failed to move");
                }
            }

            return false;
        }

        public void UseAbility(string userParticipantId, UsableAbility ability, string targetParticipantId)
        {
            if (_combat.CurrentStep is TurnStep turn && turn.Participant.ParticipantId == userParticipantId)
            {
                UseAbilityInCurrentTurn(turn, ability.AbilityType.Id, targetParticipantId);
            }
        }

        private void UseAbilityInCurrentTurn(TurnStep turn, string abilityId, string targetParticipantId)
        {
            turn.UseAbility(new TargetedAbilityUse(abilityId, targetParticipantId));

            Step();
        }

        private void ShowDebugText(params string[] text)
        {
            var textToShow = new List<string>();

            if (_combat != null)
            {
                textToShow.Add($"Combat round: {_combat.CurrentRound}");

                foreach (var participant in _combat.Participants.All)
                {
                    var participantText = $"{participant.ParticipantId}: {participant.Character.CurrentHealth} / {participant.Character.Stats.MaxHealth}";
                    textToShow.Add(participantText);
                }
            }

            if (_combat?.CurrentStep != null)
            {
                textToShow.Add(_combat.CurrentStep.ToString());
                if (_combat.CurrentStep is TurnStep turn)
                {
                    textToShow.Add($"Remaining Movement: {turn.Participant.Status.RemainingMovement}");
                    textToShow.Add($"Remaining Actions: {string.Join(",", turn.Participant.Status.RemainingActions)}");
                    textToShow.Add($"Available Abilities: {string.Join(",", turn.Abilities.Select(x => x.AbilityType.Id))}");
                }
            }

            textToShow.AddRange(text);

            _debug.Show(textToShow.ToArray());
        }

        public void AddListener(ICombatListener listener)
        {
            _listeners.Add(listener);
        }
    }
}
