using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    private float currentTime = 0f;
    [SerializeField] float spawnTime = 4f;

    private GameObject[] spawnTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        spawnTransform = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime > spawnTime) {

            int randomIndex = Random.Range(0, spawnTransform.Length - 1);
            GameObject instantiatedEnemy = Instantiate(enemyPrefab, spawnTransform[randomIndex].transform.position, Quaternion.identity);
            instantiatedEnemy.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            
            
            
            currentTime = 0f;
        }
        else
        {
            currentTime += Time.deltaTime;
        }



    }
}
