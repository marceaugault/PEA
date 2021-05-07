using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
	[SerializeField]
	float speed = 5f;
	public float Damage { get; set; }
	public float CurrentLifetime { get; private set; }
	public float Lifetime { get; set; }
	public bool IsActive { get; private set; }

	new Rigidbody rigidbody;

	public LayerMask DestructionOnLayerCollision;

	// Start is called before the first frame update
	void Start()
	{
		rigidbody = GetComponent<Rigidbody>();

		IsActive = false;
		CurrentLifetime = 0;
	}

	private void Update()
	{
		if (IsActive)
		{
			CurrentLifetime += Time.deltaTime;

			if (CurrentLifetime >= Lifetime)
			{
				Deactivate();
			}
		}
	}

	public void Activate(Vector3 position, Vector3 dir)
	{
		IsActive = true;
		CurrentLifetime = 0f;

		transform.position = position;
		rigidbody.velocity = dir.normalized * speed;
	}

	public void Deactivate()
	{
		IsActive = false;
		transform.position = new Vector3(500f, 500f, 0f);

		rigidbody.velocity = Vector3.zero;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (((1 << other.gameObject.layer) & DestructionOnLayerCollision) != 0)
		{
			Deactivate();
		}

		if (((1 << other.gameObject.layer) & LayerMask.GetMask("Player")) != 0)
		{
			other.GetComponent<PlayerController>()?.TakeDamage(Damage);
			Deactivate();
		}
	}
}
