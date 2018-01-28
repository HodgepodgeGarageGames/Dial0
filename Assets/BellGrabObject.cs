using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellGrabObject : GrabObject {

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
        rb.isKinematic = false;
        
        rb.velocity = grabber.gameObject.GetComponent<Rigidbody>().velocity;
        rb.angularVelocity = grabber.gameObject.GetComponent<Rigidbody>().angularVelocity;

        grabber = null;        
    }

    void FixedUpdate()
    {
        if (grabber != null)
        {
            transform.position = grabber.position;
            transform.rotation = grabber.rotation * Quaternion.Euler(new Vector3 (0.0f, 0.0f, -90.0f));
        }
    }

    /*
     newpos = transform.position;
     var media =  (newpos - oldpos);
     velocity = media /Time.deltaTime;
     oldpos = newpos;
     newpos = transform.position;
     */

}
