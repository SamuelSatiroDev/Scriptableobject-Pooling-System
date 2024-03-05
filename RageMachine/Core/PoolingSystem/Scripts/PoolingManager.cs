using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

namespace PoolingSystem
{
    [CreateAssetMenu(fileName = "PoolingManagerStandard", menuName = "Rage Machine/Pooling/Pooling Manager")]
    public sealed class PoolingManager : ScriptableSingleton<PoolingManager>
    {
        private Dictionary<string, PoolingItem> _poolingItems = new Dictionary<string, PoolingItem>();

        public static Dictionary<string, PoolingItem> PoolingItems { get => Instance._poolingItems; set => Instance._poolingItems = value; }

        /// <summary>
        /// Set item pooling information.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="prefab"></param>
        /// <param name="content"></param>
        /// <param name="spawnPosition"></param>
        /// <param name="defaultCapacity"></param>
        /// <param name="maxSize"></param>
        public static void HandlerInitialize(string ID, GameObject prefab, Transform content = null, short defaultCapacity = 0, int maxSize = int.MaxValue)
        {
            if (PoolingItems.ContainsKey(ID) == true)
            {
                PoolingItems[ID].Content = content;
                return;
            }

            PoolingItem pooling = new PoolingItem();
            pooling.OnInitialize(prefab, content, defaultCapacity, maxSize);

            PoolingItems.Add(ID, pooling);
        }

        /// <summary>
        /// Get a gameObject pool, if queue is empty a new one is generated according to the defined maximum limit.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="prefab"></param>
        /// <param name="content"></param>
        /// <param name="spawnPosition"></param>
        /// <param name="defaultCapacity"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        public static GameObject HandlerOnGet(string ID, GameObject prefab, Transform content, short defaultCapacity = 0, int maxSize = int.MaxValue)
        {
            HandlerInitialize(ID, prefab, content, defaultCapacity, maxSize);

            GameObject pool = PoolingItems[ID].OnGetFromPool();

            return pool;
        }

        /// <summary>
        /// Deactivate the gameObject and add it back to the queue.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="item"></param>
        public static void HandlerRelease(string ID, GameObject pool)
        {
            if (PoolingItems.ContainsKey(ID) == false)
            {
                return;
            }

            PoolingItems[ID].OnReleaseToPool(pool);
        }

        /// <summary>
        /// Deactivate all gameObjects and add them back to the queue.
        /// </summary>
        /// <param name="ID"></param>
        public static void HandlerReleaseAll(string ID)
        {
            if (PoolingItems.ContainsKey(ID) == false)
            {
                return;
            }

            PoolingItems[ID].OnReleaseToAllPool();
        }

        public static void AddListenerGet(string ID, GameObject prefab, Transform content, System.Action<GameObject> action)
        {
            if (Instance == null)
            {
                return;
            }

            HandlerInitialize(ID, prefab, content);
            PoolingItems[ID].OnGet += action;
        }

        public static void RemoveListenerGet(string ID, GameObject prefab, Transform content, System.Action<GameObject> action)
        {
            if (Instance == null)
            {
                return;
            }

            HandlerInitialize(ID, prefab, content);
            PoolingItems[ID].OnGet -= action;
        }

        public static void AddListenerRelease(string ID, GameObject prefab, Transform content, System.Action<GameObject> action)
        {
            if (Instance == null)
            {
                return;
            }

            HandlerInitialize(ID, prefab, content);
            PoolingItems[ID].OnRelease += action;
        }

        public static void RemoveListenerRelease(string ID, GameObject prefab, Transform content, System.Action<GameObject> action)
        {
            if (Instance == null)
            {
                return;
            }

            HandlerInitialize(ID, prefab, content);
            PoolingItems[ID].OnRelease -= action;
        }
    }
}