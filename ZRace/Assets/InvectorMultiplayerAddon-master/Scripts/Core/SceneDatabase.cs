using Invector.vItemManager;
using System.Collections.Generic;
using UnityEngine;

namespace CBGames.Core {
    [CreateAssetMenu(menuName = "CB Games/Scene Database", fileName = "ScenesDatabase")]
    public class SceneDatabase : ScriptableObject
    {
        public List<DatabaseScene> storedScenesData;
    }
    [System.Serializable]
    public class ItemWrapper
    {
        public List<ItemReference> items = new List<ItemReference>();

        public ItemWrapper(List<ItemReference> inputItems)
        {
            this.items = inputItems;
        }
    }
}