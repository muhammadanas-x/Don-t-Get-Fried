using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShot : MonoBehaviour
{

    [SerializeField] private float angle;
    [SerializeField] private float maxVelocity;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float step;

    [SerializeField] GameObject impactPoint;
    [SerializeField] GameObject fireEgg;
    

    public FixedJoystick joystick; // Reference to the joysti
    float lastVelocity;
    private float velocity;
    private ExplosionSelector explosionSelector;

    private Vector3 mainDirection;
    private Vector3 startPosition;
    private bool isBeingShoot = false;



    private List<Collider> colliders = new List<Collider>();

    private void Start()
    {
        startPosition = transform.position;
        explosionSelector = GameObject.Find("ExplosionSelector").GetComponent<ExplosionSelector>();
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        // Get the direction from the joystick
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector2 velocityMagnitude = new Vector2(horizontal, vertical);

        if (horizontal != 0 || vertical != 0 && !isBeingShoot)
        {
            float angle = Mathf.Atan2(-horizontal, -vertical) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        // Handle touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {
                lineRenderer.enabled = true;
                DrawPath();
                velocity = velocityMagnitude.magnitude * maxVelocity;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                lineRenderer.enabled = false;
                mainDirection = transform.forward;

                Instantiate(explosionSelector.currentExplosionEgg, startPosition, Quaternion.identity);
                isBeingShoot = true;
            } 
        }

        lastVelocity = velocity;
    }


    void DrawPath()
    {
        float totalTime = 2;
        step = Mathf.Max(0.01f, step);
        lineRenderer.positionCount = (int)(totalTime / step) + 1; // Updated to +1 instead of +2
        float angleRad = angle * Mathf.Deg2Rad;
        int count = 0;

        Vector3 initialPosition = transform.position;
        Vector3 previousPosition = initialPosition;

        for (float i = 0; i <= totalTime; i += step) // Use <= to include the endpoint
        {
            float x = lastVelocity * Mathf.Cos(angleRad) * i;
            float y = lastVelocity * Mathf.Sin(angleRad) * i - (0.5f * 9.81f * Mathf.Pow(i, 2));

            Vector3 displacement = new Vector3(0f, y, x);
            Vector3 currentPosition = initialPosition + transform.forward * displacement.z + transform.up * displacement.y;

            lineRenderer.SetPosition(count, currentPosition);

            // Perform raycast to detect collision with Ground
            if (count > 0)
            {
                if (Physics.Raycast(previousPosition, currentPosition - previousPosition, out RaycastHit hit, Vector3.Distance(previousPosition, currentPosition)))
                {
                    if (hit.collider.CompareTag("Ground"))
                    {
                        impactPoint.transform.position = hit.point;
                        impactPoint.SetActive(true);
                        lineRenderer.positionCount = count + 1;
                        lineRenderer.SetPosition(count, hit.point);
                        return;
                    }
                }
            }

            previousPosition = currentPosition;
            count++;
        }
    }

    public float GetCurrentVelocity()
    {
        return velocity;
    }

    public Vector3 GetDirection()
    {
        return mainDirection;
    }
   

}
