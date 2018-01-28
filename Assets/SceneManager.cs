using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class SceneManager : MonoBehaviour {

    public AudioSource BellAudioSource;

    private float CustomerSatisfaction = 50;
    public List<Call> activeCalls = new List<Call>();
    private Switchboard switchboard = new Switchboard();

    public AudioSource ringer;
    public AudioSource buzzer;
    public AudioSource ding;

    private float TimeOnShift = 0.0f;
    public float EndOfShift = 0.0f;
    private float timeSinceLastIncomingCall = 0.0f;
    private float minTimeBetweenCalls = 20.0f;

    void StartCall()
    {
        if (!(ringer.isPlaying))
            ringer.Play();

        Call newcall = new Call(switchboard, activeCalls);

        Jackable[] jax = GetComponentsInChildren<Jackable>();

        foreach (Jackable j in jax)
        {
            if (j != null)
            {
                if (j.id == newcall.Caller)
                {
                    j.set_light_state(2);
                    break;
                }
            }
        }

        activeCalls.Add(newcall);
    }
    
    public void OnPlugIn(Wire wire, Jackable jack)
    {
        Jackable a_jack = wire.plug_a.GetComponent<WireGrabObject>().currently_plugged_in_to;
        Jackable b_jack = wire.plug_b.GetComponent<WireGrabObject>().currently_plugged_in_to;

        if (a_jack == null || b_jack == null)
        {
            var thisCall = activeCalls.Find(a => (a_jack == null) ? a.Caller == b_jack.id : a.Caller == a_jack.id);
            if (thisCall != null)
            {
                if (jack.id == thisCall.Caller)
                    jack.set_light_state(1);
            }
        }
        else
        {
            Jackable otherEndOfWire = jack.id == a_jack.id ? b_jack : a_jack;

            var thisCall = activeCalls.Find(a => a.Caller == a_jack.id || a.Caller == b_jack.id);

            if (thisCall != null)
            {
                if (jack.id == thisCall.Caller)
                {
                    jack.set_light_state(1);
                    otherEndOfWire.set_light_state(1);
                }
                else
                {
                    if (otherEndOfWire.id == thisCall.Caller)
                        jack.set_light_state(1);
                }
            }

            if (thisCall != null)
            {
                if (jack.id == -1 || otherEndOfWire.id == -1)
                {
                    OnBellConnection(thisCall);
                    if (thisCall.call.state == IncomingCalls.CallState.unanswered)
                    {                        
                        thisCall.call.state = IncomingCalls.CallState.answered;
                    }
                }
                else if (jack.id == thisCall.Callee || otherEndOfWire.id == thisCall.Callee)
                {
                    OnSuccessfulConnection(thisCall);
                    thisCall.call.state = IncomingCalls.CallState.connected;
                }
                else
                {
                    jack.set_light_state(0);
                    otherEndOfWire.set_light_state(0);
                    OnBadConnection(thisCall);                    
                }
            }
        }
    }

    void OnSuccessfulConnection(Call call)
    {
        ding.Play();

        if (call.ElapsedTimeToAnswer <= call.RingToleranceGood)
        {
            CustomerSatisfaction = Mathf.Min(CustomerSatisfaction + 5, 100);
        }

        
    }

    void OnBadConnection(Call thisCall)
    {
        buzzer.loop = false;
        buzzer.Play();
        CustomerSatisfaction -= 10;

        activeCalls.Remove(thisCall);
    }

    void OnBellConnection(Call thisCall)
    {
        BellAudioSource.clip = thisCall.call.Audio;
        BellAudioSource.loop = false;
        BellAudioSource.Play();
    }

    public void OnUnplug(Jackable jack)
    {
        var thisCall = activeCalls.Find(a => a.Caller == jack.id);
        if (thisCall != null)
        {
            jack.currently_plugged_in.ParentWire.plug_a.GetComponent<WireGrabObject>().currently_plugged_in_to.set_light_state(0);
            if (jack.currently_plugged_in.ParentWire.plug_b.GetComponent<WireGrabObject>().currently_plugged_in_to != null)
                jack.currently_plugged_in.ParentWire.plug_b.GetComponent<WireGrabObject>().currently_plugged_in_to.set_light_state(0);
        }
        else
            jack.set_light_state(0);

        if (jack.id >= 0)
        {
            foreach (Call c in activeCalls)
            {
                if (c.Caller == jack.id || c.Callee == jack.id)
                {
                    jack.currently_plugged_in.ParentWire.plug_a.GetComponent<WireGrabObject>().currently_plugged_in_to.set_light_state(0);
                    jack.currently_plugged_in.ParentWire.plug_b.GetComponent<WireGrabObject>().currently_plugged_in_to.set_light_state(0);
                    OnBadConnection(c);
                    break;
                }
            }
        }

        BellAudioSource.Stop();
    }

    // Use this for initialization
    void Start () {
        switchboard.initCalls();
    }

    // Update is called once per frame
    void Update()
    {
        TimeOnShift += Time.deltaTime;
        if (CustomerSatisfaction <= 0)
        {
            //GameOver();
        }
        if (TimeOnShift >= EndOfShift)
        {
            //YouWin();
        }

        var callsToRemove = new List<Call>();
        foreach (var call in activeCalls)
        {
            if (call.call.state == IncomingCalls.CallState.unanswered)
            {
                call.ElapsedTimeToAnswer += Time.deltaTime;
                if (call.ElapsedTimeToAnswer > call.RingToleranceNeutral)
                {
                    callsToRemove.Add(call);
                    CustomerSatisfaction -= 5;
                }
            }

            if (call.call.state == IncomingCalls.CallState.connected)
            {
                call.ElapsedTime += Time.deltaTime;
                if (call.ElapsedTime >= call.CallLength)
                {
                    callsToRemove.Add(call);
                    CustomerSatisfaction += 5;                    
                }
            }
        }

        foreach (Call c in callsToRemove)
        {
            Jackable[] jax = GetComponentsInChildren<Jackable>();
            foreach (Jackable j in jax)
            {
                if (j != null)
                {
                    if (j.id == c.Caller || j.id == c.Callee)
                    {
                        j.set_light_state(0);
                    }
                }
            }

            activeCalls.Remove(c);
        }

        if (!activeCalls.Any(x => x.call.state == IncomingCalls.CallState.unanswered))
        {
            ringer.Stop();
        }

        timeSinceLastIncomingCall += Time.deltaTime;
        if (!activeCalls.Any(c => c.call.state == IncomingCalls.CallState.unanswered) && timeSinceLastIncomingCall >= minTimeBetweenCalls)
        {
            minTimeBetweenCalls = Mathf.Max(5.0f, minTimeBetweenCalls - 0.5f);

            timeSinceLastIncomingCall = 0;
            StartCall();
        }
    }
}
