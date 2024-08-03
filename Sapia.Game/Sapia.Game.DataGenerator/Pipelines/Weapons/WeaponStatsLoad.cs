using Sapia.Game.Structs.Dice;
using Sapia.Game.Types.Enums;
using Sapia.Game.Types.Interfaces;

namespace Sapia.Game.DataGenerator.Pipelines.Weapons
{
    public class WeaponStatsLoad : IHasDescription
    {
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public WeaponWeight Weight { get; set; }

        public float Value { get; set; }
        public Dice Damage { get; set; } = new Dice(4);

        public string Name { get; set; }
        public string[] Description { get; set; } = Array.Empty<string>();
    }
}
