using Sapia.Game.Hack.Types;

namespace Sapia.Game.TestConsole;

public class TestTypeDataRoot : ITypeDataRoot
{
    public TestTypeDataRoot(ITypeDataProvider<AbilityType> abilities, ITypeDataProvider<ClassType> classes)
    {
        Abilities = abilities;
        Classes = classes;
    }

    public ITypeDataProvider<AbilityType> Abilities { get;  }
    public ITypeDataProvider<ClassType> Classes { get;  }
}