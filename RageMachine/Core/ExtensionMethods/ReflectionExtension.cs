using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System;
using Common.Enum;
using UnityEditor;

namespace ExtensionMethods
{
    public static class ReflectionExtension
    {
        public static Dictionary<Type, EnumVariablesType> Types = new Dictionary<Type, EnumVariablesType>
        {
        {typeof(string), EnumVariablesType.String},
        {typeof(char), EnumVariablesType.Char},
        {typeof(byte), EnumVariablesType.Byte},
        {typeof(sbyte), EnumVariablesType.Sbyte},
        {typeof(short), EnumVariablesType.Short},
        {typeof(ushort), EnumVariablesType.Ushort},
        {typeof(int), EnumVariablesType.Int},
        {typeof(uint), EnumVariablesType.Uint},
        {typeof(long), EnumVariablesType.Long},
        {typeof(ulong), EnumVariablesType.Ulong},
        {typeof(float), EnumVariablesType.Float},
        {typeof(double), EnumVariablesType.Double},
        {typeof(decimal), EnumVariablesType.Decimal},
        {typeof(bool), EnumVariablesType.Bool},
        {typeof(Vector2), EnumVariablesType.Vector2},
        {typeof(Vector3), EnumVariablesType.Vector3},
        {typeof(Vector4), EnumVariablesType.Vector4},
        {typeof(Quaternion), EnumVariablesType.Quaternion},
        {typeof(Color), EnumVariablesType.Color},
        {typeof(Color32), EnumVariablesType.Color32},
        };

        public static Dictionary<Type, EnumGenericVariablesType> GenericTypes = new Dictionary<Type, EnumGenericVariablesType>
        {
        {typeof(GameObject), EnumGenericVariablesType.GameObject},
        };

        public static UnityEngine.Object CreateInstance(this Type type)
        {
            UnityEngine.Object newObj = ScriptableObject.CreateInstance(type);
            return newObj;
        }

        public static Type GetInnerListType(this Type source)
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

        public static object GetFieldValue<T>(this T obj, params string[] fieldPath)
        {
            Queue<string> fieldPathStack = new Queue<string>(fieldPath);
            object outputValue = CheckFields(obj);
            string fieldName;
            object newValue;
            FieldInfo fieldInfo;

            object CheckFields(object newObj)
            {
                fieldName = fieldPathStack.Dequeue();
                fieldInfo = newObj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                newValue = null;

                if (Types.ContainsKey(fieldInfo.FieldType) == false)
                {
                    if(fieldPathStack.Count <= 0)
                    {
                        if (fieldInfo.FieldType.Name == "List`1" || fieldInfo.FieldType.Name.Contains("[]"))
                        {
                            newValue = (IList)fieldInfo.GetValue(newObj);
                        }
                        else
                        {
                            newValue = fieldInfo.GetValue(newObj);
                        } 
                    }
                    else
                    {
                        newValue = CheckFields(fieldInfo.GetValue(newObj));
                    }
                }
                else
                {
                    newValue = fieldInfo.GetValue(newObj);
                }

                return newValue;
            }

            return outputValue;
        }

        #region SetAllFields
        public static T SetAllFields<T>(this T obj, List<FieldData> values) where T : class
        {
            FieldInfo fieldInfo = null;
            Type listType;
            IList list;
            object listItemInstance = null;
            T newObj = obj;

            for (int i = 0; i < newObj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length; i++)
            {
                fieldInfo = newObj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)[i];

                foreach (FieldData value in values)
                {
                    if (fieldInfo.Name == value.fieldName)
                    {
                        if (Types.ContainsKey(fieldInfo.FieldType) == false && fieldInfo.FieldType.IsEnum == false)
                        {
                            if (fieldInfo.FieldType.Name == "List`1" || fieldInfo.FieldType.Name.Contains("[]"))
                            {
                                list = (IList)fieldInfo.GetValue(obj);
                                listType = GetInnerListType(list.GetType());

                                if (Types.ContainsKey(listType) == false && listType.IsEnum == false)
                                {                               
                                    list.Clear();
                                  
                                    foreach (var item in value.objectValues)
                                    {
                                        listItemInstance = Activator.CreateInstance(listType);
                                        list.Add(listItemInstance);
                                    }

                                    for (int x = 0; x < value.objectValues.Count; x++)
                                    {
                                        list[x] = list[x].SetAllFields(value.objectValues[x].objectValues);
                                    }
                                }
                                else
                                {
                                    fieldInfo = SetFieldListValue(fieldInfo, obj, value.singleValues);
                                }
                            }
                            else
                            {
                                fieldInfo.GetValue(obj).SetAllFields(values[i].objectValues);
                            }
                        }
                        else
                        {
                            SetFieldValue(fieldInfo, obj, value.singleValue);
                        }
                    }
                }
            }

            return newObj;
        }

        private static void SetFieldValue(this object classType, string propertyName, object value)
        {
            classType.GetType().GetField(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(classType, value);
        }

        private static void SetFieldValue(FieldInfo field, object obj, string value)
        {
            if (Types.ContainsKey(field.FieldType))
            {
                switch (Types[field.FieldType])
                {
                    case EnumVariablesType.Vector2:
                        obj.SetFieldValue(field.Name, Convert.ChangeType(value.ToVector2(), field.FieldType));
                        break;

                    case EnumVariablesType.Vector3:
                        obj.SetFieldValue(field.Name, Convert.ChangeType(value.ToVector3(), field.FieldType));
                        break;

                    case EnumVariablesType.Vector4:
                        obj.SetFieldValue(field.Name, Convert.ChangeType(value.ToVector4(), field.FieldType));
                        break;

                    case EnumVariablesType.Quaternion:
                        obj.SetFieldValue(field.Name, Convert.ChangeType(value.ToQuaternion(), field.FieldType));
                        break;

                    case EnumVariablesType.Color:
                        Color color = value.ToColor(Color.white);
                        obj.SetFieldValue(field.Name, Convert.ChangeType(color, field.FieldType));
                        break;

                    case EnumVariablesType.Color32:
                        Color32 color32 = value.ToColor32(Color.white);
                        obj.SetFieldValue(field.Name, Convert.ChangeType(color32, field.FieldType));
                        break;

                    default:
                        obj?.SetFieldValue(field.Name, Convert.ChangeType(value, field.FieldType));
                        break;

                }
            }
            else
            {
                if (field.FieldType.IsEnum)
                {
                    obj?.SetFieldValue(field.Name, Convert.ToInt32(value));
                }
            }
        }

        public static FieldInfo SetFieldListValue(FieldInfo field, object obj, List<string> values)
        {
           FieldInfo fieldInfo = field;

           IList list = (IList)fieldInfo.GetValue(obj);
           Type listType = GetInnerListType(list.GetType());

            if (Types.ContainsKey(listType))
            {
                switch (Types[listType])
                {
                    case EnumVariablesType.String:
                        obj.SetFieldValue(fieldInfo.Name, values);
                        break;

                    case EnumVariablesType.Char:
                        List<char> charValues = values.ConvertAll(input => input.ToCharArray()[0]);
                        obj.SetFieldValue(fieldInfo.Name, charValues);
                        break;

                    case EnumVariablesType.Byte:
                        List<byte> byteValues = values.ConvertAll(input => byte.Parse(input));
                        obj.SetFieldValue(fieldInfo.Name, byteValues);
                        break;

                    case EnumVariablesType.Sbyte:
                        List<sbyte> sbyteValues = values.ConvertAll(input => sbyte.Parse(input));
                        obj.SetFieldValue(fieldInfo.Name, sbyteValues);
                        break;

                    case EnumVariablesType.Short:
                        List<short> shortValues = values.ConvertAll(input => short.Parse(input));
                        obj.SetFieldValue(fieldInfo.Name, shortValues);
                        break;

                    case EnumVariablesType.Ushort:
                        List<ushort> ushortValues = values.ConvertAll(input => ushort.Parse(input));
                        obj.SetFieldValue(fieldInfo.Name, ushortValues);
                        break;

                    case EnumVariablesType.Int:
                        List<int> intValues = values.ConvertAll(input => int.Parse(input));
                        obj.SetFieldValue(fieldInfo.Name, intValues);
                        break;

                    case EnumVariablesType.Uint:
                        List<uint> uintValues = values.ConvertAll(input => uint.Parse(input));
                        obj.SetFieldValue(fieldInfo.Name, uintValues);
                        break;

                    case EnumVariablesType.Long:
                        List<long> longValues = values.ConvertAll(input => long.Parse(input));
                        obj.SetFieldValue(fieldInfo.Name, longValues);
                        break;

                    case EnumVariablesType.Ulong:
                        List<ulong> ulongValues = values.ConvertAll(input => ulong.Parse(input));
                        obj.SetFieldValue(fieldInfo.Name, ulongValues);
                        break;

                    case EnumVariablesType.Float:
                        List<float> floatValues = values.ConvertAll(input => float.Parse(input));
                        obj.SetFieldValue(fieldInfo.Name, floatValues);
                        break;

                    case EnumVariablesType.Double:
                        List<double> doubleValues = values.ConvertAll(input => double.Parse(input));
                        obj.SetFieldValue(fieldInfo.Name, doubleValues);
                        break;

                    case EnumVariablesType.Decimal:
                        List<decimal> decimalValues = values.ConvertAll(input => decimal.Parse(input));
                        obj.SetFieldValue(fieldInfo.Name, decimalValues);
                        break;

                    case EnumVariablesType.Bool:
                        List<bool> boolValues = values.ConvertAll(input => bool.Parse(input));
                        obj.SetFieldValue(fieldInfo.Name, boolValues);
                        break;

                    case EnumVariablesType.Vector2:
                        List<Vector2> vector2Values = values.ConvertAll(input => input.ToVector2());
                        obj.SetFieldValue(fieldInfo.Name, vector2Values);
                        break;

                    case EnumVariablesType.Vector3:
                        List<Vector3> vector3Values = values.ConvertAll(input => input.ToVector3());
                        obj.SetFieldValue(fieldInfo.Name, vector3Values);
                        break;

                    case EnumVariablesType.Vector4:
                        List<Vector4> vector4Values = values.ConvertAll(input => input.ToVector4());
                        obj.SetFieldValue(fieldInfo.Name, vector4Values);
                        break;

                    case EnumVariablesType.Quaternion:
                        List<Quaternion> quaternionValues = values.ConvertAll(input => input.ToQuaternion());
                        obj.SetFieldValue(fieldInfo.Name, quaternionValues);
                        break;

                    case EnumVariablesType.Color:
                        List<Color> colorValues = values.ConvertAll(input => input.ToColor(Color.white));
                        obj.SetFieldValue(fieldInfo.Name, colorValues);
                        break;

                    case EnumVariablesType.Color32:
                        List<Color32> color32Values = values.ConvertAll(input => input.ToColor32(Color.white));
                        obj.SetFieldValue(fieldInfo.Name, color32Values);
                        break;
                }
            }
            else
            {
                if (listType.IsEnum)
                {
                    list.Clear();
                    foreach (string value in values)
                    {
                        list.Add(Convert.ToInt32(value));
                    }  
                }
            }

            return fieldInfo;
        }
        #endregion

        #region GetAllFields
        public static List<FieldData> GetAllFields<T>(this T obj) where T : class
        {
            List<FieldData> newFiedsData = new List<FieldData>();
            FieldData newFieldData = new FieldData();
            FieldData newFieldDataItemList = new FieldData();
            Type listType;
            IList fieldList;
            int index = 0;

            foreach (FieldInfo fieldInfo in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                newFieldData = new FieldData
                {
                    fieldName = fieldInfo.Name,
                    singleValue = GetFieldValue(fieldInfo.FieldType, fieldInfo.GetValue(obj)),
                    fieldType = FieldType.Single
                };

                if (Types.ContainsKey(fieldInfo.FieldType) == false && fieldInfo.FieldType.IsEnum == false)
                {
                    newFieldData.fieldType = FieldType.Object;
                    newFieldDataItemList.fieldType = FieldType.Object;
                    newFieldData.singleValue = string.Empty;

                    if (fieldInfo.FieldType.Name == "List`1" || fieldInfo.FieldType.Name.Contains("[]"))
                    {
                        fieldList = (IList)fieldInfo.GetValue(obj);
                        listType = GetInnerListType(fieldList.GetType());

                        if (Types.ContainsKey(listType) == false && listType.IsEnum == false)
                        {
                            newFieldData.objectValues = new List<FieldData>();
                            newFieldData.fieldType = FieldType.ObjectsValues;

                            for (int i = 0; i < fieldList.Count; i++)
                            {
                                index = i;

                                if(fieldList[i] != null)
                                {
                                    newFieldDataItemList = new FieldData();
                                    newFieldDataItemList.fieldType = FieldType.Object;
                                    newFieldDataItemList.fieldName = index.ToString();
                                    newFieldDataItemList.objectValues = fieldList[i].GetAllFields();
                                    newFieldData.objectValues.Add(newFieldDataItemList);
                                }
                            }

                            fieldList = null;
                        }
                        else
                        {
                            newFieldData.fieldType = FieldType.SingleValues;
                            foreach (var item in fieldList)
                            {
                                newFieldData.singleValues.Add(GetFieldValue(listType, item));
                            }
                        }
                    }
                    else
                    {
                        newFieldData.fieldType = FieldType.Object;
                        newFieldData.objectValues = new List<FieldData>(fieldInfo.GetValue(obj).GetAllFields());
                    }
                }

                newFiedsData.Add(newFieldData);
            }

            return newFiedsData;
        }

        private static string GetFieldValue(Type fieldType, object objValue)
        {
            string value = string.Empty;

            if (Types.ContainsKey(fieldType))
            {
                switch (Types[fieldType])
                {
                    case EnumVariablesType.Vector2:
                        Vector2 vector2 = (Vector2)objValue;
                        value = vector2.ToStringType();
                        break;

                    case EnumVariablesType.Vector3:
                        Vector3 vector3 = (Vector3)objValue;
                        value = vector3.ToStringType();
                        break;

                    case EnumVariablesType.Vector4:
                        Vector4 vector4 = (Vector4)objValue;
                        value = vector4.ToStringType();
                        break;

                    case EnumVariablesType.Quaternion:
                        Quaternion quaternion = (Quaternion)objValue;
                        value = quaternion.ToStringType();
                        break;

                    case EnumVariablesType.Color:
                        Color color = (Color)objValue;
                        value = color.ToStringType();
                        break;

                    case EnumVariablesType.Color32:
                        Color32 color32 = (Color32)objValue;
                        value = color32.ToStringType();
                        break;
                  
                    default:
                        value = objValue.ToString();
                        break;
                }
            }
            else
            {
                if (fieldType.IsEnum)
                {
                    Enum enumObject = (Enum)objValue;
                    value = Convert.ToInt32(enumObject).ToString();
                }
            }

            return value;
        }
        #endregion

        #region SubClass
        public static List<string> GetAllSubClassNames<T>(this T obj) where T : class
        {
            List<string> names = new List<string>();

            foreach (Type type in AppDomain.CurrentDomain.GetAllSubClass(obj.GetType()))
            {
                names.Add(type.Name);
            }

            return names;
        }

        public static Type[] GetAllSubClass<T>(this T classType) where T : class
        {
            Type[] types = Assembly.GetAssembly(classType.GetType()).GetTypes();
            Type[] possible = (from Type type in types where type.IsSubclassOf(classType.GetType()) select type).ToArray();

            return possible;
        }

        public static Type[] GetAllSubClass(this AppDomain aAppDomain, Type aType)
        {
            List<Type> result = new List<Type>();
            Assembly[] assemblies = aAppDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();

                foreach (Type type in types)
                {
                    if (type.IsSubclassOf(aType))
                    {
                        result.Add(type);
                    }
                }
            }

            return result.ToArray();
        }
        #endregion

        #region Interface
        public static IEnumerable<System.Type> GetTypesInherit<T>(this Assembly assembly)
        {

            var types = assembly.GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)))
                .ToList();

            return types;
        }

        public static Type[] GetTypesWithInterface(this AppDomain aAppDomain, Type aInterfaceType)
        {
            List<Type> result = new List<Type>();
            Assembly[] assemblies = aAppDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();

                foreach (Type type in types)
                {
                    if (aInterfaceType.IsAssignableFrom(type))
                    {
                        result.Add(type);
                    }
                }
            }
            return result.ToArray();
        }

        public static Type[] GetTypesWithInterface(Type interfaceType) => GetTypesWithInterface(AppDomain.CurrentDomain, interfaceType);
        #endregion  

        public static IEnumerable<Type> GetBaseTypes(this Type type, bool includeSelf = false)
        {
            IEnumerable<Type> enumerable = type.GetBaseClasses(includeSelf).Concat(type.GetInterfaces());
            if (includeSelf && type.IsInterface)
            {
                enumerable.Concat(new Type[1] { type });
            }

            return enumerable;
        }

        public static IEnumerable<Type> GetBaseClasses(this Type type, bool includeSelf = false)
        {
            if (!(type == null) && !(type.BaseType == null))
            {
                if (includeSelf)
                {
                    yield return type;
                }

                Type current = type.BaseType;
                while (current != null)
                {
                    yield return current;
                    current = current.BaseType;
                }
            }
        }
    }
}