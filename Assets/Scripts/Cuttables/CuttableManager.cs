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
        public Cuttables.CuttableObject AddNewItem(CuttableSO cuttableSo, Transform position)
        {
            var pool = cuttablesByID[cuttableSo.name];
            if (pool == null)
            {
                Debug.LogError("Pool not found");
                return null;
            }
            GameObject newItem = null;
            pool.Get(out newItem);
            newItem.GetComponent<CuttableObject>().OnDespawn.AddListener(OnDespawn);

            _cuttableFactory.ItemConfigure(newItem, position);
            newItem.transform.SetParent(parent);
            return newItem.GetComponent<CuttableObject>();
        }
        
        private void OnDespawn(GameObject CuttableItem)
        {
            CuttableObject item = CuttableItem.GetComponent<CuttableObject>();
            item.OnDespawn.RemoveListener(OnDespawn);
            ObjectPool<GameObject> pool = cuttablesByID[item.SO.name];
            pool.Release(CuttableItem);
        }
        
    }

    internal class CuttableFactory
    {
        public void ItemConfigure(GameObject CuttableItem, Transform position)
        {
            throw new NotImplementedException();
        }
    }
}