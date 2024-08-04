using Microsoft.Extensions.DependencyInjection;
using PtahBuilder.BuildSystem;
using PtahBuilder.BuildSystem.Config;
using PtahBuilder.Plugins.NewtonsoftJson;
using PtahBuilder.Util.Helpers;
using Sapia.Game.DataGenerator;
using Sapia.Game.DataGenerator.JsonConverters;
using Sapia.Game.DataGenerator.Pipelines.Weapons;
using Sapia.Game.Services;

const string DataDirectory = "Sapia.Obsidian";
const string FullDataDirectory = "Sapia.Obsidian\\August Prototype";

await new BuilderFactory()
    .UseNewtonsoftJson(typeof(DiceJsonConverter).Assembly)
    .ConfigureFiles(f =>
    {
        f.Configure(PathHelper.GetRootPath(DataDirectory, args), relativeDataDirectory: FullDataDirectory);
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton<IGoldService, GoldService>();
    })
    .AddJsonConverterTypes(typeof(Program).Assembly)
        .AddSapiaTypeHandling()
        .ConfigureExecution(x =>
        {
            x.DeleteOutputDirectory = true;
            x.MissingIdPreference = MissingIdPreference.SourceFile;

            // Load any shared or build data
            x.AddPipelinePhase(phase =>
            {
                phase.AddWeaponStatsPipeline();
            });

            // Load actual entities
            x.AddPipelinePhase(phase =>
            {
                phase.AddWeaponTypePipeline();
                phase.AddAbilitiesPipeline();
            });

        })
        .Run();