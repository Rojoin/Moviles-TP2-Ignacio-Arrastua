using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class Blade : MonoBehaviour
    {
        
        [Header("Channels")]
        [SerializeField] private Vector2ChannelSO inputMovementChannel;
        [Header("Values")]
        [SerializeField] private float minSliceVelocity = 0.01f;
        [SerializeField] private float force = 50.0f;
        private Camera mainCamera;
        private Vector3 direction;
        private Vector3 newPosition;
        private SphereCollider _collider;
        private bool isSlicing;
        private Vector3 inputPosition;

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            StopSlice();
            inputMovementChannel.Subscribe(SetInputPosition);
        }

        private void SetInputPosition(Vector2 obj)
        {
            inputPosition = obj;
        }

        private void OnDisable()
        {
            inputMovementChannel.Unsubscribe(SetInputPosition);
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

            newPosition = mainCamera.ScreenToWorldPoint(inputPosition);
            newPosition.z = 0.0f;
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