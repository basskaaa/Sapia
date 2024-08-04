using Sapia.Game.Types;
using Sapia.Game.Types.Entities;

namespace Sapia.Game.Tests.Configuration.TypeData;

public class TestTypeDataRoot : ITypeDataRoot
{
    public TestTypeDataRoot(ITypeDataProvider<AbilityType> abilities, ITypeDataProvider<ClassType> classes)
    {
        Abilities = abilities;
        Classes = classes;
    }

    public ITypeDataProvider<AbilityType> Abilities { get; }
    public ITypeDataProvider<ClassType> Classes { get; }
}