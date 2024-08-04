using PtahBuilder.BuildSystem.Config;
using PtahBuilder.BuildSystem.Services.Serialization;
using PtahBuilder.BuildSystem.Steps.Output;
using Sapia.Game.DataGenerator.Shared;
using Sapia.Game.Types.Entities;

namespace Sapia.Game.DataGenerator.Pipelines.Abilities;

public static class AbilitiesPipeline
{
    public static void AddAbilitiesPipeline(this PhaseAddContext phase)
    {
        phase.AddPipeline<AbilityType>(p =>
        {
            var settings = new YamlDeserializationSettings
            {
                UnmatchedPropertyAction = UnmatchedPropertyAction.Warn,

                PropertySettings = new()
                {
                    { nameof(AbilityType.Action), new()
                        {
                            PreProcess = s=>s.Replace("Action", string.Empty).Trim()
                        }
                    }
                }
            };

            p.AddInputStep<MarkdownYamlLoader<AbilityType>>("Abilities", settings);

            p.AddOutputStep<JsonOutputStep<AbilityType>>();
        });
    }
}