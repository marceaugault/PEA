using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float Life = 50f;

    [SerializeField] float Speed = 10f;
    
    [SerializeField] float MeleeDamage = 10f;

    List<Vector3> Waypoints;
    int NextWaypoint;

    public delegate void OnEnemyKilled();
    public OnEnemyKilled EnemyKilledDelegate;

    // Start is called before the first frame update
    void Start()
    {
        Waypoints = new List<Vector3>();
        NextWaypoint = 0;

        for (int i = 0; i < 5; i++)
        {
            Waypoints.Add(new Vector3(Random.Range(-24f, 24f), transform.position.y, Random.Range(-14f, 14f)));
        }
    }

    // Update is called once per frame
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

            EnemyKilledDelegate();
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
