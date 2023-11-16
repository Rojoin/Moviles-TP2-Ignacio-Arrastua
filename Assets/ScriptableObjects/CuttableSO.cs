using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Create Gem", menuName = "Gem", order = 0)]
    public class CuttableSO : ScriptableObject
    {
        public string name;
        public GameObject asset;
    }

    
}