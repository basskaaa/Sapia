using PtahBuilder.BuildSystem.Entities;
using PtahBuilder.BuildSystem.Execution.Abstractions;
using PtahBuilder.BuildSystem.Extensions;
using Sapia.Game.Services;
using Sapia.Game.Types.Entities;

namespace Sapia.Game.DataGenerator.Pipelines.Weapons;

public class WeaponTypeFromWeaponStatsLoadDescriptionStep : IStep<WeaponType>
{
    private readonly IEntityProvider<WeaponStatsLoad> _weaponTypes;
    private readonly IGoldService _goldService;

    public WeaponTypeFromWeaponStatsLoadDescriptionStep(IEntityProvider<WeaponStatsLoad> weaponTypes, IGoldService goldService)
    {
        _weaponTypes = weaponTypes;
        _goldService = goldService;
    }

    public Task Execute(IPipelineContext<WeaponType> context, IReadOnlyCollection<Entity<WeaponType>> entities)
    {
        foreach (var entity in _weaponTypes.ToArray())
        {
            var description = entity.Description.ToList();

            for (int i = 0; i < description.Count; i++)
            {
                var line = description[i];

                if (line.StartsWith("-"))
                {
                    var splits = line.Substring(1).Split(".", StringSplitOptions.RemoveEmptyEntries);

                    var weaponType = new WeaponType
                    {
                        Name = splits[0].Trim(),
                        Value = splits.Length > 1 ? _goldService.Parse(splits[1]) : entity.Value
                    };

                    context.AddEntity(weaponType);

                    description.RemoveAt(i);
                    i--;
                }
            }

            entity.Description = description.ToArray();
        }

        return Task.CompletedTask;
    }
}