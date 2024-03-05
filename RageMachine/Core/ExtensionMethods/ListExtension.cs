using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExtensionMethods
{
    public static class ListExtension
    {
        [Serializable]
        public class SearchObject<T>
        {
            public string itemName;
            public T itemValue;

            public SearchObject(string itemName, T itemValue)
            {
                this.itemName = itemName;
                this.itemValue = itemValue;
            }
        }

        public static IEnumerable<GameObject> GetAllRootGameObjects()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                GameObject[] rootObjs = SceneManager.GetSceneAt(i).GetRootGameObjects();
                foreach (GameObject obj in rootObjs)
                {
                    yield return obj;
                }  
            }
        }

        public static List<T> FindAllObjectsOfTypeExpensive<T>() 
        {
            List<T> objList = new List<T>();

            foreach (GameObject obj in GetAllRootGameObjects())
            {
                foreach (T child in obj.GetComponentsInChildren<T>(true))
                {
                    objList.Add(child);
                } 
            }

            return objList;
        }

        #region Search
        public static List<T> SearchObjects<T>(this List<T> dataBase, string searchInput, int resultLimit = int.MaxValue, bool searchCaseSensitive = false)
        {
            List<T> list = dataBase;

            if (string.IsNullOrEmpty(searchInput))
            {
                return list;
            }

            List<SearchObject<T>> searchObject = new List<SearchObject<T>>();

            UnityEngine.Object result = null;

            foreach (T item in list)
            {
                result = item as UnityEngine.Object;

                if (item != null)
                {
                    searchObject.Add(new SearchObject<T>(result.name, item));
                }
            }

            list.Search(searchObject, searchInput, resultLimit, searchCaseSensitive);

            return list;
        }

        public static void Search<T>(this List<T> itemsResult, List<SearchObject<T>> dataBase, string inputText, int resultLimit = int.MaxValue, bool searchCaseSensitive = false)
        {
            int searchTextLength = inputText.Length;
            int searchElements = 0;
            itemsResult.Clear();

            if (inputText == string.Empty)
            {
                return;
            }

            foreach (SearchObject<T> item in dataBase)
            {
                searchElements += 1;

                if (item.itemName.Length >= searchTextLength)
                {
                    if (searchCaseSensitive == false && inputText.ToLower() == item.itemName.Substring(0, searchTextLength).ToLower() ||
                        searchCaseSensitive == true && inputText == item.itemName.Substring(0, searchTextLength))
                    {
                        if (inputText.ToLower() == item.itemName.Substring(0, searchTextLength).ToLower())
                        {
                            if (itemsResult.Count < resultLimit)
                            {
                                itemsResult.Add(item.itemValue);
                            }
                        }
                        else
                        {
                            itemsResult.Remove(item.itemValue);
                        }
                    }
                }
            }
        }

        public static void Search(this List<string> wordsResult, List<string> wordsDataBase, string inputText, int resultLimit = int.MaxValue, bool searchCaseSensitive = false)
        {
            int searchTextLength = inputText.Length;
            int searchElements = 0;
            wordsResult.Clear();

            if(inputText == string.Empty)
            {
                return;
            }

            foreach (string item in wordsDataBase)
            {
                searchElements += 1;

                if (item.Length >= searchTextLength)
                {
                    if (searchCaseSensitive == false && inputText.ToLower() == item.Substring(0, searchTextLength).ToLower() ||
                        searchCaseSensitive == true && inputText == item.Substring(0, searchTextLength))
                    {
                        if (inputText.ToLower() == item.Substring(0, searchTextLength).ToLower())
                        {
                            if(wordsResult.Count < resultLimit)
                            {
                                wordsResult.Add(item);
                            }
                        }
                        else
                        {
                            wordsResult.Remove(item);
                        }
                    }
                }
            }
        }
        #endregion

        #region SortList
        /// <summary>
        /// Sort list by variable name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="reverse"></param>
        /// <param name="fieldPath"></param>
        /// <returns></returns>
        public static List<T> SortList<T>(this List<T> list, bool reverse = false, params string [] fieldPath)
        {
            list.Sort(SortByList);
            return list;

            int SortByList(T item1, T item2)
            {
                object newItem1 = fieldPath.Length > 0 ? item1.GetFieldValue(fieldPath) : item1;
                object newItem2 = fieldPath.Length > 0 ? item2.GetFieldValue(fieldPath) : item2;

                switch (Type.GetTypeCode(newItem1.GetType()))
                {
                    case TypeCode.Boolean:
                        bool bool1 = reverse ? (bool)newItem2 : (bool)newItem1;
                        bool bool2 = reverse ? (bool)newItem1 : (bool)newItem2;
                        return bool1.CompareTo(bool2);

                    case TypeCode.Byte:
                        byte byte1 = reverse ? (byte)newItem2 : (byte)newItem1;
                        byte byte2 = reverse ? (byte)newItem1 : (byte)newItem2;
                        return byte1.CompareTo(byte2);

                    case TypeCode.SByte:
                        sbyte sbyte1 = reverse ? (sbyte)newItem2 : (sbyte)newItem1;
                        sbyte sbyte2 = reverse ? (sbyte)newItem1 : (sbyte)newItem2;
                        return sbyte1.CompareTo(sbyte2);

                    case TypeCode.Char:
                        char char1 = reverse ? (char)newItem2 : (char)newItem1;
                        char char2 = reverse ? (char)newItem1 : (char)newItem2;
                        return char1.CompareTo(char2);

                    case TypeCode.DateTime:
                        DateTime DateTime1 = reverse ? (DateTime)newItem2 : (DateTime)newItem1;
                        DateTime DateTime2 = reverse ? (DateTime)newItem1 : (DateTime)newItem2;
                        return DateTime1.CompareTo(DateTime2);

                    case TypeCode.Decimal:
                        decimal decimal1 = reverse ? (decimal)newItem2 : (decimal)newItem1;
                        decimal decimal2 = reverse ? (decimal)newItem1 : (decimal)newItem2;
                        return decimal1.CompareTo(decimal2);

                    case TypeCode.Double:
                        double double1 = reverse ? (double)newItem2 : (double)newItem1;
                        double double2 = reverse ? (double)newItem1 : (double)newItem2;
                        return double1.CompareTo(double2);

                    case TypeCode.Int16:
                        Int16 Int161 = reverse ? (Int16)newItem2 : (Int16)newItem1;
                        Int16 Int162 = reverse ? (Int16)newItem1 : (Int16)newItem2;
                        return Int161.CompareTo(Int162);

                    case TypeCode.Int32:
                        Int32 Int321 = reverse ? (Int32)newItem2 : (Int32)newItem1;
                        Int32 Int322 = reverse ? (Int32)newItem1 : (Int32)newItem2;
                        return Int321.CompareTo(Int322);

                    case TypeCode.Int64:
                        Int64 Int641 = reverse ? (Int64)newItem2 : (Int64)newItem1;
                        Int64 Int642 = reverse ? (Int64)newItem1 : (Int64)newItem2;
                        return Int641.CompareTo(Int642);

                    case TypeCode.Object:
                        if (newItem1.GetType().Name == "List`1" || newItem1.GetType().Name.Contains("[]"))
                        {
                            IList list1 = (IList)newItem1;
                            IList list2 = (IList)newItem2;

                            int count1 = reverse ? list2.Count : list1.Count;
                            int count2 = reverse ? list1.Count : list2.Count;

                            return count1.CompareTo(count2);
                        }
                        else
                        {
                            UnityEngine.Object _object1 = (UnityEngine.Object)newItem1;
                            UnityEngine.Object _object2 = (UnityEngine.Object)newItem2;

                            string objectName1 = reverse ? _object2.name : _object1.name;
                            string objectName2 = reverse ? _object1.name : _object2.name;

                            return objectName1.CompareTo(objectName2);
                        }

                    case TypeCode.Single:
                        Single Single1 = reverse ? (Single)newItem2 : (Single)newItem1;
                        Single Single2 = reverse ? (Single)newItem1 : (Single)newItem2;
                        return Single1.CompareTo(Single2);

                    case TypeCode.String:
                        string string1 = reverse ? (string)newItem2 : (string)newItem1;
                        string string2 = reverse ? (string)newItem1 : (string)newItem2;
                        return string1.CompareTo(string2);

                    case TypeCode.UInt16:
                        UInt16 UInt161 = reverse ? (UInt16)newItem2 : (UInt16)newItem1;
                        UInt16 UInt162 = reverse ? (UInt16)newItem1 : (UInt16)newItem2;
                        return UInt161.CompareTo(UInt162);

                    case TypeCode.UInt32:
                        UInt32 UInt321 = reverse ? (UInt32)newItem2 : (UInt32)newItem1;
                        UInt32 UInt322 = reverse ? (UInt32)newItem1 : (UInt32)newItem2;
                        return UInt321.CompareTo(UInt322);

                    case TypeCode.UInt64:
                        UInt64 UInt641 = reverse ? (UInt64)newItem2 : (UInt64)newItem1;
                        UInt64 UInt642 = reverse ? (UInt64)newItem1 : (UInt64)newItem2;
                        return UInt641.CompareTo(UInt642);

                    default:
                        return default;
                }
            }
        }

        public static List<T> SortList<T>(this List<T> list, string fieldPath, bool reverse = false)
        {
            list.Sort(SortByList);
            return list;

            int SortByList(T item1, T item2)
            {
                object newItem1 = fieldPath.Length > 0 ? item1.GetFieldValue(fieldPath) : item1;
                object newItem2 = fieldPath.Length > 0 ? item2.GetFieldValue(fieldPath) : item2;

                switch (Type.GetTypeCode(newItem1.GetType()))
                {
                    case TypeCode.Boolean:
                        bool bool1 = reverse ? (bool)newItem2 : (bool)newItem1;
                        bool bool2 = reverse ? (bool)newItem1 : (bool)newItem2;
                        return bool1.CompareTo(bool2);

                    case TypeCode.Byte:
                        byte byte1 = reverse ? (byte)newItem2 : (byte)newItem1;
                        byte byte2 = reverse ? (byte)newItem1 : (byte)newItem2;
                        return byte1.CompareTo(byte2);

                    case TypeCode.SByte:
                        sbyte sbyte1 = reverse ? (sbyte)newItem2 : (sbyte)newItem1;
                        sbyte sbyte2 = reverse ? (sbyte)newItem1 : (sbyte)newItem2;
                        return sbyte1.CompareTo(sbyte2);

                    case TypeCode.Char:
                        char char1 = reverse ? (char)newItem2 : (char)newItem1;
                        char char2 = reverse ? (char)newItem1 : (char)newItem2;
                        return char1.CompareTo(char2);

                    case TypeCode.DateTime:
                        DateTime DateTime1 = reverse ? (DateTime)newItem2 : (DateTime)newItem1;
                        DateTime DateTime2 = reverse ? (DateTime)newItem1 : (DateTime)newItem2;
                        return DateTime1.CompareTo(DateTime2);

                    case TypeCode.Decimal:
                        decimal decimal1 = reverse ? (decimal)newItem2 : (decimal)newItem1;
                        decimal decimal2 = reverse ? (decimal)newItem1 : (decimal)newItem2;
                        return decimal1.CompareTo(decimal2);

                    case TypeCode.Double:
                        double double1 = reverse ? (double)newItem2 : (double)newItem1;
                        double double2 = reverse ? (double)newItem1 : (double)newItem2;
                        return double1.CompareTo(double2);

                    case TypeCode.Int16:
                        Int16 Int161 = reverse ? (Int16)newItem2 : (Int16)newItem1;
                        Int16 Int162 = reverse ? (Int16)newItem1 : (Int16)newItem2;
                        return Int161.CompareTo(Int162);

                    case TypeCode.Int32:
                        Int32 Int321 = reverse ? (Int32)newItem2 : (Int32)newItem1;
                        Int32 Int322 = reverse ? (Int32)newItem1 : (Int32)newItem2;
                        return Int321.CompareTo(Int322);

                    case TypeCode.Int64:
                        Int64 Int641 = reverse ? (Int64)newItem2 : (Int64)newItem1;
                        Int64 Int642 = reverse ? (Int64)newItem1 : (Int64)newItem2;
                        return Int641.CompareTo(Int642);

                    case TypeCode.Object:
                        if (newItem1.GetType().Name == "List`1" || newItem1.GetType().Name.Contains("[]"))
                        {
                            IList list1 = (IList)newItem1;
                            IList list2 = (IList)newItem2;

                            int count1 = reverse ? list2.Count : list1.Count;
                            int count2 = reverse ? list1.Count : list2.Count;

                            return count1.CompareTo(count2);
                        }
                        else
                        {
                            UnityEngine.Object _object1 = (UnityEngine.Object)newItem1;
                            UnityEngine.Object _object2 = (UnityEngine.Object)newItem2;

                            string objectName1 = reverse ? _object2.name : _object1.name;
                            string objectName2 = reverse ? _object1.name : _object2.name;

                            return objectName1.CompareTo(objectName2);
                        }

                    case TypeCode.Single:
                        Single Single1 = reverse ? (Single)newItem2 : (Single)newItem1;
                        Single Single2 = reverse ? (Single)newItem1 : (Single)newItem2;
                        return Single1.CompareTo(Single2);

                    case TypeCode.String:
                        string string1 = reverse ? (string)newItem2 : (string)newItem1;
                        string string2 = reverse ? (string)newItem1 : (string)newItem2;
                        return string1.CompareTo(string2);

                    case TypeCode.UInt16:
                        UInt16 UInt161 = reverse ? (UInt16)newItem2 : (UInt16)newItem1;
                        UInt16 UInt162 = reverse ? (UInt16)newItem1 : (UInt16)newItem2;
                        return UInt161.CompareTo(UInt162);

                    case TypeCode.UInt32:
                        UInt32 UInt321 = reverse ? (UInt32)newItem2 : (UInt32)newItem1;
                        UInt32 UInt322 = reverse ? (UInt32)newItem1 : (UInt32)newItem2;
                        return UInt321.CompareTo(UInt322);

                    case TypeCode.UInt64:
                        UInt64 UInt641 = reverse ? (UInt64)newItem2 : (UInt64)newItem1;
                        UInt64 UInt642 = reverse ? (UInt64)newItem1 : (UInt64)newItem2;
                        return UInt641.CompareTo(UInt642);

                    default:
                        return default;
                }
            }
        }
        #endregion

        #region Contains
        public static bool Contains<T>(this List<T> list, object value, string fieldName)
        {
            List<object> property = new List<object>();

            for (int i = 0; i < list.Count; i++)
            {
                property.Add(ItemVariableValue(list[i], fieldName));
            }

            return property.Contains(value);
        }

        private static object ItemVariableValue<T>(T item, string _propertyName)
        {
            object output = item.GetType().GetField(_propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null ? item : item.GetType().GetField(_propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            return output;
        }
        #endregion

        #region IsNullOrEmpty
        /// <summary>
        /// Returns true if the array is null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this T[] data)
        {
            return ((data == null) || (data.Length == 0));
        }

        /// <summary>
        /// Returns true if the list is null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this List<T> data)
        {
            return ((data == null) || (data.Count == 0));
        }

        /// <summary>
        /// Returns true if the dictionary is null or empty.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T1, T2>(this Dictionary<T1, T2> data)
        {
            return ((data == null) || (data.Count == 0));
        }
        #endregion

        #region RemoveDuplicates
        /// <summary>
        /// Removes items from a collection based on the given condition. This is useful if a query provides
        /// some duplicates you can't get rid of. Some Linq2Sql queries are an example of this.
        /// Use this method later to remove things you know are on the list multiple times.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <remarks>http://extensionmethod.net/csharp/icollection-t/removeduplicates</remarks>
        /// <returns></returns>
        public static IEnumerable<T> RemoveDuplicates<T>(this ICollection<T> list, Func<T, int> predicate)
        {
            Dictionary<int, T> dict = new Dictionary<int, T>();

            foreach (T item in list)
            {
                if (!dict.ContainsKey(predicate(item)))
                {
                    dict.Add(predicate(item), item);
                }
            }

            return dict.Values.AsEnumerable();
        }
        #endregion

        #region DequeueOrNull
        /// <summary>
        /// Deques an item or returns null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q"></param>
        /// <returns></returns>
        public static T DequeueOrNull<T>(this Queue<T> q)
        {
            try
            {
                return (q.Count > 0) ? q.Dequeue() : default(T);
            }

            catch (Exception)
            {
                return default(T);
            }
        }
        #endregion

        #region Resize
        public static void Resize<T>(this List<T> List, int NewCount)
        {
            if (NewCount <= 0)
            {
                List.Clear();
            }
            else
            {
                while (List.Count > NewCount) List.RemoveAt(List.Count - 1);
                while (List.Count < NewCount) List.Add(default(T));
            }
        }

        public static void Resize<T>(this List<T> list, int sz, T c = default(T))
        {
            int cur = list.Count;
            if (sz < cur)
            {
                list.RemoveRange(sz, cur - sz);
            }          
            else if (sz > cur)
            {
                list.AddRange(Enumerable.Repeat(c, sz - cur));
            }            
        }
        #endregion

        #region RemoveMissingObjects
        public static void RemoveMissingObjects<T>(this List<T> List)
        {
            for (var i = List.Count - 1; i > -1; i--)
            {
                if (List[i] == null)
                {
                    List.RemoveAt(i);
                }   
            }
        }
        #endregion

        #region GetListType
        public static Type GetListType(this Type source)
        {
            Type innerType = null;

            if (source.IsArray)
            {
                innerType = source.GetElementType();
            }
            else if (source.GetGenericArguments().Any())
            {
                innerType = source.GetGenericArguments()[0];
            }

            return innerType;
        }
        #endregion
    }
}