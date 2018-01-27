using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabManager : MonoBehaviour {

    private List<GrabObject> grab_list;

    private SteamVR_TrackedObject trackedObj;

    private GrabObject currently_grabbing;

    // Use this for initialization
    void Start () {
		grab_list = new List<GrabObject>();
        currently_grabbing = null;
    }

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GrabObject>())
        {
            Debug.Log("Yay");
            grab_list.Add(other.GetComponent<GrabObject>());
        }
        
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<GrabObject>())
        {
            grab_list.Remove(other.GetComponent<GrabObject>());
        }
    }

    // Update is called once per frame
    void Update () {
        if (Controller.GetHairTriggerDown())
        {
            if (currently_grabbing != null)
            {
                currently_grabbing.release();
                currently_grabbing = null;
            }
        }

        if (Controller.GetHairTriggerUp())
        {
            foreach (GrabObject grabo in grab_list)
            {
                if (grabo.grab_try_the_waters())
                {
                    grabo.grab(gameObject);
                    currently_grabbing = grabo;
                    break;
                }
            }
        }
    }
}
