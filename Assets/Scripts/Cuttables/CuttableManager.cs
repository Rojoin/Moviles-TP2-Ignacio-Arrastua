using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Pool;

namespace Cuttables
{
    public class CuttableManager : MonoBehaviour
    {
        [SerializeField] private List<CuttableSO> cuttableSO = new List<CuttableSO>();
        [SerializeField] private Transform parent;
        private Dictionary<string, ObjectPool<GameObject>> cuttablesByID = new();

        public CuttableFactory _cuttableFactory;
        public CuttableBuilder _cuttableBuilder = new();
        int cuttableSize = 2;

        private void Awake()
        {
            foreach (CuttableSO t in cuttableSO)
            {
                cuttablesByID.Add(t.name, new ObjectPool<GameObject>(() => Instantiate(t.asset.gameObject, transform),
                    item => { item.gameObject.SetActive(true); }, item => { item.gameObject.SetActive(false); },
                    item => { Destroy(item.gameObject); }, false, cuttableSize, 100));
            }

            _cuttableFactory = new RandomCuttableFactory(cuttableSO);
        }

        public CuttableItem AddNewItem(CuttableSO cuttableSo, Vector3 position, Quaternion rotation, float size)
        {
            var pool = cuttablesByID[cuttableSo.name];
            if (pool == null)
            {
                Debug.LogError("Pool not found");
                return null;
            }

            GameObject newItem = null;
            pool.Get(out newItem);
            CuttableItem cuttableItem = newItem.GetComponent<CuttableItem>();
            cuttableItem.OnDespawn.AddListener(OnDespawn);

            _cuttableBuilder.ItemConfigure(newItem, position, rotation, size, parent);
            return cuttableItem;
        }

        private void OnDespawn(GameObject CuttableItem)
        {
            CuttableItem item = CuttableItem.GetComponent<CuttableItem>();
            item.OnDespawn.RemoveListener(OnDespawn);
            ObjectPool<GameObject> pool = cuttablesByID[item.SO.name];
            pool.Release(CuttableItem);
        }
    }
}