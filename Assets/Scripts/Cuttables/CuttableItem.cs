using System;
using Cuttables;
using EzySlice;
using Player;
using UnityEditor;
using UnityEngine;
using Plane = EzySlice.Plane;

namespace Cuttables
{

    class CuttableItem : Cuttable
    {
       

        private void SliceObject(Blade player)
        {
            whole.SetActive(false);

            Quaternion rotation = Quaternion.Euler(0, 0, 90);

            Vector3 dist = entryPoint - exitPoint;
            dist.Normalize();
            dist = rotation * dist;

            Vector3 objectPosition = whole.transform.position;
            SlicedHull hull = whole.Slice(objectPosition, dist);

            Debug.Log(hull);
            upperHull = hull.CreateUpperHull(whole);
            loverHull = hull.CreateLowerHull(whole);

            SetHullTransform(loverHull, objectPosition);
            SetHullTransform(upperHull, objectPosition);

            SetRB(loverHull, dist, player.force, objectPosition);
            SetRB(upperHull, dist, player.force, objectPosition);

            collider.enabled = false;
            rigidbody.isKinematic = true;
            enabled = false;
            OnCut.Invoke(gameObject);
        }

        private void SetHullTransform(GameObject hull, Vector3 objectPosition)
        {
            hull.transform.parent = transform;
            hull.transform.localScale = Vector3.one;
            hull.transform.position = objectPosition;
            hull.transform.rotation = transform.rotation;
        }

        private void SetRB(GameObject hull, Vector3 dist, float force, Vector3 objectPosition)
        {
            Rigidbody rb = hull.AddComponent<Rigidbody>();
            hull.AddComponent<BoxCollider>();
            rb.velocity = rigidbody.velocity;
            rb.AddForceAtPosition(dist * force, objectPosition, ForceMode.Impulse);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Blade>(out Blade player) && player.isSlicing)
            {
                entryPoint = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Blade>(out Blade player) && player.isSlicing)
            {
                exitPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                SliceObject(player);
            }
        }
    }
    }
