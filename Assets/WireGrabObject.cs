using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireGrabObject : GrabObject {
    [HideInInspector]
    public Transform grabber = null;
    private Rigidbody rb;

    private List<Jackable> plug_list;
    [HideInInspector]
    public Jackable currently_plugged_in_to;

    public SceneManager sceneManager;
    public Wire ParentWire;

    public AudioClip jackin0;
    public AudioClip jackin1;
    public AudioClip jackin2;

    private AudioSource jackingin;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        plug_list = new List<Jackable>();
        currently_plugged_in_to = null;
        jackingin = GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Jackable>())
        {
            plug_list.Add(other.GetComponent<Jackable>());
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Jackable>())
        {
            plug_list.Remove(other.GetComponent<Jackable>());
        }
    }

    public override bool grab_try_the_waters()
    {
        if (grabber == null)
            return true;
        else
            return false;
    }

    public override void grab(GameObject controller)
    {
        grabber = controller.transform;
        rb.isKinematic = true;

        //Plug in jack
        if (currently_plugged_in_to != null)
        {
            sceneManager.OnUnplug(currently_plugged_in_to);
            currently_plugged_in_to.unplug();
            currently_plugged_in_to = null;
        }
    }

    public override void release()
    {
        rb.isKinematic = false;
        rb.velocity = grabber.gameObject.GetComponent<Rigidbody>().velocity;
        rb.angularVelocity = grabber.gameObject.GetComponent<Rigidbody>().angularVelocity;

        grabber = null;        

        //Plug in jack
        foreach (Jackable j in plug_list)
        {
            if (j.plug_try_the_waters())
            {
                j.plugin(this);
                currently_plugged_in_to = j;

                sceneManager.OnPlugIn(ParentWire, currently_plugged_in_to);

                jackingin.Stop();

                int r = Random.Range(0, 2);
                if (r == 0)
                    jackingin.clip = jackin0;
                else if (r == 1)
                    jackingin.clip = jackin1;
                else
                    jackingin.clip = jackin2;

                jackingin.Play();

                break;
            }
        }
    }
}
