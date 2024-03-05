using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

namespace PoolingSystemStandard
{
    [System.Serializable]
    public sealed class PoolingItem
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _content = null;
        [SerializeField] private short _defaultCapacity = 0;
        [SerializeField] private int _maxSize = 0;

        private Queue<GameObject> _objectPool = new Queue<GameObject>();
        private List<GameObject> _generatedPool = new List<GameObject>();

        public event System.Action<GameObject> OnGet;
        public event System.Action<GameObject> OnRelease;

        public GameObject Prefab { get => _prefab; set => _prefab = value; }
        public Transform Content { get => _content; set => _content = value; }   
        public short DefaultCapacity { get => _defaultCapacity; set => _defaultCapacity = value; }
        public int MaxSize { get => _maxSize; set => _maxSize = value; }
        public Queue<GameObject> ObjectPool { get => _objectPool; set => _objectPool = value; }
        public List<GameObject> GeneratedPool { get => _generatedPool; set => _generatedPool = value; }

        /// <summary>
        /// Set item pooling information.
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="content"></param>
        /// <param name="spawnPosition"></param>
        /// <param name="defaultCapacity"></param>
        /// <param name="maxSize"></param>
        public void OnInitialize(GameObject prefab, Transform content, short defaultCapacity = 0, int maxSize = int.MaxValue)
        {
            _prefab = prefab;
            _content = content;    
            _defaultCapacity = defaultCapacity;
            _maxSize = maxSize <= 0 ? int.MaxValue : maxSize;

            for (int i = 0; i < defaultCapacity; i++)
            {
                GameObject pool = CreatePool();
                pool?.SetActive(false);
            }
        }

        /// <summary>
        /// Get a gameObject pool, if queue is empty a new one is generated according to the defined maximum limit.
        /// </summary>
        /// <returns></returns>
        public GameObject OnGetFromPool()
        {
            GameObject pool = Get();

            if (pool == null)
            {
                return null;
            }

            pool.gameObject.SetActive(true);

            OnGet?.Invoke(pool);

            return pool;
        }

        /// <summary>
        /// Deactivate the gameObject and add it back to the queue.
        /// </summary>
        /// <param name="pool"></param>
        public void OnReleaseToPool(GameObject pool)
        {
            if (pool == null || _objectPool.Contains(pool))
            {
                return;
            }

            _objectPool.Enqueue(pool);

            OnRelease?.Invoke(pool);
            pool.SetActive(false);
        }

        /// <summary>
        /// Deactivate all gameObjects and add them back to the queue.
        /// </summary>
        public void OnReleaseToAllPool()
        {
            foreach (GameObject pool in _generatedPool)
            {
                OnReleaseToPool(pool);
            }
        }

        /// <summary>
        /// Remove object from list generatedPool.
        /// </summary>
        /// <param name="pool"></param>
        public void OnDestroyPool(GameObject pool)
        {
            _objectPool.DequeueOrNull();
            _generatedPool.Remove(pool);
        }

        private GameObject CreatePool()
        {
            GameObject pool = _generatedPool.Count >= _maxSize ? null : MonoBehaviour.Instantiate(_prefab, _content);

            if (pool != null)
            {
                PoolRelease poolRelease = pool.AddComponent<PoolRelease>();
                poolRelease.PoolingItem = this;

                _generatedPool.Add(pool);
            }

            return pool;
        }

        private GameObject Get()
        {
            if (_objectPool.Count <= 0)
            {
                return CreatePool();
            }
            else
            {
                return _objectPool.Dequeue();
            }
        }
    }
}