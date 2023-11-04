using System;
using UnityEngine;

namespace Player
{
    public class Blade : MonoBehaviour
    {
        private Camera mainCamera;
        private Vector3 direction;
        private Vector3 newPosition;
        [SerializeField] private float minSliceVelocity = 0.01f;
        private float force = 50.0f;
        private bool isSlicing;
        private SphereCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            StopSlice();
        }

        private void OnDisable()
        {
            StopSlice();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartSlice();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                StopSlice();
            }
            else if (isSlicing)
            {
                ContinueSlice();
            }
        }

        private void ContinueSlice()
        { 
            Debug.Log("Slice Continue");
            Vector3 inputPosition = Input.mousePosition;
            
            newPosition = mainCamera.ScreenToWorldPoint(inputPosition);
            newPosition.z = 0.0f;
            direction = newPosition - transform.position;
            
            
            
            float velocity = direction.magnitude / Time.deltaTime;
            _collider.enabled = velocity > minSliceVelocity;
            transform.position = newPosition;
        }

        private void StartSlice()
        {
            Debug.Log("Slice Start");
            Vector3 inputPosition = Input.mousePosition;
            newPosition = mainCamera.ScreenToWorldPoint(inputPosition);
            newPosition.z = 0.0f;
            Debug.Log(newPosition);
            transform.position = newPosition;

            isSlicing = true;
            _collider.enabled = isSlicing;
        }

        private void StopSlice()
        {
            Debug.Log("Slice Stop");
            isSlicing = false;
            _collider.enabled = false;
        }
    }
}