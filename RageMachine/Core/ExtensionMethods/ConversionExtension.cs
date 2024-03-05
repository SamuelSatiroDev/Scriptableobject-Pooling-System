using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ExtensionMethods
{
    public static class ConversionExtension
    {
        public static string ToStringType(this Vector3 value) => value.ToString();
        public static string ToStringType(this Vector2 value) => value.ToString();
        public static string ToStringType(this Vector4 value) => value.ToString();
        public static string ToStringType(this Quaternion value) => value.ToString();
        public static string ToStringType(this Color value) =>$"#{ColorUtility.ToHtmlStringRGBA(value)}";
        public static string ToStringType(this Color32 value) => $"#{ColorUtility.ToHtmlStringRGBA(value)}";

        #region SpriteTo
        public static Texture2D ToTexture(this Sprite sprite)
        {
            try
            {
                if (sprite.rect.width != sprite.texture.width)
                {
                    Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                    Color[] colors = newText.GetPixels();
                    Color[] newColors = sprite.texture.GetPixels((int)System.Math.Ceiling(sprite.textureRect.x),
                                                                 (int)System.Math.Ceiling(sprite.textureRect.y),
                                                                 (int)System.Math.Ceiling(sprite.textureRect.width),
                                                                 (int)System.Math.Ceiling(sprite.textureRect.height));
                    Debug.Log(colors.Length + "_" + newColors.Length);
                    newText.SetPixels(newColors);
                    newText.Apply();
                    return newText;
                }
                else
                    return sprite.texture;
            }
            catch
            {
                return sprite.texture;
            }
        }
        #endregion

        #region IntTo
        public static float ToFloat(this int value) => (float)value;

        public static decimal ToDecimal(this int value) => (decimal)value;

        public static char ToChar(this int value) => Convert.ToChar(value);
        #endregion

        #region StringTo
        #region ToBase64
        public static byte[] ToBase64(this string data) => Convert.FromBase64String(data);
        #endregion

        #region ToColor
        public static Color NameColorToColor(this string color)
        {
            return (Color)typeof(Color).GetProperty(color.ToLowerInvariant()).GetValue(null, null);
        }

        public static Color ToColor(this string value, Color defaultValue)
        {
            return ColorUtility.TryParseHtmlString(value, out Color newColor) ? newColor : defaultValue;
        }

        public static Color32 ToColor32(this string value, Color32 defaultValue)
        {
            return ColorUtility.TryParseHtmlString(value, out Color newColor) ? newColor : defaultValue;
        }
        #endregion

        #region ToVector2
        public static Vector2 ToVector2(this string value)
        {
            if (value.StartsWith("(") && value.EndsWith(")"))
            {
                value = value.Substring(1, value.Length - 2);
            }

            string[] sArray = value.Split(',');
            Vector2 result = Vector2.zero;

            for (int i = 0; i < sArray.Length; i++)
            {
                sArray[i] = sArray[i].Replace(".", ",");
            } 

            if (sArray.Length > 0 && float.TryParse(sArray[0], out float resultX))
            {
                result.x = resultX;
            }

            if (sArray.Length > 1 && float.TryParse(sArray[1], out float resultY))
            {
                result.y = resultY;
            }

            return result;
        }

        /// <summary>
        ///  Obter valor de string para Vector2
        /// </summary>
        public static Vector2 ToVector2(this string value, Vector2 defaultValue)
        {
            if (value.StartsWith("(") && value.EndsWith(")"))
            {
                value = value.Substring(1, value.Length - 2);
            }

            string[] sArray = value.Split(',');
            Vector2 result = defaultValue;

            for (int i = 0; i < sArray.Length; i++)
            {
                sArray[i] = sArray[i].Replace(".", ",");
            }

            if (sArray.Length > 0 && float.TryParse(sArray[0], out float resultX))
            {
                result.x = resultX;
            }

            if (sArray.Length > 1 && float.TryParse(sArray[1], out float resultY))
            {
                result.y = resultY;
            }

            return result;
        }

        /// <summary>
        ///  Obter valor de string para Vector2
        /// </summary>
        public static Vector2 ToVector2(this string value, Vector2 defaultValue, string split)
        {
            if (value.StartsWith("(") && value.EndsWith(")"))
            {
                value = value.Substring(1, value.Length - 2);
            }

            string[] sArray = value.Split(split);
            Vector2 result = defaultValue;

            for (int i = 0; i < sArray.Length; i++)
            {
                sArray[i] = sArray[i].Replace(".", ",");
            }

            if (sArray.Length > 0 && float.TryParse(sArray[0], out float resultX))
            {
                result.x = resultX;
            }

            if (sArray.Length > 1 && float.TryParse(sArray[1], out float resultY))
            {
                result.y = resultY;
            }

            return result;
        }

        /// <summary>
        ///  Obter valor de string para Vector2
        /// </summary>
        public static Vector2 ToVector2(this string value, string split)
        {
            if (value.StartsWith("(") && value.EndsWith(")"))
            {
                value = value.Substring(1, value.Length - 2);
            }

            string[] sArray = value.Split(split);
            Vector2 result = new Vector2();

            for (int i = 0; i < sArray.Length; i++)
            {
                sArray[i] = sArray[i].Replace(".", ",");
            }

            if (sArray.Length > 0 && float.TryParse(sArray[0], out float resultX))
            {
                result.x = resultX;
            }

            if (sArray.Length > 1 && float.TryParse(sArray[1], out float resultY))
            {
                result.y = resultY;
            }

            return result;
        }
        #endregion

        #region ToVector3
        public static Vector3 ToVector3(this string value)
        {
            if (value.StartsWith("(") && value.EndsWith(")"))
            {
                value = value.Substring(1, value.Length - 2);
            }

            string[] sArray = value.Split(',');
            Vector3 result = new Vector3();

            for (int i = 0; i < sArray.Length; i++)
            {
                sArray[i] = sArray[i].Replace(".", ",");
            }

            if (sArray.Length > 0 && float.TryParse(sArray[0].Replace(".", ","), out float resultX))
            {
                result.x = resultX;
            }

            if (sArray.Length > 1 && float.TryParse(sArray[1].Replace(".", ","), out float resultY))
            {
                result.y = resultY;
            }

            if (sArray.Length > 2 && float.TryParse(sArray[2].Replace(".", ","), out float resultZ))
            {
                result.z = resultZ;
            }

            return result;
        }

        /// <summary>
        ///  Obter valor de string para Vector3
        /// </summary>
        public static Vector3 ToVector3(this string value, Vector3 defaultValue)
        {
            if (value.StartsWith("(") && value.EndsWith(")"))
            {
                value = value.Substring(1, value.Length - 2);
            }

            string[] sArray = value.Split(',');
            Vector3 result = defaultValue;

            for (int i = 0; i < sArray.Length; i++)
            {
                sArray[i] = sArray[i].Replace(".", ",");
            }

            if (sArray.Length > 0 && float.TryParse(sArray[0].Replace(".", ","), out float resultX))
            {
                result.x = resultX;
            }

            if (sArray.Length > 1 && float.TryParse(sArray[1].Replace(".", ","), out float resultY))
            {
                result.y = resultY;
            }

            if (sArray.Length > 2 && float.TryParse(sArray[2].Replace(".", ","), out float resultZ))
            {
                result.z = resultZ;
            }

            return result;
        }

        public static Vector3 ToVector3(this string value, Vector3 defaultValue, string split)
        {
            if (value.StartsWith("(") && value.EndsWith(")"))
            {
                value = value.Substring(1, value.Length - 2);
            }

            string[] sArray = value.Split(split);
            Vector3 result = defaultValue;

            for (int i = 0; i < sArray.Length; i++)
            {
                sArray[i] = sArray[i].Replace(".", ",");
            }

            if (sArray.Length > 0 && float.TryParse(sArray[0].Replace(".", ","), out float resultX))
            {
                result.x = resultX;
            }

            if (sArray.Length > 1 && float.TryParse(sArray[1].Replace(".", ","), out float resultY))
            {
                result.y = resultY;
            }

            if (sArray.Length > 2 && float.TryParse(sArray[2].Replace(".", ","), out float resultZ))
            {
                result.z = resultZ;
            }

            return result;
        }

        public static Vector3 ToVector3(this string value, string split)
        {
            if (value.StartsWith("(") && value.EndsWith(")"))
            {
                value = value.Substring(1, value.Length - 2);
            }

            string[] sArray = value.Split(split);
            Vector3 result = new Vector3();

            for (int i = 0; i < sArray.Length; i++)
            {
                sArray[i] = sArray[i].Replace(".", ",");
            }

            if (sArray.Length > 0 && float.TryParse(sArray[0].Replace(".", ","), out float resultX))
            {
                result.x = resultX;
            }

            if (sArray.Length > 1 && float.TryParse(sArray[1].Replace(".", ","), out float resultY))
            {
                result.y = resultY;
            }

            if (sArray.Length > 2 && float.TryParse(sArray[2].Replace(".", ","), out float resultZ))
            {
                result.z = resultZ;
            }

            return result;
        }
        #endregion

        #region ToVector4
        public static Vector4 ToVector4(this string value)
        {
            if (value.StartsWith("(") && value.EndsWith(")"))
            {
                value = value.Substring(1, value.Length - 2);
            }

            string[] sArray = value.Split(',');
            Vector4 result = new Vector4();

            for (int i = 0; i < sArray.Length; i++)
            {
                sArray[i] = sArray[i].Replace(".", ",");
            }

            if (sArray.Length > 0 && float.TryParse(sArray[0], out float resultX))
            {
                result.x = resultX;
            }

            if (sArray.Length > 1 && float.TryParse(sArray[1], out float resultY))
            {
                result.y = resultY;
            }

            if (sArray.Length > 2 && float.TryParse(sArray[2], out float resultZ))
            {
                result.z = resultZ;
            }

            if (sArray.Length > 3 && float.TryParse(sArray[3], out float resultW))
            {
                result.w = resultW;
            }

            return result;
        }

        /// <summary>
        ///  Obter valor de string para Vector3
        /// </summary>
        public static Vector4 ToVector4(this string value, Vector4 defaultValue)
        {
            if (value.StartsWith("(") && value.EndsWith(")"))
            {
                value = value.Substring(1, value.Length - 2);
            }

            string[] sArray = value.Split(',');
            Vector4 result = defaultValue;

            for (int i = 0; i < sArray.Length; i++)
            {
                sArray[i] = sArray[i].Replace(".", ",");
            }

            if (sArray.Length > 0 && float.TryParse(sArray[0], out float resultX))
            {
                result.x = resultX;
            }

            if (sArray.Length > 1 && float.TryParse(sArray[1], out float resultY))
            {
                result.y = resultY;
            }

            if (sArray.Length > 2 && float.TryParse(sArray[2], out float resultZ))
            {
                result.z = resultZ;
            }

            if (sArray.Length > 3 && float.TryParse(sArray[3], out float resultW))
            {
                result.w = resultW;
            }

            return result;
        }

        public static Vector4 ToVector4(this string value, Vector4 defaultValue, string split)
        {
            if (value.StartsWith("(") && value.EndsWith(")"))
            {
                value = value.Substring(1, value.Length - 2);
            }

            string[] sArray = value.Split(split);
            Vector4 result = defaultValue;

            for (int i = 0; i < sArray.Length; i++)
            {
                sArray[i] = sArray[i].Replace(".", ",");
            }

            if (sArray.Length > 0 && float.TryParse(sArray[0], out float resultX))
            {
                result.x = resultX;
            }

            if (sArray.Length > 1 && float.TryParse(sArray[1], out float resultY))
            {
                result.y = resultY;
            }

            if (sArray.Length > 2 && float.TryParse(sArray[2], out float resultZ))
            {
                result.z = resultZ;
            }

            if (sArray.Length > 3 && float.TryParse(sArray[3], out float resultW))
            {
                result.w = resultW;
            }

            return result;
        }

        public static Vector4 ToVector4(this string value, string split)
        {
            if (value.StartsWith("(") && value.EndsWith(")"))
            {
                value = value.Substring(1, value.Length - 2);
            }

            string[] sArray = value.Split(split);
            Vector4 result = new Vector4();

            for (int i = 0; i < sArray.Length; i++)
            {
                sArray[i] = sArray[i].Replace(".", ",");
            }

            if (sArray.Length > 0 && float.TryParse(sArray[0], out float resultX))
            {
                result.x = resultX;
            }

            if (sArray.Length > 1 && float.TryParse(sArray[1], out float resultY))
            {
                result.y = resultY;
            }

            if (sArray.Length > 2 && float.TryParse(sArray[2], out float resultZ))
            {
                result.z = resultZ;
            }

            if (sArray.Length > 3 && float.TryParse(sArray[3], out float resultW))
            {
                result.w = resultW;
            }

            return result;
        }
        #endregion

        #region ToQuaternion
        public static Quaternion ToQuaternion(this string value)
        {
            if (value.StartsWith("(") && value.EndsWith(")"))
            {
                value = value.Substring(1, value.Length - 2);
            }

            string[] sArray = value.Split(',');
            Quaternion result = new Quaternion();

            for (int i = 0; i < sArray.Length; i++)
            {
                sArray[i] = sArray[i].Replace(".", ",");
            }

            if (sArray.Length > 0 && float.TryParse(sArray[0], out float resultX))
            {
                result.x = resultX;
            }

            if (sArray.Length > 1 && float.TryParse(sArray[1], out float resultY))
            {
                result.y = resultY;
            }

            if (sArray.Length > 2 && float.TryParse(sArray[2], out float resultZ))
            {
                result.z = resultZ;
            }

            if (sArray.Length > 3 && float.TryParse(sArray[3], out float resultW))
            {
                result.w = resultW;
            }

            return result;
        }

        /// <summary>
        ///  Obter valor de string para Vector3
        /// </summary>
        public static Quaternion ToQuaternion(this string value, Quaternion defaultValue)
        {
            if (value.StartsWith("(") && value.EndsWith(")"))
            {
                value = value.Substring(1, value.Length - 2);
            }

            string[] sArray = value.Split(',');
            Quaternion result = defaultValue;

            for (int i = 0; i < sArray.Length; i++)
            {
                sArray[i] = sArray[i].Replace(".", ",");
            }

            if (sArray.Length > 0 && float.TryParse(sArray[0], out float resultX))
            {
                result.x = resultX;
            }

            if (sArray.Length > 1 && float.TryParse(sArray[1], out float resultY))
            {
                result.y = resultY;
            }

            if (sArray.Length > 2 && float.TryParse(sArray[2], out float resultZ))
            {
                result.z = resultZ;
            }

            if (sArray.Length > 3 && float.TryParse(sArray[3], out float resultW))
            {
                result.w = resultW;
            }

            return result;
        }

        public static Quaternion ToQuaternion(this string value, Quaternion defaultValue, string split)
        {
            if (value.StartsWith("(") && value.EndsWith(")"))
            {
                value = value.Substring(1, value.Length - 2);
            }

            string[] sArray = value.Split(split);
            Quaternion result = defaultValue;

            for (int i = 0; i < sArray.Length; i++)
            {
                sArray[i] = sArray[i].Replace(".", ",");
            }

            if (sArray.Length > 0 && float.TryParse(sArray[0], out float resultX))
            {
                result.x = resultX;
            }

            if (sArray.Length > 1 && float.TryParse(sArray[1], out float resultY))
            {
                result.y = resultY;
            }

            if (sArray.Length > 2 && float.TryParse(sArray[2], out float resultZ))
            {
                result.z = resultZ;
            }

            if (sArray.Length > 3 && float.TryParse(sArray[3], out float resultW))
            {
                result.w = resultW;
            }

            return result;
        }

        public static Quaternion ToQuaternion(this string value, string split)
        {
            if (value.StartsWith("(") && value.EndsWith(")"))
            {
                value = value.Substring(1, value.Length - 2);
            }

            string[] sArray = value.Split(split);
            Quaternion result = new Quaternion();

            for (int i = 0; i < sArray.Length; i++)
            {
                sArray[i] = sArray[i].Replace(".", ",");
            }

            if (sArray.Length > 0 && float.TryParse(sArray[0], out float resultX))
            {
                result.x = resultX;
            }

            if (sArray.Length > 1 && float.TryParse(sArray[1], out float resultY))
            {
                result.y = resultY;
            }

            if (sArray.Length > 2 && float.TryParse(sArray[2], out float resultZ))
            {
                result.z = resultZ;
            }

            if (sArray.Length > 3 && float.TryParse(sArray[3], out float resultW))
            {
                result.w = resultW;
            }

            return result;
        }
        #endregion

        #region ToInt
        public static int ToInt(this string value, int defaultValue)
        {
            // exit if null
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            // convert
            int rVal;
            return int.TryParse(value, out rVal) ? rVal : defaultValue;
        }
        #endregion

        #region ToIntNull
        public static int? ToIntNull(this string value)
        {
            // exit if null
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            // convert
            int rVal;
            return int.TryParse(value, out rVal) ? rVal : new int?();
        }
        #endregion

        #region ToLong
        public static long ToLong(this string value, long defaultValue)
        {
            // exit if null
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            // convert
            long rVal;
            return long.TryParse(value, out rVal) ? rVal : defaultValue;
        }
        #endregion

        #region ToLongNull
        public static long? ToLongNull(this string value)
        {
            // exit if null
            if (string.IsNullOrEmpty(value))
            {
                return null;
            } 

            // convert
            long rVal;
            return long.TryParse(value, out rVal) ? rVal : new long?();
        }
        #endregion

        #region ToDecimal
        public static decimal ToDecimal(this string value, decimal defaultValue)
        {
            // exit if null
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            } 

            // convert
            decimal rVal;
            return decimal.TryParse(value, out rVal) ? rVal : defaultValue;
        }
        #endregion

        #region ToDouble
        public static double ToDouble(this string value, double defaultValue)
        {
            // exit if null
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
             
            // convert
            double rVal;
            return double.TryParse(value, out rVal) ? rVal : defaultValue;
        }
        #endregion

        #region ToDecimalNull
        public static decimal? ToDecimalNull(this string value)
        {
            // exit if null
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            // convert
            decimal rVal;
            return decimal.TryParse(value, out rVal) ? rVal : new decimal?();
        }
        #endregion

        #region ToFloat
        public static float ToFloat(this string value, float defaultValue)
        {
            // exit if null
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            // convert
            float rVal;
            return float.TryParse(value, out rVal) ? rVal : defaultValue;
        }
        #endregion

        #region ToFloatNull
        public static float? ToFloatNull(this string value)
        {
            // exit if null
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }         

            // convert
            float rVal;
            return float.TryParse(value, out rVal) ? rVal : new float?();
        }
        #endregion

        #region ToBool
        public static bool ToBool(this string value, bool defaultValue)
        {
            // exit if null
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            // convert
            bool rVal;
            return bool.TryParse(value, out rVal) ? rVal : defaultValue;
        }
        #endregion

        #region ToBoolNull
        public static bool? ToBoolNull(this string value)
        {
            // exit if null
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
                
            // convert
            bool rVal;
            return bool.TryParse(value, out rVal) ? rVal : new bool?();
        }
        #endregion

        #region ToDateTime
        public static DateTime ToDateTime(this string value, DateTime defaultValue)
        {
            // exit if null
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            } 

            // convert
            DateTime rVal;
            return DateTime.TryParse(value, out rVal) ? rVal : defaultValue;
        }
        #endregion

        #region ToDateTimeNull
        public static DateTime? ToDateTimeNull(this string value)
        {
            // exit if null
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            // convert
            DateTime tempDate;
            return (DateTime.TryParse(value, out tempDate)) ? tempDate : new DateTime?();
        }
        #endregion

        #region ToGuid
        public static Guid? ToGuid(this string gString)
        {
            try
            {
                return new Guid(gString);
            }

            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region ToDoubleNullable
        public static double? ToDoubleNullable(this string value)
        {
            // exit if null
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }      

            // convert
            double rVal;
            return double.TryParse(value, out rVal) ? rVal : new double?();
        }
        #endregion

        #region ToDecimalNullable
        public static decimal? ToDecimalNullable(this string value)
        {
            // exit if null
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            // convert
            decimal rVal;
            return decimal.TryParse(value, out rVal) ? rVal : new decimal?();
        }
        #endregion

        #region ToIntNullable
        public static int? ToIntNullable(this string value)
        {
            // exit if null
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            // convert
            int rVal;
            return int.TryParse(value, out rVal) ? rVal : new int?();
        }
        #endregion
        #endregion

        #region ByteTo
        public static int ToInt(this byte value) => System.Convert.ToInt32(value);

        public static long ToLong(this byte value) => System.Convert.ToInt64(value);

        public static double ToDouble(this byte value) => System.Convert.ToDouble(value);
        #endregion

        #region BytesTo
        /// <summary>
        /// Convert a byte array to a base64 string.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToBase64(this byte[] data) => System.Convert.ToBase64String(data);

        public static Sprite ToSprite(this byte[] bytes)
        {
            Texture2D texture2D = bytes.ToTexture2D();
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
            return sprite;
        }

        public static Texture2D ToTexture2D(this byte[] bytes)
        {
            Texture2D texture2D = new Texture2D(2, 2);
            texture2D.LoadImage(bytes);

            return texture2D;
        }

        public static Texture ToTexture(this byte[] bytes)
        {
            Texture2D texture2D = new Texture2D(2, 2);
            texture2D.LoadImage(bytes);

            return texture2D;
        }
        #endregion

        #region GenericConversions
        public static byte[] BytesSerialize(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }

        public static object BytesDeserialize(this byte[] byteArray)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                memoryStream.Write(byteArray, 0, byteArray.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);
                object content = binaryFormatter.Deserialize(memoryStream);

                return content;
            }
        }

        #region To
        /// <summary>
        /// Convert to a new type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T To<T>(this IConvertible obj) => (T)Convert.ChangeType(obj, typeof(T));
        #endregion

        #region ToOrDefault
        /// <summary>
        /// Converts to the new type or returns the default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ToOrDefault<T>(this IConvertible obj)
        {
            try
            {
                return To<T>(obj);
            }
            catch (Exception exception)
            {
                //handle conversion exceptions by returning the default.
                if (exception is InvalidCastException || exception is ArgumentNullException || exception is FormatException)
                    return default(T);

                //everything else is re-released.
                throw;
            }
        }

        /// <summary>
        /// Returns true if the conversion was successful.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="newObj"></param>
        /// <returns></returns>
        public static bool ToOrDefault<T>(this IConvertible obj, out T newObj)
        {
            try
            {
                newObj = To<T>(obj);
                return true;
            }

            catch (Exception exception)
            {
                //handle conversion exceptions by returning the default.
                if (exception is InvalidCastException || exception is ArgumentNullException || exception is FormatException)
                {
                    newObj = default(T);
                    return false;
                }

                //everything else is re-released.
                throw;
            }
        }
        #endregion

        #region ToOrOther
        /// <summary>
        /// Convert to obj. If conversion fails, then convert to another type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static T ToOrOther<T>(this IConvertible obj, T other)
        {
            try
            {
                return To<T>(obj);
            }
            catch (Exception excep)
            {
                //handle conversion exceptions by returning the default.
                if (excep is InvalidCastException || excep is ArgumentNullException || excep is FormatException)
                    return other;

                //everything else is re-released.
                throw;
            }
        }

        /// <summary>
        /// Convert to obj. If you cannot convert, convert to another type. Returns true if the conversion was successful.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="newObj"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool ToOrOther<T>(this IConvertible obj, out T newObj, T other)
        {
            try
            {
                newObj = To<T>(obj);
                return true;
            }
            catch (Exception excep)
            {
                //handle conversion exceptions by returning the default.
                if (excep is InvalidCastException || excep is ArgumentNullException || excep is FormatException)
                {
                    newObj = other;
                    return false;
                }

                //everything else is re-released.
                throw;
            }
        }
        #endregion

        #region ToOrNull
        /// <summary>
        /// Converts to a new object or returns null if unable to convert.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ToOrNull<T>(this IConvertible obj) where T : class
        {
            try
            {
                return To<T>(obj);
            }
            catch (Exception excep)
            {
                //handle conversion exceptions by returning the default.
                if (excep is InvalidCastException || excep is ArgumentNullException || excep is FormatException)
                    return null;

                //everything else is re-released.
                throw;
            }
        }

        /// <summary>
        /// Converts to a new object or to null if unable to convert. Returns true if the conversion was successful.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="newObj"></param>
        /// <returns></returns>
        public static bool ToOrNull<T>(this IConvertible obj, out T newObj) where T : class
        {
            try
            {
                newObj = To<T>(obj);
                return true;
            }
            catch (Exception excep)
            {
                //handle conversion exceptions by returning the default.
                if (excep is InvalidCastException || excep is ArgumentNullException || excep is FormatException)
                {
                    newObj = null;
                    return false;
                }

                //everything else is relaunched
                throw;
            }
        }
        #endregion
        #endregion
    }
}