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

    private float rotate = 0f;

    private int move = 0;

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

            if(buttons.IsSet(InputButtons.FORWARD)){
                move=1;
            }else if(buttons.IsSet(InputButtons.BACKWARD)){
                move=-1;
            }else{
                move=0;
            }

            if(buttons.IsSet(InputButtons.LEFT)){
                if(rotate==0f){
                    rotate=359.9f;
                }else{
                    rotate=rotate-0.1f;
                }
            }else if(buttons.IsSet(InputButtons.RIGHT)){
                if(rotate==359.9f){
                    rotate=0f;
                }else{
                    rotate=rotate+0.1f;
                }
            }

            moveVector = new Vector3((float)(move * Math.Sin(rotate)), 0.0f, (float)(move * Math.Cos(rotate)));
            networkCharacterController.Move(moveSpeed * moveVector * Runner.DeltaTime);

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
