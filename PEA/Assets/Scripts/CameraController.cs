using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset;
    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        transform.position = Vector3.Lerp(transform.position, Target.position + Offset, Speed * Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
    }
}
