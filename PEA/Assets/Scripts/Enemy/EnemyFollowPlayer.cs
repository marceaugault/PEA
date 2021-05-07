using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    [SerializeField] float BaseLife = 50f;

    [SerializeField] float BaseSpeed = 10f;

    [SerializeField] float BaseMeleeDamage = 10f;

    float Life = 1f;
    float Speed = 1f;
    float MeleeDamage = 1f;

    PlayerController Player;

    public delegate void OnEnemyKilled(Vector3 pos, LootTable table);
    public OnEnemyKilled EnemyKilledDelegate;

    [SerializeField] LootTable LootTable;

    void Start()
    {
        Player = FindObjectOfType<PlayerController>();

        LootTable.Init();
    }

    private void FixedUpdate()
    {
        Vector3 dir = Player.transform.position - transform.position;
        dir.y = 0f;

        transform.position += dir.normalized * Speed * Time.fixedDeltaTime;
    }

    public void Init(int difficulty)
    {
        Life = BaseLife + Mathf.Pow(difficulty, 1.2f);
        Speed = BaseSpeed + Mathf.Pow(difficulty, 0.5f);
        MeleeDamage = BaseMeleeDamage + Mathf.Pow(difficulty, 0.6f);
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
