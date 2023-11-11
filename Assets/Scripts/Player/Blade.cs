using System;
using System.Collections.Generic;
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
        [SerializeField] public float force = 50.0f;
        private Camera mainCamera;
        public Plane plane;
        public Vector3 direction;
        public Vector3 newPosition;
        private Collider _collider;
        private bool isSlicing;
        private bool isTouching;
        private Vector3 inputPosition;
        private LinkedList<Vector3> previousPosition = new LinkedList<Vector3>();
        [SerializeField] private int maxPreviousPos;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            mainCamera = Camera.main;
     
        }

        private void OnEnable()
        {
            plane = new Plane(mainCamera.transform.position, force);
            StopSlice();
            inputMovementChannel.Subscribe(SetInputPosition);
        }


        private void SetInputPosition(Vector2 obj)
        {
            inputPosition = obj;
            ContinueSlice();
        }

        private void OnDisable()
        {
            inputMovementChannel.Unsubscribe(SetInputPosition);
            StopSlice();
        }

        private void Update()
        {
           
        }

        private void ContinueSlice()
        {
            Debug.Log("Slice Continue");

            newPosition = mainCamera.ScreenToWorldPoint(inputPosition);
            newPosition.z = 0.0f;
            direction = newPosition - transform.position;
            previousPosition.AddFirst(newPosition);
            if (previousPosition.Count > maxPreviousPos)
            {
                previousPosition.RemoveLast();
            }
            

            float velocity = direction.magnitude / Time.deltaTime;
            _collider.enabled = velocity > minSliceVelocity;
            isSlicing = velocity > minSliceVelocity;
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