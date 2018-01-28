using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Linq;

public class Call {
    public float RingToleranceGood;
    public float RingToleranceNeutral;
    public float CallLength;
    public int Caller;
    public int Callee;
    public IncomingCalls call;
    public float ElapsedTimeToAnswer = 0.0f;
    public float ElapsedTime = 0.0f;

    public Call(Switchboard switchboard, List<Call> activeCalls)
    {
        call = switchboard.allCalls[Random.Range(0, switchboard.allCalls.Count)];

        RingToleranceGood = 10.0f;
        RingToleranceNeutral = 15.0f;
        CallLength = 5.0f;

        do
        {
            Caller = Random.Range(0, 47);
        } while (activeCalls.Any(x => x.Caller == Caller || x.Callee == Caller));
        
        Callee = call.Destination;
    }
}
