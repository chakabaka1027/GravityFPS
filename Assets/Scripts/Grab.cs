using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour {
    
    public LayerMask grabable;
    bool isGrabbing = false;
    GameObject grabbedObject;
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.F)) {
            PickUp();
        }

        if(Input.GetMouseButtonDown(0) && isGrabbing) {
            grabbedObject.transform.parent.DetachChildren();
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObject.GetComponent<BoxCollider>().enabled = true;
            grabbedObject.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 400);        
            isGrabbing = false;        
        }
	}

    private void PickUp() {
        Vector3 ray = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray, transform.forward, out hit, 2f, grabable) && !isGrabbing) {
            grabbedObject = hit.collider.gameObject;
            grabbedObject.transform.parent = gameObject.transform;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            grabbedObject.GetComponent<BoxCollider>().enabled = false;
            isGrabbing = true;
        } else if(isGrabbing) {
            grabbedObject.transform.parent.DetachChildren();
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObject.GetComponent<BoxCollider>().enabled = true;        
            isGrabbing = false;

        }
    }
}
