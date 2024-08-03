using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PtahBuilder.BuildSystem.Entities;
using PtahBuilder.BuildSystem.Execution.Abstractions;
using Sapia.Game.Types.Entities;

namespace Sapia.Game.DataGenerator.Pipelines.Weapons
{
    public class SetWeaponStatsStep : IStep<WeaponType>
    {
        private readonly IEntityProvider<WeaponStatsLoad> _weaponStats;

        public SetWeaponStatsStep(IEntityProvider<WeaponStatsLoad> weaponStats)
        {
            _weaponStats = weaponStats;
        }

        public Task Execute(IPipelineContext<WeaponType> context, IReadOnlyCollection<Entity<WeaponType>> entities)
        {
            foreach (var entity in entities)
            {
                var weaponStats = _weaponStats.Entities.Values.FirstOrDefault(x =>
                    x.Value.Type == entity.Value.Type &&
                    x.Value.Weight == entity.Value.Weight);

                if (weaponStats == null)
                {
                    context.AddValidationError(entity, this, $"Unable to find a {nameof(WeaponStatsLoad)} for {entity.Value.Weight} {entity.Value.Type}");
                }
                else
                {
                    SetPropertiesFromWeaponStatsLoad(entity.Value, weaponStats.Value);
                }
            }

            return Task.CompletedTask;
        }

        public void SetPropertiesFromWeaponStatsLoad(WeaponType weaponType, WeaponStatsLoad weaponStats)
        {
            weaponType.Type = weaponStats.Type;
            weaponType.Weight = weaponStats.Weight;
            weaponType.Damage = weaponStats.Damage;

            if (weaponType.Value <= 0)
            {
                weaponType.Value = weaponStats.Value;
            }
        }
    }
}
