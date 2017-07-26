using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public LayerMask ground;
    bool isGrounded;

    float walkSpeed = 5;
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
        } else if (gravityShift.gravityShifted == 1) {
            Vector3 trueMoveDir = (-newRight * Input.GetAxisRaw("Horizontal") + newForward * Input.GetAxisRaw("Vertical"));
            targetWalkAmount = trueMoveDir * walkSpeed + Vector3.up * velocityY;

		    walkAmount = Vector3.SmoothDamp(walkAmount, targetWalkAmount, ref smoothDampMoveRef, 0.1f);
            controller.Move(walkAmount * Time.fixedDeltaTime);

        }

        
        

        velocityY += Time.deltaTime * gravity;

        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 1.5f, ground)) {
            velocityY = 0;
        }
        
        
	}

    
}
