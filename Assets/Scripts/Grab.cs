﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour {
    
    public LayerMask grabable;
    bool isGrabbing = false;
    GameObject grabbedObject;
    float positionLerpSpeed = 30;
    float rotationLerpSpeed = 25;
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.F)) {
            PickUp();
        }

        if(Input.GetMouseButtonDown(0) && isGrabbing) {
           Throw();
        }

        if(FindObjectOfType<PlayerController>().isSlowing){
        	positionLerpSpeed = 30 * 4;
        	rotationLerpSpeed = 25 * 4;
		} else if (FindObjectOfType<PlayerController>().isSlowing){
			positionLerpSpeed = 30;
    		rotationLerpSpeed = 25;		
    	}
	}

	void LateUpdate(){
		if(isGrabbing){
        	Carry(grabbedObject);
        }
	}

    private void PickUp() {
        Vector3 ray = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if(Physics.SphereCast(ray, .4f, transform.forward, out hit, 2.5f, grabable) && !isGrabbing) {
            grabbedObject = hit.collider.gameObject;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            if(grabbedObject.GetComponent<BoxCollider>() != null) {
                grabbedObject.GetComponent<BoxCollider>().enabled = false;
            } else if(grabbedObject.GetComponent<SphereCollider>() != null) {
                grabbedObject.GetComponent<SphereCollider>().enabled = false;
            }
            isGrabbing = true;
        } else if(isGrabbing) {
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
		isGrabbing = false;        
        grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        if(grabbedObject.GetComponent<BoxCollider>() != null) {
            grabbedObject.GetComponent<BoxCollider>().enabled = true;
        } else if(grabbedObject.GetComponent<SphereCollider>() != null) {
            grabbedObject.GetComponent<SphereCollider>().enabled = true;
        }               
        grabbedObject.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 40, ForceMode.Impulse);        

    }

    void Carry(GameObject obj){
    	obj.transform.position = Vector3.Lerp(obj.transform.position, Camera.main.transform.position + Camera.main.transform.forward * 2, Time.fixedDeltaTime * positionLerpSpeed);
		obj.transform.eulerAngles = Vector3.Lerp(obj.transform.eulerAngles, new Vector3(0, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z), Time.fixedDeltaTime * rotationLerpSpeed);
    }
}
