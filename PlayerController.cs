using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {
	
	[SerializeField]
	private float speed = 10f;
	[SerializeField]
	private float lookSensitivity = 3f;
	private PlayerMotor motor1;

	[SerializeField]
	private float thrusterForce = 1000f;
	
	void Start (){
		motor1 = GetComponent<PlayerMotor>();
		
		
	}
	void Update() {
		
		float xMov = Input.GetAxisRaw("Horizontal");
		float zMov = Input.GetAxisRaw("Vertical");
		Vector3 movHorizontal = transform.right * xMov;
		Vector3 movVertical = transform.forward * zMov;
		Vector3 velocity = (movHorizontal + movVertical) * speed;
		Vector3 _thrusterForce = Vector3.zero;

	
		float yRot = Input.GetAxisRaw("Mouse X");
		
		Vector3 rotation = new Vector3(0f, yRot, 0f) * lookSensitivity;
		
		//Apply rotation
		motor1.Rotate(rotation);
		
		//Calculate camera rotation as a 3D vector (turning around)
		float xRot = Input.GetAxisRaw("Mouse Y");
		
		float cameraRotationX = xRot * lookSensitivity;
		
		//Apply camera rotation
		motor1.RotateCamera(cameraRotationX);

		if (Input.GetButton ("Jump")) {
			_thrusterForce = Vector3.up * thrusterForce;
		}

		motor1.ApplyThruster (_thrusterForce);
		motor1.Move (velocity);
		motor1.Rotate (rotation);
	}
	
}
