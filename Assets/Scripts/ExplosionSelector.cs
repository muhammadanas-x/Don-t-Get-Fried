using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSelector : MonoBehaviour
{

    [SerializeField] GameObject fireExplosion;
    [SerializeField] GameObject fireEgg;
    public GameObject currentExplosionPrefab;
    public GameObject currentExplosionEgg;

    private void Start()
    {
        currentExplosionPrefab = fireExplosion;
        currentExplosionEgg = fireEgg;
    }

    public void setExplosion(GameObject pls)
    {
        currentExplosionPrefab = pls;

    }


    public  GameObject GetSelectedPrefab(GameObject pls)
    {
        return currentExplosionPrefab = pls;
    }

    public GameObject GetSelectedEgg()
    {
        return currentExplosionEgg;
    }

    public void setSelectedEgg(GameObject pls)
    {
        currentExplosionEgg = pls;
    }

}
