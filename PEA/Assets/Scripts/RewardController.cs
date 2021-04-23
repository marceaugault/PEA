using UnityEngine;

public class RewardController : MonoBehaviour
{
    public float Money { get; set; }

    private void Update()
    {
        if (transform.position.y <= 1.3f)
        {
            transform.position = new Vector3(transform.position.x, 1.3f, transform.position.z);
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & LayerMask.GetMask("Player")) != 0)
        {

        }
    }
}
