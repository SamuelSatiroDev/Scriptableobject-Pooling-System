using System.Collections.Generic;
using UnityEngine;

namespace ExtensionMethods
{
    [System.Serializable]
    public class FieldDataManagerSystem
    {
        public static FieldDataManager fieldManager = new FieldDataManager();
    }

    [System.Serializable]
    public class FieldDataManager
    {
        public List<FieldData> fields = new List<FieldData>();
    }

    [System.Serializable]
    public class FieldData
    {
        public FieldType fieldType;

        public string fieldName = string.Empty;
        public string singleValue = string.Empty;
        public List<string> singleValues = new List<string>();
        [SerializeReference]public List<FieldData> objectValues = new List<FieldData>();
    }

    public enum FieldType
    {
        Single,
        SingleValues,
        Object,
        ObjectsValues,
    }
}