using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Vector2ChannelSO inputChannel;
        private PlayerInput _playerInput;

        private void OnEnable()
        {
            inputChannel.Subscribe(_playerInput.actions.);
        }

        private void OnDestroy()
        {
            throw new NotImplementedException();
        }
    }
}