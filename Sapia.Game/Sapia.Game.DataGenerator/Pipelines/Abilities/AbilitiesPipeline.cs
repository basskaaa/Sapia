using Humanizer;
using PtahBuilder.BuildSystem.Config;
using PtahBuilder.BuildSystem.Entities;
using PtahBuilder.BuildSystem.Steps.Output;
using PtahBuilder.BuildSystem.Steps.Process;
using Sapia.Game.DataGenerator.Shared;
using Sapia.Game.Types.Entities;

namespace Sapia.Game.DataGenerator.Pipelines.Weapons;

public static class AbilitiesPipeline
{
    public static void AddAbilitiesPipeline(this PhaseAddContext phase)
    {
        phase.AddPipeline<AbilityType>(p =>
        {
            p.DuplicateIdBehaviour = DuplicateIdBehaviour.ReturnExistingEntity;

            p.AddInputStep<MarkdownYamlLoader<AbilityType>>("Abilities");
            
            p.AddOutputStep<JsonOutputStep<AbilityType>>();
        });
    }
}