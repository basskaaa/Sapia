using Sapia.Game.Entities.Interfaces;

namespace Sapia.Game.Entities.Items.Weapons;

public class WeaponItem : IHasDescription
{
    public string Id { get; set; } = string.Empty;

    public string WeaponType { get; set; } = string.Empty;
    public string Masterwork{ get; set; } = string.Empty;
    public string Range { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public float Value { get; set; }

    public string[] Description { get; set; } = Array.Empty<string>();
    public string[] Tags { get; set; } = Array.Empty<string>();
}