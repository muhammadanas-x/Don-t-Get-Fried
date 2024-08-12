using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggMovement : MonoBehaviour
{

    public float angle = 45f;
    private float velocity;
    private Vector3 direction;
    private ExplosionSelector explosionSelector;
    // Start is called before the first frame update
    void Start()
    {
        SlingShot slingShot = GameObject.Find("ChickenMain").GetComponent<SlingShot>();
        explosionSelector = GameObject.Find("ExplosionSelector").GetComponent<ExplosionSelector>();
        velocity = slingShot.GetCurrentVelocity();
        direction = slingShot.GetDirection();

        Debug.Log($"Velocity: {velocity}");
        Debug.Log($"Direction: {direction}");

        StartCoroutine(LaunchProjectile());
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 0) Destroy(gameObject);
    }


    
    IEnumerator LaunchProjectile()
    {
        float t = 0f;
        float angleRad = angle * Mathf.Deg2Rad;
        Vector3 initialPosition = transform.position;


        while (t < 5f)
        {

            float z = velocity * Mathf.Cos(angleRad) * t;
            float y = velocity * Mathf.Sin(angleRad) * t - (0.5f) * 9.81f * Mathf.Pow(t, 2);

            // Calculate the new position relative to the initial position
            Vector3 displacement = new Vector3(0, y, z);   
            Vector3 finalDir = initialPosition + direction * displacement.z + transform.up * displacement.y;
            transform.position = finalDir;

            Quaternion targetRotation = Quaternion.LookRotation(transform.forward) ;
            transform.rotation = targetRotation;

            t += Time.deltaTime;
            yield return null;
        }
    }



    public void Shoot(float vi , Vector3 direction)
    {
        StartCoroutine(LaunchProjectile());
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {

            float radius = 5.0f; // Adjust the radius as needed

            Vector3 collisionPoint = other.ClosestPoint(transform.position);

            Collider[] colliders = Physics.OverlapSphere(collisionPoint, radius);


            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.CompareTag("Enemy"))
                {
                    EnemyMovement enemy = collider.GetComponent<EnemyMovement>();
                    if (gameObject.CompareTag("Explosion"))
                    {
                        enemy.TakeExplosionDamage(25);
                    }
                    else if (gameObject.CompareTag("Acid"))
                    {
                        enemy.TakeAcidDamage(5, 5, 5);
                    }
                    else
                    {
                        enemy.TakeFreezeDamage(5);
                    }
                }
            }
            GameObject particlePrefab = Instantiate(explosionSelector.currentExplosionPrefab, collisionPoint, Quaternion.identity);
            ParticleSystem[] explosionParticle = particlePrefab.GetComponentsInChildren<ParticleSystem>();



            foreach (ParticleSystem particle in explosionParticle)
            {
                particle.Play();
            }


        }
    }

}
