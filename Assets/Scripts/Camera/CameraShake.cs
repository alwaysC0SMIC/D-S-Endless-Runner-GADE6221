using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//REF: https://youtu.be/7MrYSiyRKiQ?si=MW4lQmM0d09rMApN

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.2F;
    public float shakeIntensity = 0.1F;

    private Vector3 initialPosition;
    private float currentShakeDuration = 0F;

    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        initialPosition = transform.localPosition;
        Invoke("disableAnimate", 5F);
    }

    void Update()
    {
        if (currentShakeDuration > 0)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
            transform.localPosition = initialPosition + randomOffset;

            currentShakeDuration -= Time.deltaTime;
        }
        else { 
            transform.localPosition = initialPosition;
        }
    }

    //TRIGGERS CAMERA SHAKE
    public void Shake() {
        currentShakeDuration = shakeDuration;
    }

    //DISABLE ANIMATOR TO ALLOW SHAKE
    private void disableAnimate() {
        animator.enabled = false;
    }
}
