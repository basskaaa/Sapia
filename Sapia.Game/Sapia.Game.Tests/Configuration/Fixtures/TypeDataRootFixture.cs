using AutoFixture;
using Sapia.Game.Tests.Configuration.TypeData;
using Sapia.Game.Types;

namespace Sapia.Game.Tests.Configuration.Fixtures;

public class TypeDataRootFixture : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Register<ITypeDataRoot>(TypeDataFactory.CreateTypeData);
    }
}