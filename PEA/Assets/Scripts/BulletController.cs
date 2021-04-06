using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
	[SerializeField]
	float speed = 5f;
	
	public float currentLifetime { get; private set; }
	
	public bool isActive { get; private set; }

	new Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
		rigidbody = GetComponent<Rigidbody>();

		isActive = false;
		currentLifetime = 0;
	}

	private void Update()
	{
		if (isActive)
			currentLifetime += Time.deltaTime;
	}

	public void Activate(Vector3 position, Vector3 dir)
	{
		isActive = true;
		currentLifetime = 0f;

		transform.position = position;
		rigidbody.velocity = dir.normalized * speed;
	}

	public void Deactivate()
	{
		isActive = false;
		transform.position = new Vector3(0f, 1000f, 0f);

		rigidbody.velocity = Vector3.zero;
	}
}
