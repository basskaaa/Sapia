using Sapia.Game.Structs.Dice;
using Sapia.Game.Types.Enums;

namespace Sapia.Game.Types.Entities;

public class WeaponType : TypeDataWithDescription
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public float Value { get; set; }

    public string[] Description { get; set; } = Array.Empty<string>();
    public string[] Tags { get; set; } = Array.Empty<string>();

    public IDiceValue Damage { get; set; } = new Dice(4);

    public string Type { get; set; } = string.Empty;
    public WeaponWeight Weight { get; set; } = WeaponWeight.Medium;
    public int Range { get; set; } = 1;

    public string Masterwork { get; set; } = string.Empty;
}