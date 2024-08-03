using System.Diagnostics.CodeAnalysis;
using Sapia.Game.Types.Interfaces;

namespace Sapia.Game.Types;

public interface ITypeDataProvider<T> where T : ITypeData
{
    bool TryFind(string id, [MaybeNullWhen(false)] out T value);
    T? Find(string id);
    T Get(string id);
    IReadOnlyCollection<T> Get(IEnumerable<string> ids);
    IReadOnlyCollection<T> GetAll();
}