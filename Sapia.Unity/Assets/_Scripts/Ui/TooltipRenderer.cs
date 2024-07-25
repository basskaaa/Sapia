using Nova;
using Sapia.Game.Combat.Entities;
using UnityEngine;

namespace Assets._Scripts.Ui
{
    public class TooltipRenderer : MonoBehaviour
    {
        public TextBlock Description;

        public void Render(UsableAbility ability)
        {
            Description.Text = ability.AbilityType.Description;
        }
    }
}