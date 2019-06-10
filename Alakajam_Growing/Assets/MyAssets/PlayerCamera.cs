using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    public bool requireControl;
    public PlayerMovement pm;

    public bool lockedMouse = false;

    public bool horizontal = false;
    public bool vertical = false;

    public float verticalValue = 0;

    public float horizontalValue = 0;
    public float minVertical = -70;
    public float maxVertical = 80;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(!requireControl || pm.hasControl) {
            float dx = Input.GetAxis("Mouse X");
            float dy = Input.GetAxis("Mouse Y");

            if (horizontal) {
                horizontalValue += 4 * dx;
                horizontalValue = (horizontalValue + 360) % 360;
                verticalValue = Mathf.Clamp(verticalValue, minVertical, maxVertical);
                transform.localRotation = Quaternion.Euler(0, horizontalValue, 0);
            }
            if (vertical) {
                verticalValue -= 4 * dy;
                verticalValue = Mathf.Clamp(verticalValue, minVertical, maxVertical);
                transform.localRotation = Quaternion.Euler(verticalValue, 0, 0);
            }

            if (Input.GetMouseButtonDown(0)) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            if (Input.GetKey(KeyCode.Escape)) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
