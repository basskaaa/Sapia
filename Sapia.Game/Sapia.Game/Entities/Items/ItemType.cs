using Sapia.Game.Entities.Interfaces;

namespace Sapia.Game.Entities.Items
{
    public class ItemType : IHasDescription
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        
        public float Value { get; set; }

        public string[] Description { get; set; } = Array.Empty<string>();
        public string[] Tags { get; set; } = Array.Empty<string>();
    }
}
