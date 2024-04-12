using PtahBuilder.BuildSystem.Config;
using PtahBuilder.BuildSystem.Steps.Output;
using Sapia.Game.DataGenerator.Shared;
using Sapia.Game.Entities.Items.Weapons;

namespace Sapia.Game.DataGenerator.Pipelines.Weapons
{
    public static class WeaponTypePipeline
    {
        public static void AddWeaponTypePipeline(this PhaseAddContext phase)
        {
            phase.AddPipeline<WeaponType>(p =>
            {
                p.AddInputStep<MarkdownYamlLoader<WeaponType>>("Rules/2. Character Options/6. Gear/Weapons/Weapon Types", new Dictionary<string, string>()
                {
                    {"Weapon Range", "Range" },
                    {"Value (gp)", "Value"},
                    {"Item tags", "Tags"},
                    {"Damage Die", "DamageDie"}
                });

                p.AddOutputStep<JsonOutputStep<WeaponType>>();
            });
        }
    }
}
