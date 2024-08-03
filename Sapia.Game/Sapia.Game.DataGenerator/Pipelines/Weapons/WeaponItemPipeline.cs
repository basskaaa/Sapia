using PtahBuilder.BuildSystem.Config;
using PtahBuilder.BuildSystem.Steps.Output;
using PtahBuilder.BuildSystem.Steps.Process;
using Sapia.Game.DataGenerator.Shared;
using Sapia.Game.Types.Entities;

namespace Sapia.Game.DataGenerator.Pipelines.Weapons;

public static class WeaponItemPipeline
{
    public static void AddWeaponItemPipeline(this PhaseAddContext phase)
    {
        phase.AddPipeline<WeaponItem>(p =>
        {
            p.AddInputStep<MarkdownYamlLoader<WeaponItem>>("Rules/2. Character Options/6. Gear/Weapons/Weapon Items", new Dictionary<string, string>()
            {
                {"Weapon Range", "Range" },
                {"Value (gp)", "Value"},
                {"Item tags", "Tags"},
                {"Weapon Type", "WeaponType"}
            });

            p.AddInputStep<WeaponItemsFromWeaponTypeDescriptionStep>();

            p.AddProcessStep<ReferenceFixStep<WeaponItem>>(new[] { nameof(WeaponItem.WeaponType) }.ToList());

            p.AddOutputStep<ValidateEntityReferenceStep<WeaponItem, WeaponType>>(new ValidationConfig<WeaponItem>()
            {
                IsRequired = true,
                PropertyName = nameof(WeaponItem.WeaponType)
            });

            p.AddOutputStep<JsonOutputStep<WeaponItem>>();
        });
    }
}