using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	//Crouch
    public LayerMask ground;
    bool isGrounded;
    int crouchToggle = 0;
    float height = .75f;
    float percent = 0;

    //flip grav
    bool hitCeiling = false;

    //movement
    float walkSpeed = 10;
    Vector3 targetWalkAmount;
    Vector3 walkAmount;
    Vector3 smoothDampMoveRef;
    Rigidbody rb;
    CharacterController controller;

    public float gravity = -12;
    public float velocityY;

    [Header("Look Controls")]
	[Range (-5, 5)]
	public float mouseSensitivityX = 6f;
	[Range (-5, 5)]
	public float mouseSensitivityY = 6f;
	float verticalLookRotation;
    GravityShift gravityShift;


	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
        gravityShift = GetComponent<GravityShift>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}
	
	// Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.LeftShift)){
        	walkSpeed = 3;
		} else if (Input.GetKeyUp(KeyCode.LeftShift)){
			walkSpeed = 10;
		}

		if(Input.GetKeyDown(KeyCode.C)){
			//StopCoroutine("CrouchToggle");
			crouchToggle = 1 - crouchToggle;
			StopCoroutine("CrouchToggle");

			StartCoroutine("CrouchToggle");
		}

		//detect if hit ceiling
		Ray ray = new Ray(transform.position, transform.up);

        if(Physics.SphereCast(ray, .5f, height, ground) && !hitCeiling) {
			StartCoroutine("HitCeiling");
        }  


		
    }

	void LateUpdate () {

        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
	    verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
		Camera.main.transform.localEulerAngles = Vector3.left * verticalLookRotation;

        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        Vector3 newRight = Vector3.Cross(Vector3.up, gameObject.transform.forward);
        Vector3 newForward = Vector3.Cross(newRight, Vector3.up);


        if(gravityShift.gravityShifted == 0) {
            Vector3 trueMoveDir = (newRight * Input.GetAxisRaw("Horizontal") + newForward * Input.GetAxisRaw("Vertical"));
            targetWalkAmount = trueMoveDir * walkSpeed + Vector3.up * velocityY;

		    walkAmount = Vector3.SmoothDamp(walkAmount, targetWalkAmount, ref smoothDampMoveRef, 0.1f);
            controller.Move(walkAmount * Time.fixedDeltaTime);

            //reverse controls if gravity is shifted
        } else if (gravityShift.gravityShifted == 1) {
            Vector3 trueMoveDir = (-newRight * Input.GetAxisRaw("Horizontal") + newForward * Input.GetAxisRaw("Vertical"));
            targetWalkAmount = trueMoveDir * walkSpeed + Vector3.up * velocityY;

		    walkAmount = Vector3.SmoothDamp(walkAmount, targetWalkAmount, ref smoothDampMoveRef, 0.1f);
            controller.Move(walkAmount * Time.fixedDeltaTime);

        }

        velocityY += Time.deltaTime * gravity;

        //detect when grounded
        Ray ray = new Ray(transform.position, -transform.up);

        if(Physics.SphereCast(ray, .5f, height, ground)) {
            velocityY = 0;
        }  
       
	}

	IEnumerator HitCeiling(){
		hitCeiling = true;
		velocityY = 0;

		yield return new WaitForSeconds(0.5f);
		hitCeiling = false;
	}


	IEnumerator CrouchToggle(){
		//float percent = 0;
		float time = .5f;
		float speed = 1 / time;

		//crouch
		if(crouchToggle == 1){
			height = .25f;
			while(percent < 1){
				percent += Time.deltaTime * speed;
				gameObject.transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, .5f, 1), percent);
				yield return null;
			}
			percent = 1;

		//stand up
		} if(crouchToggle == 0){
			
			StartCoroutine("LiftWhenUncrouching");
			height = .75f;
			while(percent > 0){
				percent -= Time.deltaTime * speed;
				gameObject.transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), (1 - percent));
				yield return null;
			}

			percent = 0;

		}
	}

	IEnumerator LiftWhenUncrouching(){
		Ray ray = new Ray(transform.position, -transform.up);

			//move character up if 
	        if(Physics.SphereCast(ray, .5f, height, ground)) {
				float percent = 0;
				float t = .5f;
				float s = 1 / t;
				while(percent < 1){
					percent += Time.deltaTime * s;
					if(gravityShift.gravityShifted == 0){
						gameObject.transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition + Vector3.up * .03f, percent);
					} else if(gravityShift.gravityShifted == 1){
						gameObject.transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition - Vector3.up * .03f, percent);
					} 
					yield return null;
				}
	        }  
	}
    
}
