using Microsoft.Extensions.DependencyInjection;
using PtahBuilder.BuildSystem;
using PtahBuilder.BuildSystem.Config;
using PtahBuilder.Plugins.NewtonsoftJson;
using PtahBuilder.Util.Helpers;
using Sapia.Game.DataGenerator;
using Sapia.Game.DataGenerator.JsonConverters;
using Sapia.Game.DataGenerator.Pipelines.Weapons;
using Sapia.Game.Services;

const string DataDirectory = "Sapia (Obsidian Vault)";

await new BuilderFactory()
    .UseNewtonsoftJson(typeof(DiceJsonConverter).Assembly)
    .ConfigureFiles(f =>
    {
        f.Configure(PathHelper.GetRootPath(DataDirectory, args), relativeDataDirectory: DataDirectory);
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton<IGoldService, GoldService>();
    })
    .AddJsonConverterTypes(typeof(Program).Assembly)
        .AddFusionShiftTypeHandling()
        .ConfigureExecution(x =>
        {
            x.DeleteOutputDirectory = true;
            x.MissingIdPreference = MissingIdPreference.SourceFile;

            x.AddPipelinePhase(phase =>
            {
                phase.AddWeaponTypePipeline();
                phase.AddWeaponItemPipeline();
            });

        })
        .Run();