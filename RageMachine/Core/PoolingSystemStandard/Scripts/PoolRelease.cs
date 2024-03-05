using UnityEngine;

namespace PoolingSystemStandard
{
    public sealed class PoolRelease : MonoBehaviour
    {
        private PoolingItem _poolingItem;

        public PoolingItem PoolingItem { get => _poolingItem; set => _poolingItem = value; }

        private void OnDisable() => _poolingItem.OnReleaseToPool(gameObject);

        private void OnDestroy() => _poolingItem.OnDestroyPool(gameObject);
    }
}