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
           Throw();
        }
	}

    private void PickUp() {
        Vector3 ray = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if(Physics.SphereCast(ray, .25f, transform.forward, out hit, 1.5f, grabable) && !isGrabbing) {
            grabbedObject = hit.collider.gameObject;
            grabbedObject.transform.parent = gameObject.transform;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            if(grabbedObject.GetComponent<BoxCollider>() != null) {
                grabbedObject.GetComponent<BoxCollider>().enabled = false;
            } else if(grabbedObject.GetComponent<SphereCollider>() != null) {
                grabbedObject.GetComponent<SphereCollider>().enabled = false;
            }
            isGrabbing = true;
        } else if(isGrabbing) {
            grabbedObject.transform.parent.DetachChildren();
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            if(grabbedObject.GetComponent<BoxCollider>() != null) {
                grabbedObject.GetComponent<BoxCollider>().enabled = true;
            } else if(grabbedObject.GetComponent<SphereCollider>() != null) {
                grabbedObject.GetComponent<SphereCollider>().enabled = true;
            }            
            isGrabbing = false;

        }
    }
    
    void Throw() {
        grabbedObject.transform.parent.DetachChildren();
        grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        if(grabbedObject.GetComponent<BoxCollider>() != null) {
            grabbedObject.GetComponent<BoxCollider>().enabled = true;
        } else if(grabbedObject.GetComponent<SphereCollider>() != null) {
            grabbedObject.GetComponent<SphereCollider>().enabled = true;
        }               grabbedObject.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 1600);        
        isGrabbing = false;        

    }
}
