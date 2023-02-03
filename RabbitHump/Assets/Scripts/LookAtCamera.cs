using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour
{
	public float minimium = 0;
	public float maximum = 0;
	const int flipValue = 180;
	public bool lockX = false;
	public bool flip = false;
	Camera mainCamera;


	// Use this for initialization
	void Start()
	{
		mainCamera = Camera.main;
	}

	// Update is called once per frame
	void LateUpdate()
	{
		//	transform.rotation = Camera.main.transform.rotation;
		if (mainCamera != null)
		{
			Vector3 targetPosition = mainCamera.transform.position;
			if (lockX)
			{
				//float t = ((targetPosition.y) / maximum) - minimium;
				//t = Mathf.Clamp(t, 0, 1);
				//targetPosition.x = (transform.position.x * (t)) + (targetPosition.x * (1 - t));
				targetPosition.x = transform.position.x;
				targetPosition.z = -30;
			}
			transform.LookAt(targetPosition);
			if (lockX)
				transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + mainCamera.transform.eulerAngles.y, transform.eulerAngles.z);
		}
		if (flip)
			transform.Rotate(Vector3.up * flipValue);
		//viewportPoint = mainCamera.WorldToViewportPoint(transform.position);
		//	transform.eulerAngles = new Vector3(transform.eulerAngles.x , transform.eulerAngles.y + mainCamera.transform.eulerAngles.y, overrideZ);
		//	transform.Rotate(0,mainCamera.transform.eulerAngles.y,0);
	}
}

/*
 * using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour
{

	public float afterModifier = 1;
	const int flipValue = 180;
	public bool flip = false;
	[Tooltip("Used when we want the UI to stay flat")]
	public bool addXValue = false;
	Camera mainCamera;

	
	bool shouldAddXValue = false;
	// Use this for initialization
	void Start()
	{
		mainCamera = Camera.main;
		shouldAddXValue = addXValue;
	}

	// Update is called once per frame
	void Update()
	{
		//	transform.rotation = Camera.main.transform.rotation;
		if (mainCamera != null)
			transform.LookAt(mainCamera.transform,Vector3.);
		if (flip)
			transform.Rotate(Vector3.up * flipValue);
	
        //if (shouldAddXValue)
        //    addXValue = mainCamera.WorldToViewportPoint(transform.position).y <= 0.5f;
  //      if (addXValue)
		//{
		//	//float modifier = (1 - viewportPoint.x);
		//	//modifier *= 1 - viewportPoint.y;
		//	////afterModifier = viewportPoint.x * -10;
		//	//modifier *= afterModifier;


		//	//float modifier = 1 - viewportPoint.x;
		//	//         //if (viewportPoint.x >= 0.5f)
		//	//         //{
		//	//         //    modifier *= -1;
		//	//         //}
		//	//         modifier *= afterModifier;
		//	float modifier = viewportPoint.x <= 0.5f ? -1 : 1;
		//	float value = 1 - viewportPoint.x;
		//	transform.eulerAngles = new Vector3(transform.eulerAngles.x,
		//		transform.eulerAngles.y, (60 * value * modifier) / 2);
		//}
		//Debug.Log(mainCamera.WorldToViewportPoint(transform.position));
		// (mainCamera.WorldToViewportPoint(transform.position).x >= 0.5f ? -0.65f : 0.65f) :
	}
}
*/
