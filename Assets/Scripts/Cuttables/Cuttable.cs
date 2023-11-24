using Player;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Cuttables
{
    public class Cuttable : MonoBehaviour
    {
        [SerializeField] protected GameObject whole;
        [SerializeField] protected GameObject upperHull;
        [SerializeField] protected GameObject loverHull;

        public UnityEvent<GameObject> OnCut;
        public UnityEvent<GameObject> OnDespawn;
        protected Vector3 entryPoint;
        protected Vector3 exitPoint;
        private bool isCut;

        protected Rigidbody rigidbody;
        protected BoxCollider collider;
        public CuttableSO SO;
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<BoxCollider>();
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