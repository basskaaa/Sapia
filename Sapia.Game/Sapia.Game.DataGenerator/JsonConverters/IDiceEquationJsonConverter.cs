﻿using Newtonsoft.Json;
using Sapia.Game.Structs.Dice;

namespace Sapia.Game.DataGenerator.JsonConverters;

public class DiceEquationJsonConverter : JsonConverter<IDiceValue>
{
    public override void WriteJson(JsonWriter writer, IDiceValue? value, JsonSerializer serializer)
    {
        if (value != null)
        {
            writer.WriteValue(value.ToString());
        }
    }

    public override IDiceValue ReadJson(JsonReader reader, Type objectType, IDiceValue? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.Value == null)
        {
            throw new ArgumentNullException(nameof(reader.Value));
        }

        var s = (string)reader.Value;

        return DiceParser.Parse(s);
    }
}