using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Harmony;

using UnityEngine;

namespace TodayOrdeal
{
    public class Harmony_Patch
    {
        public const string ModName = "Lobotomy.inority.TodayOrdeal";

        private static Dictionary<string, int> _limiter = new Dictionary<string, int>();

        public Harmony_Patch()
        {
            Invoke(() =>
            {
                HarmonyInstance mod = HarmonyInstance.Create(ModName);
                Patch(mod);
            });
        }

        public static void OnStartStage(GameManager __instance)
        {
            Invoke(() =>
            {
                var field = typeof(OrdealManager).GetField("_ordealList", BindingFlags.NonPublic | BindingFlags.Instance);
                var ordealList = (List<OrdealBase>)field.GetValue(OrdealManager.instance);

                var names = ordealList.Select(x => GetLocalizedOrdealName(x)).ToArray();
                var message = $"Today's ordeal:\n{string.Join("\n", names)}\nGood luck.";
                Notice.instance.Send("AddSystemLog", message);
            });
        }

        public void Patch(HarmonyInstance mod)
        {
            var postfix = typeof(Harmony_Patch).GetMethod(nameof(OnStartStage));
            mod.Patch(typeof(GameManager).GetMethod(nameof(GameManager.StartGame)), null, new HarmonyMethod(postfix));
        }

        private static string GetLocalizedOrdealName(OrdealBase ordeal)
        {
            var t = GetOrdealName(ordeal);
            string id = $"ordeal_{t}_type";
            var type = LocalizeTextDataModel.instance.GetText(id);
            var color = ordeal.OrdealColor;

            // string id2 = $"ordeal_{t}_name";
            // var name = LocalizeTextDataModel.instance.GetText(id2);

            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{type}</color>";
        }

        private static string GetOrdealName(OrdealBase ordeal)
        {
            if (ordeal is BugOrdeal)
            {
                var field = typeof(BugOrdeal).GetField("_ordealName", BindingFlags.NonPublic | BindingFlags.Instance);
                return (string)field.GetValue(ordeal);
            }
            else if (ordeal is MachineOrdeal)
            {
                var field = typeof(MachineOrdeal).GetField("_ordealName", BindingFlags.NonPublic | BindingFlags.Instance);
                return (string)field.GetValue(ordeal);
            }
            else if (ordeal is CircusOrdeal)
            {
                var field = typeof(CircusOrdeal).GetField("_ordealName", BindingFlags.NonPublic | BindingFlags.Instance);
                return (string)field.GetValue(ordeal);
            }
            else if (ordeal is OutterGodOrdeal)
            {
                var field = typeof(OutterGodOrdeal).GetField("_ordealName", BindingFlags.NonPublic | BindingFlags.Instance);
                return (string)field.GetValue(ordeal);
            }
            else if (ordeal is ScavengerOrdeal)
            {
                var field = typeof(ScavengerOrdeal).GetField("_ordealName", BindingFlags.NonPublic | BindingFlags.Instance);
                return (string)field.GetValue(ordeal);
            }
            else if (ordeal is FixerOrdeal)
            {
                var field = typeof(FixerOrdeal).GetField("_ordealName", BindingFlags.NonPublic | BindingFlags.Instance);
                return (string)field.GetValue(ordeal);
            }
            throw new Exception("Not a known ordeal");
        }

        private static void Invoke(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}