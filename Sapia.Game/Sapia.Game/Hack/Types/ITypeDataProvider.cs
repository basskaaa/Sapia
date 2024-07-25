using Sapia.Game.Entities.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Sapia.Game.Hack.Types;

public interface ITypeDataProvider<T> where T : ITypeData
{
    bool TryFind(string id, [MaybeNullWhen(false)] out T value);
    T? Find(string id);
    T Get(string id);
    IReadOnlyCollection<T> Get(IEnumerable<string> ids);
    IReadOnlyCollection<T> GetAll();
}