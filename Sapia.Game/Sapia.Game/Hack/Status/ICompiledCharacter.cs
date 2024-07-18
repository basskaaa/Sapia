﻿namespace Sapia.Game.Hack.Status;

public interface ICompiledCharacter
{
    int CurrentHealth { get; set; }
    public CharacterStats Stats { get; }
    IReadOnlyCollection<PreparedAbility> Abilities { get; }
    int TotalLevel { get; }

    bool IsPlayer { get; }
}