﻿namespace Sapia.Game.Types;

public interface ITypeDataRoot
{
    ITypeDataProvider<AbilityType> Abilities { get; }
    ITypeDataProvider<ClassType> Classes { get; }
}