using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public enum InputButtons
{
    JUMP,
    FIRE
}

public struct NetworkInputData : INetworkInput
{
    public NetworkButtons buttons;
    // public double move;
    // public double rotate;
    public Vector3 movementInput;
}
