using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Sapia.Game.Tests.Configuration.Fixtures;

namespace Sapia.Game.Tests.Configuration;

public class AutoNSubstituteDataAttribute : AutoDataAttribute
{
    public AutoNSubstituteDataAttribute(params Type[] optionalCustomizationTypes)
        : base(() => BuildFixture(optionalCustomizationTypes))
    {
    }

    // ReSharper disable once ParameterTypeCanBeEnumerable.Local
    private static IFixture BuildFixture(Type[] optionalCustomizationTypes)
    {
        var fixture = new Fixture()
            .Customize(new AutoNSubstituteCustomization());

        foreach (var customizationType in optionalCustomizationTypes)
        {
            fixture.Customize((ICustomization)Activator.CreateInstance(customizationType));
        }

        return fixture;
    }
}

public class SapiaData : AutoNSubstituteDataAttribute
{
    public SapiaData() :
        base(typeof(TypeDataRootFixture))
    {

    }
}