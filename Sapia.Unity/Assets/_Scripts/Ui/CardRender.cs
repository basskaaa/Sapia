using Nova;
using Sapia.Game.Combat.Entities;
using UnityEngine;

namespace Assets._Scripts.Ui
{
    public class CardRender : MonoBehaviour
    {
        public TextBlock Name;

        public UsableAbility Ability { get; private set; }

        public void Render(UsableAbility ability)
        {
            Ability = ability;

            gameObject.name = $"Card_{ability.AbilityType.Id}";

            Name.Text = string.IsNullOrWhiteSpace(ability.AbilityType.Name) ? ability.AbilityType.Id : ability.AbilityType.Name;
        }
    }
}
