using Nova;
using Sapia.Game.Combat.Entities;
using UnityEngine;

namespace Assets._Scripts.Ui.Combat.Cards
{
    public class CardRender : MonoBehaviour
    {
        public TextBlock Name, Description;
        public UIBlock2D Art;
        
        public UsableAbility Ability { get; private set; }

        public void Render(UsableAbility ability)
        {
            Ability = ability;

            gameObject.name = $"Card_{ability.AbilityType.Id}";

            Name.Text = string.IsNullOrWhiteSpace(ability.AbilityType.Name) ? ability.AbilityType.Id : ability.AbilityType.Name;
            Description.Text = ability.AbilityType.Description;

            var image = Resources.Load<Texture2D>($"Abilities/{ability.AbilityType.Id}") ?? Resources.Load<Texture2D>("Abilities/Default");

            Art.SetImage(image);
        }
    }
}
