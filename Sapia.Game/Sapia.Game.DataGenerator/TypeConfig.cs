using PtahBuilder.BuildSystem;
using Sapia.Game.Structs.Dice;

namespace Sapia.Game.DataGenerator;

public static class TypeConfig
{
    public static BuilderFactory AddSapiaTypeHandling(this BuilderFactory builderFactory)
    {
        return builderFactory.AddCustomValueParser(typeof(IDiceValue), v => DiceParser.Parse(v!.ToString()!));
    }
}