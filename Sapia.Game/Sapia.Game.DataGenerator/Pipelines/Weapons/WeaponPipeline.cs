using Humanizer;
using PtahBuilder.BuildSystem.Config;
using PtahBuilder.BuildSystem.Entities;
using PtahBuilder.BuildSystem.Steps.Output;
using PtahBuilder.BuildSystem.Steps.Process;
using Sapia.Game.DataGenerator.Shared;
using Sapia.Game.Types.Entities;

namespace Sapia.Game.DataGenerator.Pipelines.Weapons;

public static class WeaponTypePipeline
{
    public static void AddWeaponTypePipeline(this PhaseAddContext phase)
    {
        phase.AddPipeline<WeaponType>(p =>
        {
            p.DuplicateIdBehaviour = DuplicateIdBehaviour.ReturnExistingEntity;

            p.AddInputStep<MarkdownYamlLoader<WeaponType>>("Weapons");

            p.AddInputStep<WeaponTypeFromWeaponStatsLoadDescriptionStep>();

            p.AddProcessStep<SetWeaponStatsStep>();

            p.AddOutputStep<JsonOutputStep<WeaponType>>();
        });
    }
    public static void AddWeaponStatsPipeline(this PhaseAddContext phase)
    {
        phase.AddPipeline<WeaponStatsLoad>(p =>
        {
            p.AddInputStep<MarkdownYamlLoader<WeaponStatsLoad>>("../Rules/2. Character Options/6. Gear/Weapons/Weapon Types");

            p.AddProcessStep<ProcessStep<WeaponStatsLoad>>((Entity<WeaponStatsLoad> entity) =>
            {
                if (string.IsNullOrWhiteSpace(entity.Value.Type))
                {
                    entity.Value.Type = entity.Id.Split('-').Last().Humanize();
                }
            });
        });
    }
}