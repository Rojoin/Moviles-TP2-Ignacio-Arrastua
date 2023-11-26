using System;
using System.Collections.Generic;
using System.Numerics;
using Shop;
using UnityEngine;
using UnityEngine.Serialization;
using Plane = UnityEngine.Plane;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Player
{
    public class Blade : MonoBehaviour
    {
        [Header("Channels")]
        [SerializeField] private PlayerConfig player;
        [SerializeField] private Vector2ChannelSO inputMovementChannel;
        [SerializeField] private BoolChannelSO inputTouchChannel;
        [Header("Values")]
        [SerializeField] private float minSliceVelocity = 0.01f;
        [SerializeField] public float force = 50.0f;
        private Camera mainCamera;
        public Plane plane;
        public Vector3 direction;
        public Vector3 newPosition;
        private Collider _collider;
        private float velocity;
        public bool isSlicing => velocity > minSliceVelocity;
        private bool isTouching;
        private Vector3 inputPosition;
        private LinkedList<Vector3> previousPosition = new LinkedList<Vector3>();
        [SerializeField] private int maxPreviousPos;
        private GameObject trail;
        public Material cutMaterial;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            mainCamera = Camera.main;
            trail = Instantiate(player.GetCurrentBlade().asset, this.transform);
            cutMaterial = player.GetCurrentBlade().cutMaterial;
            trail.transform.position = Vector3.zero;
            StopSlice();
        }

        private void OnDestroy()
        {
        }

        private void ChangeTouchStatus(bool state)
        {
            isTouching = state;
            if (!isTouching)
            {
                StopSlice();
            }
        }

        private void OnEnable()
        {
            plane = new Plane(mainCamera.transform.position, force);
            StopSlice();
            inputMovementChannel.Subscribe(SetInputPosition);
            inputTouchChannel.Subscribe(ChangeTouchStatus);
        }


        private void SetInputPosition(Vector2 obj)
        {
            trail.gameObject.SetActive(isTouching);
            if (isTouching)
            {
                inputPosition = obj;
                ContinueSlice();
            }
            else
            {
                StopSlice();
              
            }
        }

        private void OnDisable()
        {
            inputMovementChannel.Unsubscribe(SetInputPosition);
            inputTouchChannel.Unsubscribe(ChangeTouchStatus);
            StopSlice();
        }


        private void ContinueSlice()
        {
            Debug.Log("Slice Continue");

            newPosition = mainCamera.ScreenToWorldPoint(inputPosition);
            newPosition.z = 0.0f;
            if (previousPosition.Count == 0)
            {
                transform.position = newPosition;
            }

            previousPosition.AddFirst(newPosition);
            if (previousPosition.Count > maxPreviousPos)
            {
                previousPosition.RemoveLast();
            }

            direction = newPosition - transform.position;

            velocity = direction.magnitude / Time.deltaTime;
            _collider.enabled = velocity > minSliceVelocity && previousPosition.Count > 1;
            transform.position = newPosition;
        }

        private void StartSlice()
        {
            Debug.Log("Slice Start");

            newPosition = mainCamera.ScreenToWorldPoint(inputPosition);
            newPosition.z = 0.0f;
            transform.position = newPosition;

            _collider.enabled = isSlicing;
        }

        private void StopSlice()
        {
            Debug.Log("Slice Stop");
            _collider.enabled = false;
            previousPosition.Clear();
            velocity = 0;
        }
    }
}