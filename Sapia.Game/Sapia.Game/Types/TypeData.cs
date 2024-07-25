using System.Diagnostics;
using Sapia.Game.Entities.Interfaces;

namespace Sapia.Game.Types;

[DebuggerDisplay("{Id}")]
public class TypeData : ITypeData
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class TypeDataWithDescription : TypeData
{
    public string Description { get; set; } = string.Empty;
}