using System;
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
using Sapia.Game.Types;
using UnityEngine;

namespace Assets._Scripts.Game
{
    public class CombatRunner : MonoBehaviour
    {
        private DebugText _debug;

        private CombatBag _combatBag;
        private CombatStep _currentStep;
        private HashSet<ICombatListener> _listeners = new();

        void Start()
        {
            _debug = FindFirstObjectByType<DebugText>();

            _combatBag = CreateCombat();
            Step();

            ShowDebugText("Combat runner has started");
        }

        private CombatBag CreateCombat()
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

            var executor = new CombatExecutor(combat);
            
            return new CombatBag(typeData, executor, combat.Participants);
        }

        public void Step()
        {
            if (_combatBag.Execution.MoveNext())
            {
                _currentStep = _combatBag.Execution.Current;
                ShowDebugText();
            }
            else
            {
                _currentStep = null;
                ShowDebugText("Combat finished");
            }

            RaiseStepChanged();
        }

        private void RaiseStepChanged()
        {
            foreach (var combatListener in _listeners)
            {
                combatListener.StepChanged(_combatBag.Combat, _currentStep);
            }
        }

        public void Act()
        {
            AutoRunStep();
        }

        private void AutoRunStep()
        {
            if (_combatBag == null || _currentStep == null)
            {
                return;
            }

            var turn = _currentStep as TurnStep;

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
                foreach (var other in _combatBag.Combat.Participants)
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

            if (_combatBag.Execution.MoveNext())
            {
                _currentStep = _combatBag.Execution.Current;
            }

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

        public void Move(string participantId, Vector3 worldPos)
        {
            if (_currentStep is TurnStep turn && participantId == turn.Participant.ParticipantId)
            {
                var coord = new Coord((int)worldPos.x, (int)worldPos.z);

                if (turn.TryMove(coord))
                {
                    Step();
                }
                else
                {
                    UnityEngine.Debug.Log("Failed to move");
                }
            }
        }

        public void UseAbility(string userParticipantId, UsableAbility ability, string targetParticipantId)
        {
            if (_currentStep is TurnStep turn && turn.Participant.ParticipantId == userParticipantId)
            {
                UseAbilityInCurrentTurn(turn, ability.AbilityType.Id, targetParticipantId);
            }
        }

        public class CombatBag
        {
            public CombatBag(ITypeDataRoot typeData, CombatExecutor combatExecutor, IReadOnlyCollection<CombatParticipant> participants)
            {
                TypeData = typeData;
                CombatExecutor = combatExecutor;
                Participants = participants;
                Execution = combatExecutor.Execute();
            }

            public IReadOnlyCollection<CombatParticipant> Participants { get; }
            public ITypeDataRoot TypeData { get; }
            public CombatExecutor CombatExecutor { get; }
            public IEnumerator<CombatStep> Execution { get; }
            public Combat Combat => CombatExecutor.Combat;
        }


        private void ShowDebugText(params string[] text)
        {
            var textToShow = new List<string>();

            if (_combatBag != null)
            {
                textToShow.Add($"Combat round: {_combatBag.Combat.CurrentRound}");

                foreach (var participant in _combatBag.Participants)
                {
                    var participantText = $"{participant.ParticipantId}: {participant.Character.CurrentHealth} / {participant.Character.Stats.MaxHealth}";
                    textToShow.Add(participantText);
                }
            }

            if (_currentStep != null)
            {
                textToShow.Add(_currentStep.ToString());
                if (_currentStep is TurnStep turn)
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
