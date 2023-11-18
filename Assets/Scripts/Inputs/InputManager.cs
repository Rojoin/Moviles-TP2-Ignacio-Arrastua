using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Serialization;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;


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

        public void OnTouchInput(InputAction.CallbackContext ctx)
        {
           var a = ctx.ReadValue<TouchState>();
            switch (a.phase)
            {
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    touchChannel.RaiseEvent(true);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    touchChannel.RaiseEvent(false);
                    break;
            }
        }
    }
}