using UnityEngine;

namespace PoolingSystem.Demo
{
    public class PoolingDemo : MonoBehaviour
    {
        [SerializeField] private PoolingItem _poolingItem;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject pool = PoolingManager.HandlerOnGet($"{_poolingItem.Prefab.name}", _poolingItem.Prefab, _poolingItem.Content, _poolingItem.DefaultCapacity, _poolingItem.MaxSize);

                if (pool != null)
                {
                    pool.SetActive(true);
                }
            }
        }
    }
}