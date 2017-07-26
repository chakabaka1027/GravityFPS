using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GravityShift : MonoBehaviour {

    public int gravityShifted = 0;
    float constantForceValue = 196.2f;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
            gravityShifted = 1 - gravityShifted;
            if(gravityShifted == 1) {
                GetComponent<PlayerController>().gravity *= -1;
                
                //GetComponent<PlayerController>().velocityY = 0;
                StopCoroutine("SmoothRotation");
                StartCoroutine("SmoothRotation");
            } else if(gravityShifted == 0) {
                GetComponent<PlayerController>().gravity *= -1;
                //GetComponent<PlayerController>().velocityY = 0;

                StopCoroutine("SmoothRotation");
                StartCoroutine("SmoothRotation");

            }
        }
	}

    IEnumerator SmoothRotation() {
        float percent = 0;
        float time = .2f;
        float speed = 1 / time;
        
        while(percent < 1) {
            percent += Time.deltaTime * speed;
            if(gravityShifted == 1) {
                transform.localEulerAngles = Vector3.Lerp(new Vector3(0, transform.localEulerAngles.y, 0), new Vector3(0, transform.localEulerAngles.y, 180), percent);
            } else if(gravityShifted == 0) {
                transform.localEulerAngles = Vector3.Lerp(new Vector3(0, transform.localEulerAngles.y, 180), new Vector3(0, transform.localEulerAngles.y, 0), percent);
            }
            yield return null;
        }


    }


}
