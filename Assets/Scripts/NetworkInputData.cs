using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public enum InputButtons
{
    FORWARD,
    BACKWARD,
    LEFT,
    RIGHT,
    JUMP,
    FIRE
}

public struct NetworkInputData : INetworkInput
{
    public NetworkButtons buttons;
}
