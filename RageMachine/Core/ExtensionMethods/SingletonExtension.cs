using System.IO;
using UnityEngine;

namespace ExtensionMethods
{
    /// <summary>
    /// Should not be added to the scene!
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        private static object Lock = new object();

        public static T Instance
        {
            get
            {
                lock (Lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debug.LogError("[Singleton] Something went really wrong " +
                                " - there should never be more than 1 singleton!" +
                                " Reopening the scene might fix it.");
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            GameObject singletonPrefab = null;
                            GameObject singleton = null;

                            // Check if exists a singleton prefab on Resources Folder.
                            // -- Prefab must have the same name as the Singleton SubClass
                            singletonPrefab = (GameObject)Resources.Load(typeof(T).ToString(), typeof(GameObject));

                            // Create singleton as new or from prefab
                            if (singletonPrefab != null)
                            {
                                singleton = Instantiate(singletonPrefab);
                                _instance = singleton.GetComponent<T>();
                            }
                            else
                            {
                                singleton = new GameObject();
                                _instance = singleton.AddComponent<T>();
                            }

                            singleton.name = $"(singleton) {typeof(T).Name}";

                            DontDestroyOnLoad(singleton);

                            // Debug.Log("[Singleton] An instance of " + typeof(T) + 
                            // 	" is needed in the scene, so '" + singleton +
                            // 	"' was created with DontDestroyOnLoad.");
                        }
                        else
                        {
                            DebugExtension.Log("Using instance already created: " +
                                _instance.gameObject.name);
                        }
                    }

                    return _instance;
                }
            }
        }
    }

	public abstract class SingletonAwake<T> : MonoBehaviour where T : MonoBehaviour
    {
        // create a private reference to T instance
        private static T INSTANCE;

        public static T Instance
        {
            get
            {
                // if instance is null
                if (INSTANCE == null)
                {
                    // find the generic instance
                    INSTANCE = FindObjectOfType<T>();

                    // if it's null again create a new object
                    // and attach the generic instance
                    if (INSTANCE == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        INSTANCE = obj.AddComponent<T>();
                    }
                }
                return INSTANCE;
            }
        }

        public virtual void Awake()
        {
            // create the instance
            if (INSTANCE == null)
            {
                INSTANCE = this as T;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                _instance = Resources.Load<T>(typeof(T).Name.ToString());

                if (_instance == null)
                {
                    CreateScriptableObject();

                    _instance = Resources.Load<T>(typeof(T).Name.ToString());

                    (_instance as ScriptableSingleton<T>).OnInitialize();
                }

                void CreateScriptableObject()
                {
#if UNITY_EDITOR
                    string directory = $"{Application.dataPath}/Resources";

                    if (Directory.Exists(directory) == false)
                    {
                        Directory.CreateDirectory(directory);
                        UnityEditor.AssetDatabase.Refresh();
                    }

                    Editor.ScriptableObjectExtension.CreateAsset(typeof(T).Name.ToString(), "Resources", typeof(T).Name.ToString());
                    UnityEditor.AssetDatabase.Refresh();
#endif
                }

                return _instance;
            }
        }

        protected virtual void OnInitialize() { }
    }
}