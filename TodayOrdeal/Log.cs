using System.IO;

using UnityEngine;

namespace TodayOrdeal
{
    internal class Log
    {
        public static void Error(System.Exception e)
        {
            Error($"{e.GetType().Name}: {e.Message}");
            Error(e.StackTrace);
        }

        public static void Error(string message)
        {
            File.AppendAllText(Application.dataPath + "/BaseMods/Error.txt", message + "\n");
        }
    }
}