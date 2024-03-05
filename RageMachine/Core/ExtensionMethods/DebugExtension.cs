using Common.Enum;
using UnityEngine;

namespace ExtensionMethods
{
    public static class DebugExtension
    {
        public static void Log(string title, EnumTones titleColor, params string [] values)
        {
            Debug.Log(DebugValue("[log]", title, EnumTones.WhiteSmoke_F5F5F5, titleColor, values));
        }

        public static void Log(params string[] values)
        {
            Debug.Log(DebugValue("[log]", string.Empty, EnumTones.WhiteSmoke_F5F5F5, EnumTones.WhiteSmoke_F5F5F5, values));
        }

        public static void LogWarning(string title, EnumTones titleColor, params string[] values)
        {
            Debug.LogWarning(DebugValue("[warning]", title, EnumTones.YellowGreen_9ACD32, titleColor, values));
        }

        public static void LogWarning(params string[] values)
        {
            Debug.LogWarning(DebugValue("[warning]", string.Empty, EnumTones.YellowGreen_9ACD32, EnumTones.WhiteSmoke_F5F5F5, values));
        }

        public static void LogError(string title, EnumTones titleColor, params string[] values)
        {
            Debug.LogError(DebugValue("[error]", title, EnumTones.OrangeRed_FF4500, titleColor, values));
        }

        public static void LogError(params string[] values)
        {
            Debug.LogError(DebugValue("[error]", string.Empty, EnumTones.OrangeRed_FF4500, EnumTones.WhiteSmoke_F5F5F5, values));
        }

        private static string DebugValue(string debugTitle, string title, EnumTones debugTitleColor, EnumTones titleColor, params string[] values)
        {
            string debug = string.Empty;
            string value = string.Empty;

            foreach (string item in values)
            {
                value += $"{item}";
            }

            debug = $"<b><color={TryParseHtml(debugTitleColor)}>{debugTitle}</color></b> <color={TryParseHtml(titleColor)}>{title}</color> {value}";
            return debug;
        }

        private static string TryParseHtml(EnumTones tones)
        {
            string[] splitColor = tones.ToString().Split('_');
            return string.Format("{0}{1}", "#", splitColor[1]);
        }
    }
}