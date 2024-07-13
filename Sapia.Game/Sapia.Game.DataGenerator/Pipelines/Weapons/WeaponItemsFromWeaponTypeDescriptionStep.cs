using PtahBuilder.BuildSystem.Entities;
using PtahBuilder.BuildSystem.Execution.Abstractions;
using PtahBuilder.BuildSystem.Extensions;
using Sapia.Game.Entities.Items.Weapons;
using Sapia.Game.Services;

namespace Sapia.Game.DataGenerator.Pipelines.Weapons;

public class WeaponItemsFromWeaponTypeDescriptionStep : IStep<WeaponItem>
{
    private readonly IEntityProvider<WeaponType> _weaponTypes;
    private readonly IGoldService _goldService;

    public WeaponItemsFromWeaponTypeDescriptionStep(IEntityProvider<WeaponType> weaponTypes, IGoldService goldService)
    {
        _weaponTypes = weaponTypes;
        _goldService = goldService;
    }

    public Task Execute(IPipelineContext<WeaponItem> context, IReadOnlyCollection<Entity<WeaponItem>> entities)
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

                    var weaponItem = new WeaponItem
                    {
                        Name = splits[0].Trim(),
                        Value = splits.Length > 1 ? _goldService.Parse(splits[1]) : entity.Value,
                        WeaponType = entity.Id
                    };

                    context.AddEntity(weaponItem);

                    description.RemoveAt(i);
                    i--;
                }
            }

            entity.Description = description.ToArray();
        }

        return Task.CompletedTask;
    }
}