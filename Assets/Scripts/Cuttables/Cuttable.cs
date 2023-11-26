using System;
using Player;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Cuttables
{
    public class Cuttable : MonoBehaviour
    {
        [SerializeField] public GameObject whole;
        [SerializeField] public GameObject upperHull;
        [SerializeField] public GameObject loverHull;
        public float despawnTime = 7.0f;
        public UnityEvent<GameObject> OnCut;
        public UnityEvent<GameObject> OnDespawn;
        protected Vector3 entryPoint;
        protected Vector3 exitPoint;
        public bool isCut;

        public Rigidbody rigidbody;
        public BoxCollider collider;
        public CuttableSO SO;

        
        public void OnEnable()
        {
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<BoxCollider>();
            collider.enabled = true;
            isCut = false;
        }

        public void OnDisable()
        {
            OnCut.RemoveAllListeners();
            OnDespawn.RemoveAllListeners();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Blade>(out Blade player) && player.isSlicing)
            {
                entryPoint = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            }
        }
    }
}