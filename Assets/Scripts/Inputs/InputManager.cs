using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using TouchPhase = UnityEngine.TouchPhase;

namespace Inputs
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Vector2ChannelSO inputMovementChannel;
        [SerializeField] private BoolChannelSO touchChannel;
        private PlayerInput _playerInput;

        public void OnInputMove(InputAction.CallbackContext ctx)
        {
            inputMovementChannel.RaiseEvent(ctx.ReadValue<Vector2>());
        }

        
    }
}