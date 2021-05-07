using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float Life { get; private set; }

	[SerializeField] float InvicibilityTimerAfterDamaged = 0.5f;
	float currentInvicibilityTimer = 0f;

	new Rigidbody rigidbody = null;

	public bool IsDead { get { return Life <= 0f; } }
	public CharacterStats stats { get; private set; }

	#region Shoot
	BulletPool bulletController;

	float fireTimer;

	bool allowFire;

	#endregion

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		stats = GetComponent<CharacterStats>();
		bulletController = FindObjectOfType<BulletPool>();
	}
	void Start()
    {
		stats.ComputeStats();

		fireTimer = stats.FireRate;
		allowFire = false;

		Life = stats.MaxLife;
	}

    void Update()
    {
		currentInvicibilityTimer -= Time.deltaTime;

		Shoot();
	}

	private void FixedUpdate()
	{
		UpdateInputs();
	}

	public void UpdateStats()
	{
		if (!stats)
		{
			stats = GetComponent<CharacterStats>();
		}
		
		if (stats)
			stats.ComputeStats();
	}

	void UpdateInputs()
	{
		if (IsDead)
			return;

		Vector3 dir = Vector3.zero;

		if (Input.GetKey(KeyCode.Z) || Input.GetKeyDown(KeyCode.Z))
			dir += new Vector3(0f, 0f, 1f);

		if (Input.GetKey(KeyCode.S) || Input.GetKeyDown(KeyCode.S))
			dir += new Vector3(0f, 0f, -1f);

		if (Input.GetKey(KeyCode.Q) || Input.GetKeyDown(KeyCode.Q))
			dir += new Vector3(-1f, 0f, 0f);

		if (Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.D))
			dir += new Vector3(1f, 0f, 0f);

		rigidbody.AddForce(dir.normalized * stats.MoveSpeed * Time.deltaTime * 100f);

		if (Input.GetButton("Fire1"))
			allowFire = true;
		else
			allowFire = false;
		
	}

	void Shoot()
	{
		if (IsDead)
			return;

		fireTimer += Time.deltaTime;

		if (!allowFire)
		{
			return;
		}

		if (fireTimer >= stats.FireRate)
		{
			fireTimer = 0f;

			if (bulletController)
				bulletController.FireBullet(transform.position, GetMousePosition() - transform.position, stats.Damage, stats.BulletLifetime);
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

			if (IsDead)
			{
				UIController ui = FindObjectOfType<UIController>();
				ui.GameOver();
			}
		}
	}

	public float GetLife01()
	{
		return Mathf.Clamp01(Life / stats.MaxLife);
	}
}
