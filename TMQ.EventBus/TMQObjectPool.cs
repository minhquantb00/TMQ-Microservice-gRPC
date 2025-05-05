using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.EventBus
{
    public class TMQObjectPool<T>(Func<Task<T>> objectGenerator)
    {
        private readonly ConcurrentBag<T> _objects = [];

        private readonly Func<Task<T>> _objectGenerator =
            objectGenerator ?? throw new ArgumentNullException(nameof(objectGenerator));

        public async Task<T?> Get()
        {
            if (_objects.TryTake(out var item) && item != null)
            {
                return item;
            }

            return await _objectGenerator();
        }

        public void Return(T? item)
        {
            if (item != null)
            {
                _objects.Add(item);
            }
        }
    }
}
