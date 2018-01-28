using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jackable : MonoBehaviour
{
    public int id;
    public WireGrabObject currently_plugged_in;
    private GameObject flashy;

    private int light_state = 0;
    private float light_flash = 0.0f;

    public Material unlit;
    public Material lit;

    private const float blink_speed = 0.7f;

    // Use this for initialization
    void Start () {
        currently_plugged_in = null;
        foreach (Transform child in transform)
        {
            if (child.name == "Flashy")
            {
                flashy = child.gameObject;
                break;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (light_state == 2)
        {
            float old_flash = light_flash;
            light_flash += Time.deltaTime;

            if (old_flash < blink_speed && light_flash >= blink_speed)
            {
                flashy.GetComponent<MeshRenderer>().material = unlit;
            }

            if (old_flash < (2.0f * blink_speed) && light_flash >= (2.0f * blink_speed))
            {
                flashy.GetComponent<MeshRenderer>().material = lit;
                light_flash -= (2.0f * blink_speed);
            }
        }
	}

    public void set_light_state(int s)
    {
        light_state = s;
        if (light_state == 0)
        {
            flashy.GetComponent<MeshRenderer>().material = unlit;
        }
        else
        {
            flashy.GetComponent<MeshRenderer>().material = lit;
            light_flash = 0.0f;
        }
    }

    public bool plug_try_the_waters()
    {
        if (currently_plugged_in == null)
            return true;
        else
            return false;
    }

    public void plugin(WireGrabObject w)
    {
        currently_plugged_in = w;
        SetMyTransformTo(currently_plugged_in.transform);
        currently_plugged_in.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void unplug()
    {
        currently_plugged_in = null;
    }

    public void SetMyTransformTo(Transform t)
    {
        t.position = transform.position;
        t.rotation = transform.rotation * Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f));
    }
}
