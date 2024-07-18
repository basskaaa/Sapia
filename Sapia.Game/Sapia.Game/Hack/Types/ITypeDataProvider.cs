using Sapia.Game.Entities.Interfaces;

namespace Sapia.Game.Hack.Types;

public interface ITypeDataProvider<T> where T : ITypeData
{
    T? Find(string id);
    T Get(string id);
    IReadOnlyCollection<T> Get(IEnumerable<string> ids);
    IReadOnlyCollection<T> GetAll();
}