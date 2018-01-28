using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lose : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine("flip");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator flip()
    {
        float delta_z = 15.0f;
        float elapsed = 0.0f;
        while (delta_z > -180.0f)
        {
            elapsed += delta_z * Time.deltaTime;
            transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, -transform.localEulerAngles.y, delta_z * Time.deltaTime);
            yield return null;
        }
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 180.0f);
    }
}
