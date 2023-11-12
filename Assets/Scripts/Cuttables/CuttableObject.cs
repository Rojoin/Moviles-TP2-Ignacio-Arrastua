using System;
using EzySlice;
using Player;
using UnityEditor;
using UnityEngine;
using Plane = EzySlice.Plane;

namespace Cuttables
{
    public class CuttableObject : MonoBehaviour
    {
        [SerializeField] private GameObject whole;
        [SerializeField] private GameObject upperHull;
        [SerializeField] private GameObject loverHull;

        private Vector3 entryPoint;
        private Vector3 exitPoint;
        private bool isCut;

        private Rigidbody rigidbody;
        private BoxCollider collider;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<BoxCollider>();
        }

        [ContextMenu("Test SliceObject")]
        private void SliceObject(Blade player)
        {
            whole.SetActive(false);
           

            Quaternion rotation = Quaternion.Euler(0, 0, 90);
            var dist = entryPoint - exitPoint;
            dist.Normalize();
            dist = rotation * dist;

            var objectPosition = whole.transform.position;
            SlicedHull hull = whole.Slice(objectPosition, dist);

            Debug.Log(hull);
            upperHull = hull.CreateUpperHull(whole);
            loverHull = hull.CreateLowerHull(whole);
            upperHull.transform.parent = this.transform;
            loverHull.transform.parent = this.transform;
            upperHull.transform.position = objectPosition;
            loverHull.transform.position = objectPosition;
            upperHull.transform.rotation = transform.rotation;
            loverHull.transform.rotation = transform.rotation;

            Rigidbody rb1 = loverHull.AddComponent<Rigidbody>();
            loverHull.AddComponent<BoxCollider>();
            Rigidbody  rb2 = upperHull.AddComponent<Rigidbody>();
            upperHull.AddComponent<BoxCollider>();
           


            rb1.velocity = rigidbody.velocity;
            rb2.velocity = rigidbody.velocity;
            rb1.AddForceAtPosition(dist * player.force, objectPosition, ForceMode.Impulse);
            rb2.AddForceAtPosition(dist * player.force, objectPosition, ForceMode.Impulse);
            collider.enabled = false;
            rigidbody.isKinematic = true;
            this.enabled = false;
        }

        


        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Blade>(out Blade player) && player.isSlicing)
            {
                exitPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                SliceObject(player);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Blade>(out Blade player) && player.isSlicing)
            {
                entryPoint = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            }
        }
    }
}