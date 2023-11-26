using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    [CreateAssetMenu(menuName = "Create BladeSFX", fileName = "BladeSFX", order = 0)]
    public class BladeSFX : ScriptableObject
    {
        public string name;
        public int id;
        public Sprite image;
        public Material cutMaterial;
        public GameObject asset;
        public int price;
        public bool isAvalaible;
    }
}