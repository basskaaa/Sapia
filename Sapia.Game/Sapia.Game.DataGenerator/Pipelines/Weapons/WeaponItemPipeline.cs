using PtahBuilder.BuildSystem.Config;
using PtahBuilder.BuildSystem.Steps.Output;
using Sapia.Game.DataGenerator.Shared;
using Sapia.Game.Types.Entities;

namespace Sapia.Game.DataGenerator.Pipelines.Weapons;

public static class WeaponTypePipeline
{
    public static void AddWeaponTypePipeline(this PhaseAddContext phase)
    {
        phase.AddPipeline<WeaponType>(p =>
        {
            p.AddInputStep<MarkdownYamlLoader<WeaponType>>("Weapons");

            p.AddInputStep<WeaponTypeFromWeaponStatsLoadDescriptionStep>();

            p.AddProcessStep<SetWeaponStatsStep>();

            p.AddOutputStep<JsonOutputStep<WeaponType>>();
        });
    }
}