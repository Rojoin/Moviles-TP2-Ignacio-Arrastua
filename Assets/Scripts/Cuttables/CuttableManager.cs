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

        private CuttableFactory _cuttableFactory = new CuttableFactory();
        int cuttableSize = 50;

        private void Awake()
        {
            foreach (CuttableSO t in cuttableSO)
            {
                cuttablesByID.Add(t.name, new ObjectPool<GameObject>(() => Instantiate(t.asset.gameObject, transform),
                    item => { item.gameObject.SetActive(true); }, item => { item.gameObject.SetActive(false); },
                    item => { Destroy(item.gameObject); }, false, cuttableSize, 100));
            }
        }
        public CuttableItem AddNewItem(CuttableSO cuttableSo, Vector3 position, Quaternion rotation)
        {
            var pool = cuttablesByID[cuttableSo.name];
            if (pool == null)
            {
                Debug.LogError("Pool not found");
                return null;
            }
            GameObject newItem = null;
            pool.Get(out newItem);
            newItem.GetComponent<CuttableItem>().OnDespawn.AddListener(OnDespawn);

            _cuttableFactory.ItemConfigure(newItem, position,rotation,parent);
            return newItem.GetComponent<CuttableItem>();
        }
        
        private void OnDespawn(GameObject CuttableItem)
        {
            CuttableItem item = CuttableItem.GetComponent<CuttableItem>();
            item.OnDespawn.RemoveListener(OnDespawn);
            ObjectPool<GameObject> pool = cuttablesByID[item.SO.name];
            pool.Release(CuttableItem);
        }
        
    }

    internal class CuttableFactory
    {
        public void ItemConfigure(GameObject CuttableItem, Vector3 position, Quaternion rotation,Transform parent)
        {
            CuttableItem.transform.SetParent(parent);
            CuttableItem.transform.position = position;
            CuttableItem.transform.rotation = rotation;
            CuttableItem.transform.localScale = Vector3.one;
        }
    }
}