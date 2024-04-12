using Sapia.Game.Structs.Dice;

namespace Sapia.Game.Entities.Items.Weapons
{
    public class WeaponType : ItemType
    {
        public IDiceValue DamageDie { get; set; } = new Dice(4);

        public string Weight { get; set; } = string.Empty;
        public string Range { get; set; } = string.Empty;
    }
}
