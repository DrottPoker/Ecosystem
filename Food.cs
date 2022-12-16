using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{

    public float foodValue;
    public string foodType;
    public float lifeTime;
    public float currentAge;

    public GameObject Occupier = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AgeingFood();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Creature" && collision.gameObject.GetComponent<CreatureBehavior>().hungry == true)
        {
            
            Occupier = collision.gameObject;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Occupier = null;
    }

    private void AgeingFood()
    {
        currentAge += 1 * Time.deltaTime;
        if(currentAge >= lifeTime)
        {
            Destroy(this.gameObject);
        }
    }

}
