using System.Diagnostics;
using Sapia.Game.Types.Interfaces;

namespace Sapia.Game.Types;

[DebuggerDisplay("{Id}")]
public class TypeData : ITypeData
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class TypeDataWithDescription : TypeData, IHasDescription
{
    public string[] Description { get; set; } = Array.Empty<string>();
}