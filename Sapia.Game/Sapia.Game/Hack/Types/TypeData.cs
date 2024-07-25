using System.Diagnostics;
using Sapia.Game.Entities.Interfaces;

namespace Sapia.Game.Hack.Types;

[DebuggerDisplay("{Id}")]
public class TypeData : ITypeData
{
    public string Id { get; set; }
    public string Name { get; set; }
}