using Newtonsoft.Json;
using Sapia.Game.Structs.Dice;

namespace Sapia.Game.DataGenerator.JsonConverters;

public class DiceJsonConverter : JsonConverter<Dice>
{
    public override void WriteJson(JsonWriter writer, Dice value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }

    public override Dice ReadJson(JsonReader reader, Type objectType, Dice existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.Value == null)
        {
            throw new ArgumentNullException(nameof(reader.Value));
        }

        var s = (string)reader.Value;

        return Dice.Parse(s);
    }
}