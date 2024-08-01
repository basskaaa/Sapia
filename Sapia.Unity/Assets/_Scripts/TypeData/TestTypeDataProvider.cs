using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Sapia.Game.Entities.Interfaces;
using Sapia.Game.Types;

namespace Assets._Scripts.TypeData
{
    public class TestTypeDataProvider<T> : ITypeDataProvider<T> where T : ITypeData
    {
        private readonly Dictionary<string, T> _data;

        public TestTypeDataProvider(params T[] data)
        {
            _data = data.ToDictionary(x => x.Id, x => x);
        }

        public TestTypeDataProvider(IEnumerable<T> data)
        {
            _data = data.ToDictionary(x => x.Id, x => x);
        }

        public bool TryFind(string id, [MaybeNullWhen(false)] out T value) => _data.TryGetValue(id, out value);

        public T Find(string id)
        {
            if (_data.TryGetValue(id, out var data))
            {
                return data;
            }

            return data;
        }

        public T Get(string id) => _data[id];

        public IReadOnlyCollection<T> Get(IEnumerable<string> ids)
        {
            return ids.Select(x => _data[x]).ToArray();
        }

        public IReadOnlyCollection<T> GetAll() => _data.Values;
    }
}