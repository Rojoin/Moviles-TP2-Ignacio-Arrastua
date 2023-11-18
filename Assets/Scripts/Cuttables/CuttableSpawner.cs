using System;
using System.Collections;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Cuttables
{
    public class CuttableSpawner : MonoBehaviour
    {
        private BoxCollider spawnArea;
        [SerializeField] private CuttableSO[] cuttableObjects;
        [SerializeField] private CuttableManager _cuttableManager;
        [SerializeField] float minSpawnDelay = 0.25f;
        [SerializeField] float maxSpawnDelay = 1.0f;
        [SerializeField] private float minAngle = -15;
        [SerializeField] private float maxAngle = 15;
        [SerializeField] private float timeUntilDestroy = 2.0f;
        [SerializeField] private float minForce;
        [SerializeField] private float maxForce;

        private void Awake()
        {
            spawnArea = GetComponent<BoxCollider>();
            _cuttableManager = GetComponent<CuttableManager>();
        }

        private void OnEnable()
        {
            StartCoroutine(nameof(SpawnObjects));
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator SpawnObjects()
        {
            while (enabled)
            {
                var item = _cuttableManager._cuttableFactory.GetItem();
                
                Vector3 position = new Vector3();
                position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
                position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
                position.z = 0;

                Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(minAngle, maxAngle));
                var inst = _cuttableManager.AddNewItem(item, position, rotation);

                float force = Random.Range(minForce, maxForce);
                inst.GetComponent<Rigidbody>().AddForce(inst.transform.up * force, ForceMode.Impulse);
                yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            }
        }
    }
}