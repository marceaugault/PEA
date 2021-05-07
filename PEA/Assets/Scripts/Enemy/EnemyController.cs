using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float BaseLife = 50f;

    [SerializeField] float BaseSpeed = 10f;
    
    [SerializeField] float BaseBulletDamage = 20f;
    [SerializeField] float BaseMeleeDamage = 10f;
    [SerializeField] float BulletLifetime = 5f;

    [SerializeField] float fireRate = 2f;
    float NextTimeToShoot;

    float Life = 1f;
    float Speed = 1f;
    float BulletDamage = 1f;
    float MeleeDamage = 1f;

    List<Vector3> Waypoints;
    int NextWaypoint;

    PlayerController Player;
    EnemyBulletPool BulletPool;

    public delegate void OnEnemyKilled(Vector3 pos, LootTable table);
    public OnEnemyKilled EnemyKilledDelegate;

    [SerializeField] LootTable LootTable;

    // Start is called before the first frame update
    void Start()
    {
        Waypoints = new List<Vector3>();
        NextWaypoint = 0;

        NextTimeToShoot = Time.time + fireRate;
        BulletPool = FindObjectOfType<EnemyBulletPool>();

        Player = FindObjectOfType<PlayerController>();

        for (int i = 0; i < 5; i++)
        {
            Waypoints.Add(new Vector3(Random.Range(-24f, 24f), transform.position.y, Random.Range(-14f, 14f)));
        }

        LootTable.Init();
    }

	private void Update()
	{
	    if (Time.time >= NextTimeToShoot)
		{
            NextTimeToShoot = Time.time + fireRate;

            Vector3 dir = Player.transform.position - transform.position;
            dir.y = 0f;
            BulletPool.FireBullet(transform.position, dir.normalized, BulletDamage, BulletLifetime);
        }
	}
	public void Init(int difficulty)
	{
        Life = BaseLife + Mathf.Pow(difficulty, 1.2f);
        Speed = BaseSpeed + Mathf.Pow(difficulty, 0.5f);
        BulletDamage = BaseBulletDamage + Mathf.Pow(difficulty, 0.8f);
        MeleeDamage = BaseMeleeDamage + Mathf.Pow(difficulty, 0.6f);
    }
    void FixedUpdate()
    {
        MoveToWaypoint(Waypoints[NextWaypoint]);
    }

    void MoveToWaypoint(Vector3 point)
    {
        transform.position += (point - transform.position).normalized * Speed * Time.fixedDeltaTime;

        if ((point - transform.position).sqrMagnitude < 1f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if (NextWaypoint < Waypoints.Count - 1)
        {
            NextWaypoint++;
        }
        else
        {
            NextWaypoint = 0;
        }
    }

    public void TakeDamage(float Damage)
    {
        Life -= Damage;

        if (Life <= 0f)
        {
            Debug.Log("Enemy Killed");

            EnemyKilledDelegate(transform.position, LootTable);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & LayerMask.GetMask("Player")) != 0)
        {
            other.GetComponent<PlayerController>()?.TakeDamage(MeleeDamage);
        }
    }
}
