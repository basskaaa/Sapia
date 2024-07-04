namespace Sapia.Game.Services;

public class GoldService  :IGoldService
{
    public float Parse(string value)
    {
        value = value.ToLower();

        if (value.EndsWith("gp"))
        {
            return Convert.ToSingle(value.Substring(0, value.Length - 2));
        }

        if (value.EndsWith("sp"))
        {
            return Convert.ToSingle(value.Substring(0, value.Length - 2)) / 10;
        }

        if (value.EndsWith("cp"))
        {
            return Convert.ToSingle(value.Substring(0, value.Length - 2)) / 100;
        }

        if (float.TryParse(value, out var g))
        {
            return g;
        }

        throw new InvalidOperationException($"Unable to parse \"{value}\" as gp value");
    }
}