using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPool : MonoBehaviour
{
	[SerializeField]
	GameObject bulletPrefab = null;

	public int poolSize = 30;

	EnemyBulletController[] bulletPool;

	// Start is called before the first frame update
	void Start()
	{
		bulletPool = new EnemyBulletController[poolSize];
		for (int i = 0; i < poolSize; i++)
		{
			var go = Instantiate(bulletPrefab, new Vector3(500f, 500f, 0f), Quaternion.identity);
			bulletPool[i] = go.GetComponent<EnemyBulletController>();
		}
	}

	EnemyBulletController GetAvailableBullet()
	{
		for (int i = 0; i < poolSize; i++)
		{
			if (!bulletPool[i].IsActive)
			{
				return bulletPool[i];
			}
		}

		return null;
	}

	public void FireBullet(Vector3 position, Vector3 dir, float damage, float lifetime)
	{
		dir.y = 0f;

		EnemyBulletController bullet = GetAvailableBullet();

		if (bullet)
		{
			bullet.Activate(position, dir.normalized);
			bullet.Damage = damage;
			bullet.Lifetime = lifetime;
		}
	}
}
