using UnityEngine;
public enum RewardType
{
    Money1x = 0,
    Money5x,
    Money10x,
    Money50x,
    None,
}
public class RewardController : MonoBehaviour
{
    public RewardType Type { get; set; }
    public int Money { get; set; }
    public bool IsActive{ get; private set; }

	new Rigidbody rigidbody;
	private void Update()
    {
        if (transform.position.y <= 1.3f)
        {
            transform.position = new Vector3(transform.position.x, 1.3f, transform.position.z);
            rigidbody.isKinematic = true;
        }
    }

	public void Init()
	{
        rigidbody = GetComponent<Rigidbody>();

        IsActive = false;
    }

	public void Enable(bool enable)
    {
        gameObject.SetActive(enable);
        IsActive = enable;
    }

    void SetReward(RewardType type)
	{
        Type = type;

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        switch (Type)
		{
			case RewardType.Money1x:
				sprite.sprite = Resources.Load<Sprite>("Sprites/money-reward-icon");
                break;
            case RewardType.Money5x:
				sprite.sprite = Resources.Load<Sprite>("Sprites/money5x-reward-icon");
				break;
			case RewardType.Money10x:
                sprite.sprite = Resources.Load<Sprite>("Sprites/money10x-reward-icon");
                break;
            case RewardType.Money50x:
				sprite.sprite = Resources.Load<Sprite>("Sprites/money50x-reward-icon");
				break;
		}
	}

    public void SpawnReward(RewardType type, Vector3 pos, float spawnForce)
	{
        Enable(true);

        SetReward(type);

        transform.position = pos;

        rigidbody.isKinematic = false;
        rigidbody.velocity *= 0f;
        rigidbody.AddForce(Vector3.up * spawnForce);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & LayerMask.GetMask("Player")) != 0)
        {
            GameController gc = FindObjectOfType<GameController>();
            gc.AddMoney(Money);

            Enable(false);
        }
    }
}
