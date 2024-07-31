using System.Collections.Generic;
using System.Linq;
using Assets._Scripts.TypeData;
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
            var theRockConfiguration = new CharacterConfiguration
            {
                Name = "Dwayne 'The Rock' Johnson",
                LevelConfigurations = new()
                {
                    {1, new()
                    {
                        ClassId = "Fighter"
                    }},
                    {2, new()
                    {
                        ClassId = "Fighter"
                    }}
                }
            };

            var typeData = TypeDataFactory.CreateTypeData();
            var characterStatusService = new CharacterService(typeData);

            var theRock = characterStatusService.CompileCharacter(theRockConfiguration, new[] { "Jab", "Slash" });

            ICompiledCharacter CreateSkeleton() =>  new SimpleCharacter("Skeleton", new CharacterStats(3))
            {
                Abilities = new[] { new PreparedAbility("Slash") }
            };

            var participantRefs = FindObjectsByType<CombatParticipantRef>( FindObjectsInactive.Include, FindObjectsSortMode.None);

            var participants = participantRefs.Select(x =>
            {
                var pos = new Coord((int)x.transform.position.x, (int)x.transform.position.z);
                var id = x.ParticipantId;

                var character = x.ParticipantId == "Player" ? theRock : CreateSkeleton();

                return new CombatFactory.CombatParticipantEntry(id, character, id == "Player" ? 20 : 5, pos);
            });

            var combat = CombatFactory.Create(typeData, participants);

            foreach (var combatParticipantRef in participantRefs)
            {
                combatParticipantRef.JoinCombat(this, combat);
            }

            return combat;
        }

        public void Step()
        {
            if (_combat.Step())
            {
                ShowDebugText();
            }
            else
            {
                ShowDebugText("Combat finished");
            }

            RaiseStepChanged();
        }

        private void RaiseStepChanged()
        {
            foreach (var combatListener in _listeners)
            {
                combatListener.StepChanged(_combat, _combat.CurrentStep);
            }
        }

        public void Act()
        {
            AutoRunStep();
        }

        private void AutoRunStep()
        {
            if (_combat == null || _combat.CurrentStep == null)
            {
                return;
            }

            var turn = _combat.CurrentStep as TurnStep;

            if (turn == null)
            {
                return;
            }

            if (turn.Abilities.Count == 0)
            {
                turn.EndTurn();
                ShowDebugText("Ended turn", "Use Step to advance combat");
                RaiseStepChanged();
                return;
            }

            CombatParticipant FindTargetFor(CombatParticipant participant)
            {
                foreach (var other in _combat.Participants.All)
                {
                    if (other.Character.IsPlayer != participant.Character.IsPlayer && other.Character.IsAlive)
                    {
                        return other;
                    }
                }

                return null;
            }

            var ability = turn.Abilities.Last();

            var target = FindTargetFor(turn.Participant);

            if (target != null)
            {
                var abilityId = ability.AbilityType.Id;
                var targetParticipantId = target.ParticipantId;

                UseAbilityInCurrentTurn(turn, abilityId, targetParticipantId);
            }
            else
            {
                ShowDebugText($"Unable to find a target for {ability.AbilityType.Id}");
            }
        }

        private void UseAbilityInCurrentTurn(TurnStep turn, string abilityId, string targetParticipantId)
        {
            var result = turn.UseAbility(new TargetedAbilityUse(abilityId, targetParticipantId));

            _combat.Step();

            if (result.HasValue)
            {
                var targetInfo = result.Value.AffectedParticipants.Select(x => $"{x.ParticipantId} {x.HealthChange}").ToArray();
                ShowDebugText($"Used {result.Value.Ability.Id}: {string.Join(", ", targetInfo)}");
            }
            else
            {
                ShowDebugText($"Failed to use {abilityId}");
            }

            RaiseStepChanged();
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
