using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float speed;
	public float turnSpeed;

	public GameObject bullet;
	public Transform bulletSpawn;

	public LayerMask layerMask;
	public float skinWidth = 0.1f;
	private Rigidbody myRigidbody;
	private Vector3 previousPosition;
	private float minimumExtent;
	private float partialExtent;
	private float sqrtMinimumExtent;


	// Use this for initialization
	void Start () 
	{
		rigidbody.freezeRotation = true;
		rigidbody.drag = 4.0f;

		myRigidbody = rigidbody;
		previousPosition = myRigidbody.position;
		minimumExtent = Mathf.Min (Mathf.Min(collider.bounds.extents.x, collider.bounds.extents.y), collider.bounds.extents.z);
		partialExtent = minimumExtent * (1.0f - skinWidth);
		sqrtMinimumExtent = minimumExtent * minimumExtent;
	}

	void Update()
	{
		if (Input.GetButton ("Fire1")) {
						Instantiate (bullet, bulletSpawn.position, bulletSpawn.rotation);
				}

		float steer = Input.GetAxis ("Horizontal");
		float gas = Input.GetAxis ("Vertical");
		
		if (gas != 0) {
			float moveDist = gas * speed * Time.deltaTime;
			
			transform.Translate(Vector3.forward * moveDist);
		}
		
		if (steer != 0) {
			float turnAngle = steer * turnSpeed * Time.deltaTime * gas;
            transform.Rotate(0, turnAngle ,0);
        }
	}

	void FixedUpdate()
	{
		Vector3 movementThisStep = myRigidbody.position - previousPosition;
		float movementSqrtMagnitude = movementThisStep.sqrMagnitude;

		if (movementSqrtMagnitude > sqrtMinimumExtent) 
		{
			float movementMagnitude = Mathf.Sqrt(movementSqrtMagnitude);
			RaycastHit hitInfo;

			if(Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, layerMask.value))
			{
				myRigidbody.position = hitInfo.point - (movementThisStep/movementMagnitude) * partialExtent;
			}

			previousPosition = myRigidbody.position;
		}
	}
}
