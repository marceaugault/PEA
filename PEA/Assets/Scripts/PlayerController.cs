﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] float MaxLife = 100;
	public float Life { get; private set; }

	[SerializeField] float speed = 5f;

	[SerializeField] float InvicibilityTimerAfterDamaged = 0.5f;
	float currentInvicibilityTimer = 0f;

	new Rigidbody rigidbody = null;

	#region Shoot
	BulletPool bulletController;

	[SerializeField]
	float fireRate = 0.5f;
	float fireTimer;

	bool allowFire;

	#endregion

	// Start is called before the first frame update
	void Start()
    {
		Life = MaxLife;

		rigidbody = GetComponent<Rigidbody>();
		bulletController = FindObjectOfType<BulletPool>();

		fireTimer = fireRate;
		allowFire = false;
	}

    // Update is called once per frame
    void Update()
    {
		currentInvicibilityTimer -= Time.deltaTime;

		Shoot();
	}

	private void FixedUpdate()
	{
		UpdateInputs();
	}

	void UpdateInputs()
	{
		Vector3 dir = Vector3.zero;

		if (Input.GetKey(KeyCode.Z) || Input.GetKeyDown(KeyCode.Z))
			dir += new Vector3(0f, 0f, 1f);

		if (Input.GetKey(KeyCode.S) || Input.GetKeyDown(KeyCode.S))
			dir += new Vector3(0f, 0f, -1f);

		if (Input.GetKey(KeyCode.Q) || Input.GetKeyDown(KeyCode.Q))
			dir += new Vector3(-1f, 0f, 0f);

		if (Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.D))
			dir += new Vector3(1f, 0f, 0f);

		rigidbody.AddForce(dir.normalized * speed * Time.deltaTime * 100f);

		if (Input.GetButton("Fire1"))
			allowFire = true;
		else
			allowFire = false;
		
	}

	void Shoot()
	{
		fireTimer += Time.deltaTime;

		if (!allowFire)
		{
			return;
		}

		if (fireTimer >= fireRate)
		{
			fireTimer = 0f;

			bulletController.FireBullet(transform.position, GetMousePosition() - transform.position, 10f);
		}
	}

	Vector3 GetMousePosition()
	{
		Plane plane = new Plane(Vector3.up, 0);

		float distance;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (plane.Raycast(ray, out distance))
		{
			return ray.GetPoint(distance);
		}

		return Vector3.zero;
	}

	public void TakeDamage(float Damage)
	{
		if (currentInvicibilityTimer <= 0f)
		{
			Life -= Damage;
			currentInvicibilityTimer = InvicibilityTimerAfterDamaged;
		}
	}

	public float GetLife01()
	{
		return Mathf.Clamp01(Life / MaxLife);
	}
}
