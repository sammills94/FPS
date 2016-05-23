using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private Camera cam;

	
	private Vector3 velocity1 = Vector3.zero;
	private Vector3 rotation1 = Vector3.zero;
	private Vector3 thrusterForce = Vector3.zero;
	private float cameraRotationX1 = 0f;
	private float currentCameraRotationX = 0f;

	[SerializeField]
	private float cameraRotationLimit = 85f;

	private Rigidbody rb;
	
	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}
	
	public void Move (Vector3 velocity) {
		velocity1 = velocity;
	}
	public void Rotate(Vector3 rotation)
	{
		rotation1 = rotation;
	}
	public void RotateCamera(float cameraRotationX)
	{
		cameraRotationX1 = cameraRotationX;
	}

	public void ApplyThruster (Vector3 _thrusterForce) {
		thrusterForce = _thrusterForce;
	}
	void FixedUpdate() {
		PerformMovement ();
		PerformRotation ();
	}
	
	void PerformMovement() {
		if (velocity1 != Vector3.zero) {
			rb.MovePosition (rb.position + velocity1 * Time.fixedDeltaTime);
		}
		if (thrusterForce != Vector3.zero) {
			rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
		}
	}
	void PerformRotation ()
	{
		rb.MoveRotation(rb.rotation * Quaternion.Euler (rotation1));
		if (cam != null)
		{
			// Set our rotation and clamp it
			currentCameraRotationX -= cameraRotationX1;
			currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
			
			//Apply our rotation to the transform of our camera
			cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
		}
	}

}

