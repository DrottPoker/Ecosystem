using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCreatures : MonoBehaviour
{
    public GameObject planet;

    public List<GameObject> creatures = new List<GameObject>();
    public List<int> numberOfCreatures = new List<int>();

    public Dictionary<GameObject, int> creatureDic = new Dictionary<GameObject, int>();

    //public int[] totalCreatureSpawn;


    // Start is called before the first frame update
    void Start()
    {
        //AddCreatures(fox, numberOffoxes, creature);

        SpawnCreature();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    public void SpawnCreature()
    {
        //get the size of the planet
        float planetWidth = planet.transform.localScale.x / 2;
        float planetHeight = planet.transform.localScale.y / 2;
        //find random spawnpoint on planet
        



        // pair every creature with the amount we want
        for (int i = 0; i < creatures.Count; i++)
        {
            creatureDic.Add(creatures[i], numberOfCreatures[i]);


        }

        //loop through every creature and spawn the associated amount
        foreach (KeyValuePair<GameObject, int> data in creatureDic)
        {
            for (int i = 0; i < data.Value; i++)
            {
                Vector2 randomSpawnposition = new Vector2(Random.Range(planetWidth, -planetWidth), Random.Range(planetHeight, -planetHeight));
                //Make it round
                Vector2 SpawnPoint = Random.insideUnitCircle * randomSpawnposition;

                Instantiate(data.Key, SpawnPoint, Quaternion.identity);
            }
        }

    } 

}
