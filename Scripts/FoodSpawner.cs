using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodToSpawn;

    public float spawnRate;

    public GameObject[] foodOnPlanet;

    public Vector2 SpawnRadius;

    public int maxFood;
    public int currentFood;
    public int startingFood;
    // Start is called before the first frame update
    void Start()
    {
        //spawn starting food
        for (int i = 0; i < startingFood; i++)
        {
            SpawnFood();
        }

        //food spawn rate on the planet
        InvokeRepeating("SpawnFood", 0f, spawnRate);
    }

    // Update is called once per frame
    void Update()
    {
        IdentifyAllFood();
        
    }

    public void SpawnFood()
    {
        //get the size of the planet

        float planetWidth = this.transform.localScale.x / 2;
        float planetHeight = this.transform.localScale.y / 2;




        //find random spawnpoint on planet
        Vector2 randomSpawnPosition = new Vector2(Random.Range(planetWidth, -planetWidth), Random.Range(planetHeight, -planetHeight));
        //Make it round
        SpawnRadius = Random.insideUnitCircle.normalized * randomSpawnPosition;
        
        if(foodOnPlanet.Length < maxFood)
        {
            GameObject food = Instantiate(foodToSpawn, SpawnRadius, Quaternion.identity);
            food.transform.parent = this.transform;
            
        }
        
    }

    //debug spawn radius
    private void OnDrawGizmosSelected()
    {
        if (this != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, transform.localScale.x / 2);
        }
    }

    public void IdentifyAllFood()
    {
        foodOnPlanet = GameObject.FindGameObjectsWithTag("Food");
        currentFood = foodOnPlanet.Length;
    }
}
