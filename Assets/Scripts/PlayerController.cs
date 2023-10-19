using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Fusion;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private NetworkCharacterControllerPrototype networkCharacterController = null;

    [SerializeField]
    private Bullet bulletPrefab;

    [SerializeField]
    private float moveSpeed = 15f;

    private Vector3 moveVector = new Vector3(0.0f, 0.0f, 0.0f);

    [Networked]
    public NetworkButtons ButtonsPrevious { get; set; }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            NetworkButtons buttons = data.buttons;
            var pressed = buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = buttons;

            Vector3 moveVector = data.movementInput.normalized;
            networkCharacterController.Move(moveSpeed * moveVector * Runner.DeltaTime);
            // double move = data.move;
            // double rotate = data.rotate;
            // moveVector = new Vector3((float)(move * Math.Sin(rotate)), 0.0f, (float)(move * Math.Cos(rotate)));
            // networkCharacterController.Move(moveSpeed * moveVector * Runner.DeltaTime);

            if (pressed.IsSet(InputButtons.JUMP))
            {
                networkCharacterController.Jump();
            }

            if (pressed.IsSet(InputButtons.FIRE))
            {
                Runner.Spawn(
                    bulletPrefab,
                    transform.position + transform.TransformDirection(Vector3.forward),
                    Quaternion.LookRotation(transform.TransformDirection(Vector3.forward)),
                    Object.InputAuthority);
            }
        }
    }
}
