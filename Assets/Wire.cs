using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour {
    public Transform plug_a;
    public Transform plug_b;
    public float length;
    private float length_actual;
    public PhysicMaterial physics_material_ref;

    private GameObject[] wire_node;

    private const int number_of_wire_nodes = 10;

    private LineRenderer lr;

	// Use this for initialization
	void Start () {
        length_actual = length * 3.0f;

        wire_node = new GameObject[number_of_wire_nodes];

        for (int i = 0; i < number_of_wire_nodes; ++i)
        {
            wire_node[i] = new GameObject();
            wire_node[i].transform.SetParent(transform);
            wire_node[i].layer = LayerMask.NameToLayer("Wire");
            Rigidbody rb = wire_node[i].AddComponent<Rigidbody>();
            rb.drag = 1.0f;
            rb.angularDrag = 10.0f;

            wire_node[i].transform.position = (((plug_b.position - plug_a.position) / (((float)number_of_wire_nodes) + 2.0f)) * ((float)(i+1))) + plug_a.position;

            SphereCollider sc = wire_node[i].AddComponent<SphereCollider>();
            sc.radius = 0.02f;
            sc.material = physics_material_ref;
        }

        lr = GetComponent<LineRenderer>();
        lr.positionCount = number_of_wire_nodes+2;

        SpringJoint sj = plug_a.gameObject.AddComponent<SpringJoint>();
        sj.connectedBody = wire_node[0].GetComponent<Rigidbody>();
        sj.autoConfigureConnectedAnchor = false;
        sj.connectedAnchor = Vector3.zero;
        sj.minDistance = length / ((float)(number_of_wire_nodes + 1)); sj.maxDistance = sj.minDistance;
        sj.spring = 10000;
        for (int i = 0; i < number_of_wire_nodes-1; ++i)
        {
            sj = wire_node[i].AddComponent<SpringJoint>();
            sj.connectedBody = wire_node[i + 1].GetComponent<Rigidbody>();
            sj.autoConfigureConnectedAnchor = false;
            sj.connectedAnchor = Vector3.zero;
            sj.minDistance = length / ((float)(number_of_wire_nodes + 1)); sj.maxDistance = sj.minDistance;
            sj.spring = 10000;
        }
        sj = wire_node[number_of_wire_nodes - 1].AddComponent<SpringJoint>();
        sj.connectedBody = plug_b.GetComponent<Rigidbody>();
        sj.autoConfigureConnectedAnchor = false;
        sj.connectedAnchor = Vector3.zero;
        sj.minDistance = length / ((float)(number_of_wire_nodes + 1)); sj.maxDistance = sj.minDistance;
        sj.spring = 10000;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3[] p = new Vector3[number_of_wire_nodes + 2];
        p[0] = plug_a.position;
        for (int i = 0; i < number_of_wire_nodes; ++i)
        {
            p[i + 1] = wire_node[i].transform.position;
        }
        p[number_of_wire_nodes + 1] = plug_b.position;
        lr.SetPositions(p);
	}

    void FixedUpdate()
    {
        WireGrabObject plug_a_wiregrab = plug_a.GetComponent<WireGrabObject>();
        WireGrabObject plug_b_wiregrab = plug_b.GetComponent<WireGrabObject>();

        if (plug_a_wiregrab.grabber != null)
        {
            plug_a.position = plug_a_wiregrab.grabber.position;
            plug_a.rotation = plug_a_wiregrab.grabber.rotation;
        }
        else if (plug_a_wiregrab.currently_plugged_in_to != null)
        {
            plug_a_wiregrab.currently_plugged_in_to.SetMyTransformTo(plug_a);
        }

        if (plug_b_wiregrab.grabber != null)
        {
            plug_b.position = plug_b_wiregrab.grabber.position;
            plug_b.rotation = plug_b_wiregrab.grabber.rotation;
        }
        else if (plug_b_wiregrab.currently_plugged_in_to != null)
        {
            plug_b_wiregrab.currently_plugged_in_to.SetMyTransformTo(plug_b);
        }

        if (plug_a_wiregrab.grabber != null)
        {
            if (plug_b_wiregrab.grabber != null)
            {
                if (Vector3.Distance(plug_a.position, plug_b.position) > length_actual)
                {
                    plug_a.position = ((plug_a.position + plug_b.position) * 0.5f) + ((plug_a.position - plug_b.position).normalized * (length_actual * 0.5f));
                    plug_b.position = ((plug_b.position + plug_a.position) * 0.5f) + ((plug_b.position - plug_a.position).normalized * (length_actual * 0.5f));
                }
            }
            else
            {
                if (plug_b_wiregrab.currently_plugged_in_to != null)
                {
                    if (Vector3.Distance(plug_a.position, plug_b.position) > length_actual)
                    {
                        plug_a.position = ((plug_a.position - plug_b.position).normalized * length_actual) + plug_b.position;
                    }
                }
            }
        }
        else if (plug_b.GetComponent<WireGrabObject>().grabber != null)
        {
            if (plug_a_wiregrab.currently_plugged_in_to != null)
            {
                if (Vector3.Distance(plug_a.position, plug_b.position) > length_actual)
                {
                    plug_b.position = ((plug_b.position - plug_a.position).normalized * length_actual) + plug_a.position;
                }
            }
        }
    }
}
