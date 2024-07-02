using System;
using System.Collections.Generic;

namespace ODProjects.PoolModule.Extentions
{
    public class ObjectPool<T> : AbstractObjectPool
    {
        private readonly List<T> _currentStock;
        private readonly Func<T> _factoryMethod;
        private readonly Action<T> _turnOnCallback;
        private readonly Action<T> _turnOffCallback;
        private readonly bool _isDynamic;
        
        public ObjectPool(Func<T> factoryMethod, Action<T> turnOnCallback, Action<T> turnOffCallback, int initialStock = 0, bool isDynamic = true)
        {
            _factoryMethod = factoryMethod;
            _isDynamic = isDynamic;

            _turnOffCallback = turnOffCallback;
            _turnOnCallback = turnOnCallback;

            _currentStock = new List<T>();

            for (var i = 0; i < initialStock; i++)
            {
                var o = _factoryMethod();
                _turnOffCallback(o);
                _currentStock.Add(o);
            }
        }

        public T GetObject()
        {
            var result = default(T);
            if (_currentStock.Count > 0)
            {
                result = _currentStock[0];
                _currentStock.RemoveAt(0);
            }
            else if (_isDynamic)
            {
                result = _factoryMethod();
            }

            _turnOnCallback(result);
            return result;
        }

        public void ReturnObject(T o)
        {
            _turnOffCallback(o);
            _currentStock.Add(o);
        }
    }
}
