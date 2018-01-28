using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomingCalls
{
    public enum CallState
    {
        unanswered,
        answered,
        connected
    }

    public AudioClip Audio;
    public int Destination;

    public CallState state = CallState.unanswered;
}
