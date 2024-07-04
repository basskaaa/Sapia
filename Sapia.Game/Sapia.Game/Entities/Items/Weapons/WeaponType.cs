using Sapia.Game.Entities.Interfaces;
using Sapia.Game.Structs.Dice;

namespace Sapia.Game.Entities.Items.Weapons;

public class WeaponType : IHasDescription
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public float Value { get; set; }

    public string[] Description { get; set; } = Array.Empty<string>();
    public string[] Tags { get; set; } = Array.Empty<string>();

    public IDiceValue DamageDie { get; set; } = new Dice(4);

    public string Weight { get; set; } = string.Empty;
    public string Range { get; set; } = string.Empty;
}