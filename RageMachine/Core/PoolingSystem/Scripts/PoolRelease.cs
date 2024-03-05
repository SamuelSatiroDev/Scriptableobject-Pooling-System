using UnityEngine;

namespace PoolingSystem
{
    public sealed class PoolRelease : MonoBehaviour
    {
        private PoolingItem _poolingItem;

        public PoolingItem PoolingItem { get => _poolingItem; set => _poolingItem = value; }

        private void OnDisable() => _poolingItem?.ObjectPool.Release(gameObject);

        private void OnDestroy() => _poolingItem?.ObjectPool.Dispose();
    }
}