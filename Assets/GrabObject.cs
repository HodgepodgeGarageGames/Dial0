using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GrabObject : MonoBehaviour {

    public abstract bool grab_try_the_waters();
    public abstract void grab(GameObject controller);
    public abstract void release();
}
