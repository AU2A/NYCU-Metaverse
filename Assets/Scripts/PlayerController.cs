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
    private MeshRenderer[] _visuals;

    // [SerializeField]
    // private Bullet bulletPrefab;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private float moveSpeed = 10f;

    [SerializeField]
    private float spinSpeed = 5f;

    [Networked]
    public NetworkButtons ButtonsPrevious { get; set; }

    private float rotate = 0f;

    private int move = 0;

    // private Vector3 myPos = new Vector3(0.0f, 0.0f, 0.0f);

    private Vector3 moveVector = new Vector3(0.0f, 0.0f, 0.0f);


    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            _camera.enabled = true;
            _camera.GetComponent<AudioListener>().enabled = true;
            foreach (var visual in _visuals)
            {
                visual.enabled = false;
            }
        }
        else
        {
            _camera.enabled = false;
            _camera.GetComponent<AudioListener>().enabled = false;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            NetworkButtons buttons = data.buttons;
            var pressed = buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = buttons;

            if (buttons.IsSet(InputButtons.FORWARD))
                move = 1;
            else if (buttons.IsSet(InputButtons.BACKWARD))
                move = -1;
            else
                move = 0;

            if (buttons.IsSet(InputButtons.LEFT))
                if (rotate <= -180f)
                    rotate = 180f - spinSpeed;
                else
                    rotate = rotate - spinSpeed;
            else if (buttons.IsSet(InputButtons.RIGHT))
                if (rotate >= 180f - spinSpeed)
                    rotate = -180f;
                else
                    rotate = rotate + spinSpeed;

            // Debug.Log(transform.rotation);
            moveVector = new Vector3(move * Mathf.Sin(rotate * Mathf.Deg2Rad), 0.0f, move * Mathf.Cos(rotate * Mathf.Deg2Rad));
            // transform.position+=moveSpeed * moveVector * Runner.DeltaTime;
            networkCharacterController.Move(moveSpeed * moveVector * Runner.DeltaTime);



            if (pressed.IsSet(InputButtons.JUMP))
            {
                networkCharacterController.Jump();
            }

            // if (pressed.IsSet(InputButtons.FIRE))
            // {
            //     Runner.Spawn(
            //         bulletPrefab,
            //         transform.position + transform.TransformDirection(Vector3.forward),
            //         Quaternion.LookRotation(transform.TransformDirection(Vector3.forward)),
            //         Object.InputAuthority);
            // }
        }
    }
}
