using System;
using UnityEditor;
using UnityEngine;

namespace Cuttables
{
    public class CuttableObject : MonoBehaviour
    {
        
       [SerializeField]private GameObject whole;
       [SerializeField]private GameObject cutted;
        private bool isCut;

        private Rigidbody rigidbody;
        private BoxCollider collider;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<BoxCollider>();
        }

        [ContextMenu("Test Slice")]
        private void Slice()
        {
            whole.SetActive(false);
            cutted.SetActive(true);
            collider.enabled = false;
            rigidbody.isKinematic = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Slice"))
            {
             //   Slice();
            }   
            
        }
    }
}