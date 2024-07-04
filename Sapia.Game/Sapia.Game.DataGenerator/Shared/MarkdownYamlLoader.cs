using PtahBuilder.BuildSystem.Config;
using PtahBuilder.BuildSystem.Entities;
using PtahBuilder.BuildSystem.Execution.Abstractions;
using PtahBuilder.BuildSystem.Extensions;
using PtahBuilder.BuildSystem.Services.Serialization;
using Sapia.Game.Entities.Interfaces;

namespace Sapia.Game.DataGenerator.Shared;

public class MarkdownYamlLoader<T> : IStep<T> where T : IHasDescription
{
    private readonly IFilesConfig _filesConfig;
    private readonly IYamlService _yamlService;
    private readonly string _directory;
    private readonly Dictionary<string, string>? _nodeNameToPropertyMappings;

    public MarkdownYamlLoader(IFilesConfig filesConfig, IYamlService yamlService, string directory, Dictionary<string, string>? nodeNameToPropertyMappings = null)
    {
        _filesConfig = filesConfig;
        _directory = directory;
        _yamlService = yamlService;
        _nodeNameToPropertyMappings = nodeNameToPropertyMappings;
    }

    public Task Execute(IPipelineContext<T> context, IReadOnlyCollection<Entity<T>> entities)
    {
        foreach (var file in Directory.GetFiles(Path.Combine(_filesConfig.DataDirectory, _directory), "*.md"))
        {
            ProcessFile(context, file);
        }

        return Task.CompletedTask;
    }

    private void ProcessFile(IPipelineContext<T> context, string file)
    {
        var text = File.ReadAllText(file);

        var parts = text.Split("---", StringSplitOptions.RemoveEmptyEntries);

        var (entity, metadata) = _yamlService.DeserializeAndGetMetadata<T>(parts[0], _nodeNameToPropertyMappings);

        entity.Name = Path.GetFileNameWithoutExtension(file).Trim();
        entity.Description = parts[1].Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

        context.AddEntityFromFile(entity, file, metadata);
    }
}