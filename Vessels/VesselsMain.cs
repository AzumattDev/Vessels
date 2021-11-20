﻿using System;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace Vessels
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class Vessels : BaseUnityPlugin
    {
        private const string ModName = "Vessels";
        private const string ModVersion = "1.0";
        private const string ModGUID = "azumatt.Vessels";
        private readonly Harmony _harmony = new(ModGUID);
        private static AssetBundle? assetBundle;
        #region MyRegion

        /* Game Objects */
        private static GameObject Gallion;

        #endregion
        
        
        public void Awake()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            assetBundle = Utilities.Utils.LoadAssetBundle("vessels");
            _harmony.PatchAll(assembly);
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }
        
        [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
        public static class VesselsZNetScene
        {
            public static void Prefix(ZNetScene __instance)
            {
                if (__instance.m_prefabs.Count <= 0) return;
                Utilities.Utils.LoadAssets(assetBundle, __instance);
            }
        }
    }
}