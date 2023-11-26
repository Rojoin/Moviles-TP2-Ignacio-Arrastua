using System;
using System.Collections.Generic;
using Managers;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Pool;

namespace Cuttables
{
    public class CuttableManager : MonoBehaviour
    {
        [SerializeField] private List<CuttableSO> cuttableSO = new List<CuttableSO>();
        [SerializeField] private GameManager gameManager;
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

        public Cuttable AddNewItem(CuttableSO cuttableSo, Vector3 position, Quaternion rotation, float size)
        {
            var pool = cuttablesByID[cuttableSo.name];
            if (pool == null)
            {
                Debug.LogError("Pool not found");
                return null;
            }

            GameObject newItem = null;
            pool.Get(out newItem);
            Cuttable cuttable = newItem.GetComponent<Cuttable>();
            cuttable.OnDespawn.AddListener(OnDespawn);

            _cuttableBuilder.ItemConfigure(newItem, position, rotation, size, parent);
            if (cuttable is not Bomb)
            {
                cuttable.OnCut.AddListener(gameManager.AddPoint);
            }
            
            return cuttable;
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