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
        [SerializeField] private GameObject cutted;

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
            //cutted.SetActive(true);
            
            Quaternion rotation = Quaternion.Euler(0, 0, 90);
            var dist = rotation * player.direction;


            var objectPosition = whole.transform.position;
            SlicedHull hull = whole.Slice(objectPosition, dist);

            GameObject upperHull = hull.CreateUpperHull(whole);
            GameObject loverHull = hull.CreateLowerHull(whole);
            upperHull.transform.position = objectPosition;
            loverHull.transform.position = objectPosition;

            var rb1 = loverHull.AddComponent<Rigidbody>();
            loverHull.AddComponent<BoxCollider>();
            var rb2 = upperHull.AddComponent<Rigidbody>();
            upperHull.AddComponent<BoxCollider>();


            rb1.velocity = rigidbody.velocity;
            rb2.velocity = rigidbody.velocity;
            rb1.AddForceAtPosition(dist * player.force, objectPosition, ForceMode.Impulse);
            rb2.AddForceAtPosition(dist * player.force, objectPosition, ForceMode.Impulse);
            collider.enabled = false;
            rigidbody.isKinematic = true;
            this.enabled = false;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Blade>(out Blade player))
            {
                SliceObject(player);
            }
        }
    }
}