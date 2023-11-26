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
        [SerializeField] private List<CuttableSO> cuttableSO;
        [SerializeField] private CuttableSO bomb;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private Transform parent;
        private Dictionary<string, ObjectPool<GameObject>> cuttablesByID = new();

        public CuttableFactory _cuttableFactory;
        private RandomCuttableFactory bombFactory;
        public CuttableBuilder _cuttableBuilder = new();
        int cuttableSize = 2;

        private void Awake()
        {
            foreach (CuttableSO t in cuttableSO)
            {
                Debug.Log(t.name);
                cuttablesByID.Add(t.name, new ObjectPool<GameObject>(() => Instantiate(t.asset.gameObject, transform),
                    item => { item.gameObject.SetActive(true); }, item => { item.gameObject.SetActive(false); },
                    item => { Destroy(item.gameObject); }, false, cuttableSize, 100));
            }
            cuttablesByID.Add(bomb.name, new ObjectPool<GameObject>(() => Instantiate(bomb.asset.gameObject, transform),
                item => { item.gameObject.SetActive(true); }, item => { item.gameObject.SetActive(false); },
                item => { Destroy(item.gameObject); }, false, cuttableSize, 100));

            _cuttableFactory = new RandomCuttableFactory(cuttableSO);
            bombFactory = new RandomCuttableFactory(cuttableSO);
            bombFactory.AddToList(bomb);
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
            cuttable.SO = cuttableSo;
            cuttable.OnDespawn.AddListener(OnDespawn);
            cuttable.enabled = true;
            _cuttableBuilder.ItemConfigure(newItem, position, rotation, size, parent);
            if (cuttable is not Bomb)
            {
                cuttable.OnCut.AddListener(gameManager.AddPoint);
            }
            else
            {
                cuttable.OnCut.AddListener(gameManager.Explode);
            }

            return cuttable;
        }

        private void OnDespawn(GameObject CuttableItem)
        {
            Cuttable item = CuttableItem.GetComponent<Cuttable>();
            item.OnDespawn.RemoveListener(OnDespawn);
            Destroy(item.loverHull);
            Destroy(item.upperHull);
            item.rigidbody.velocity = Vector3.zero;
            item.rigidbody.angularVelocity = Vector3.zero;
            item.isCut = false;
            ObjectPool<GameObject> pool = cuttablesByID[item.SO.name];
            pool.Release(CuttableItem);
        }

        public void SetBombFactory()
        {
            _cuttableFactory = bombFactory;
        }
    }
}