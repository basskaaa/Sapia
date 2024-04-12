using PtahBuilder.BuildSystem.Entities;
using PtahBuilder.BuildSystem.Execution.Abstractions;
using PtahBuilder.Util.Extensions;

namespace Sapia.Game.DataGenerator.Shared;

public class ReferenceFixStep<T> : IStep<T>
{
    private readonly string[] _propertyNames;

    public ReferenceFixStep(List<string> propertyNames)
    {
        _propertyNames = propertyNames.ToArray();
    }

    public Task Execute(IPipelineContext<T> context, IReadOnlyCollection<Entity<T>> entities)
    {
        var relevantProperties = typeof(T).GetProperties().Where(x => _propertyNames.Contains(x.Name)).ToArray();

        foreach (var entity in entities)
        {
            var data = entity.Value;

            foreach (var relevantProperty in relevantProperties)
            {
                var val = relevantProperty.GetValue(data)?.ToString();

                if (!string.IsNullOrWhiteSpace(val))
                {
                    val = val.Replace("[", string.Empty)
                        .Replace("]", string.Empty)
                        .ToSlug();

                    relevantProperty.SetValue(data, val);
                }
            }
        }

        return Task.CompletedTask;
    }
}