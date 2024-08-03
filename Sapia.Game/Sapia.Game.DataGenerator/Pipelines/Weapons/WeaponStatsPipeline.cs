using PtahBuilder.BuildSystem.Config;
using PtahBuilder.BuildSystem.Steps.Output;
using Sapia.Game.DataGenerator.Shared;

namespace Sapia.Game.DataGenerator.Pipelines.Weapons;

public static class WeaponStatsPipeline
{
    public static void AddWeaponStatsPipeline(this PhaseAddContext phase)
    {
        phase.AddPipeline<WeaponStatsLoad>(p =>
        {
            p.AddInputStep<MarkdownYamlLoader<WeaponStatsLoad>>("../Rules/2. Character Options/6. Gear/Weapons/Weapon Types");

            // Only output for validating
            p.AddOutputStep<JsonOutputStep<WeaponStatsLoad>>();
        });
    }
}