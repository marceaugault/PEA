using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchFlickerLight : MonoBehaviour
{
    Light Light;

    public float VariationIntensity = 1f;
    public float VariationSpeed = 1f;

    float Intensity;

    // Start is called before the first frame update
    void Start()
    {
        Light = GetComponentInChildren<Light>();

        Intensity = Light.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        Light.intensity = Intensity + Mathf.Sin(Time.time * VariationSpeed) * VariationIntensity;
    }
}
