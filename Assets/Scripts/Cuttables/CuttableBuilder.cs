using UnityEngine;

namespace Cuttables
{
    public class CuttableBuilder
    {
        public void ItemConfigure(GameObject CuttableItem, Vector3 position, Quaternion rotation,
            Transform parent)
        {
            CuttableItem.transform.SetParent(parent);
            CuttableItem.transform.position = position;
            CuttableItem.transform.rotation = rotation;
            CuttableItem.transform.localScale = Vector3.one;
        }
    }
}