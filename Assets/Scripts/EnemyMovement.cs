using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float enemySpeed = 2f;
    [SerializeField] private Transform target;
    [SerializeField] Material freezeMaterial;
    [SerializeField] private GameObject score;



    private Material[] originalMaterials;
    

    public int health = 100;

    private bool isFrozen = false;

    private void Start()
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            // Store the original material of each renderer
            originalMaterials[i] = renderers[i].material;
        }

    }

    void Update()
    {
        if (!isFrozen)
        {
            transform.position += transform.forward * Time.deltaTime;
        }

   


    }

    public void TakeExplosionDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Destroy(gameObject);
    }

    public void TakeAcidDamage(int damage, int ticks, float interval)
    {
        StartCoroutine(AcidDamageOverTime(damage, ticks, interval));
    }

    public void TakeFreezeDamage(float freezeDuration)
    {
        StartCoroutine(FreezeEnemy(freezeDuration));
    }

    private IEnumerator AcidDamageOverTime(int damage, int ticks, float interval)
    {
        for (int i = 0; i < ticks; i++)
        {
            health -= damage;
            if (health <= 0)
            {
                Destroy(gameObject);
                
                yield break;
            }
            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator FreezeEnemy(float freezeDuration)
    {
        isFrozen = true;
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

        foreach(Renderer render in renderers)
        {
            render.material = freezeMaterial;
        }
        yield return new WaitForSeconds(freezeDuration);

        for (int j = 0; j < renderers.Length; j++)
        {
            renderers[j].material = originalMaterials[j];
        }

        isFrozen = false;
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Farm"))
        {
            FarmArea farm = other.GetComponent<FarmArea>();
            farm.TakeDamage(5);
            Destroy(gameObject);

        }
    }
}