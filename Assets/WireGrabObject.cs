using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireGrabObject : GrabObject {
    [HideInInspector]
    public Transform grabber = null;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
    }

    public override void release()
    {
        rb.velocity = grabber.GetComponent<Rigidbody>().velocity;
        rb.angularVelocity = grabber.GetComponent<Rigidbody>().angularVelocity;

        grabber = null;
        rb.isKinematic = false;
    }

    void FixedUpdate()
    {
        if (grabber != null)
        {
            transform.position = grabber.position;
            transform.rotation = grabber.rotation;
        }
    }
}
