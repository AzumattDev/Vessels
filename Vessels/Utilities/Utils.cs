using System.IO;
using System.Linq;
using UnityEngine;

namespace Vessels.Utilities
{
    public class Utils
    {
        internal static AssetBundle? LoadAssetBundle(string bundleName)
        {
            string? resource = typeof(Vessels).Assembly.GetManifestResourceNames().Single
                (s => s.EndsWith(bundleName));
            using Stream? stream = typeof(Vessels).Assembly.GetManifestResourceStream(resource);
            return AssetBundle.LoadFromStream(stream);
        }

        internal static void LoadAssets(AssetBundle? bundle, ZNetScene zNetScene)
        {
            Object[]? tmp = bundle?.LoadAllAssets();
            if (zNetScene.m_prefabs.Count <= 0) return;
            if (tmp == null) return;
            foreach (var o in tmp)
            {
                GameObject? obj = (GameObject)o;
                Vessels.Gallion = obj;
                zNetScene.m_prefabs.Add(obj);
                int hashcode = obj.GetHashCode();
                zNetScene.m_namedPrefabs.Add(hashcode, obj);
            }
        }

        internal static Recipe RecipeMaker(int ammount, ItemDrop item, CraftingStation craftingStation, 
            CraftingStation repair, int level, Piece.Requirement[] resources)
        {
            Recipe temp = ScriptableObject.CreateInstance<Recipe>();
            //temp = Recipe.CreateInstance<Recipe>();
            temp.m_amount = ammount;
            temp.m_enabled = true;
            temp.m_item = item;
            temp.m_craftingStation = craftingStation;
            temp.m_repairStation = repair;
            temp.m_minStationLevel = level;
            temp.m_resources = resources;


            return temp;
        }

        internal static CraftingStation Station(string name)
        {
            GameObject? tmp = ZNetScene.instance.GetPrefab(name);
            return tmp.GetComponent<CraftingStation>();
        }

        internal static void AddtoZnet(GameObject GO, ZNetScene scene)
        {
            int hash = GO.GetHashCode();
            scene.m_prefabs.Add(GO);
            scene.m_namedPrefabs.Add(hash, GO);
        }
        
        internal static ItemDrop ReturnItemDrop(GameObject gameObject)
        {
            ItemDrop? drop = gameObject.GetComponent<ItemDrop>();

            return drop;
        }
        
        internal static GameObject RetrieveGO(string name)
        {
            GameObject? fab = ObjectDB.instance.GetItemPrefab(name);
            return fab;
        }

        internal static void AddToConsume(MonsterAI monsterAI, string Name, ObjectDB objectDB)
        {
            GameObject? tmp=objectDB.GetItemPrefab(Name);
            ItemDrop? drop = tmp.GetComponent<ItemDrop>();
            monsterAI.m_consumeItems.Add(drop);
        }
    }
}