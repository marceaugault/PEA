using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
	[SerializeField]
	float bulletLifetime = 3f;

	[SerializeField]
	GameObject bulletPrefab = null;

	public int poolSize = 30;

	BulletController[] bulletPool;

    // Start is called before the first frame update
    void Start()
    {
		bulletPool = new BulletController[poolSize];
        for (int i = 0; i < poolSize; i++)
		{
			var go = Instantiate(bulletPrefab, new Vector3(0f, 100f, 0f), Quaternion.identity);
			bulletPool[i] = go.GetComponent<BulletController>();
		}
    }

	// Update is called once per frame
	void Update()
	{
		for (int i = 0; i < poolSize; i++)
		{
			if (bulletPool[i].currentLifetime >= bulletLifetime)
				bulletPool[i].Deactivate();
		}
	}

	BulletController GetAvailableBullet()
	{
		for (int i = 0; i < poolSize; i++)
		{
			if (!bulletPool[i].isActive)
			{
				return bulletPool[i];
			}
		}

		return null;
	}

	public void FireBullet(Vector3 position, Vector3 dir)
	{
		dir.y = 0f;
		
		BulletController bullet = GetAvailableBullet();

		if (bullet)
		{
			bullet.Activate(position, dir);
		}
	}
}
