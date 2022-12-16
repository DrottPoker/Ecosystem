using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureBehavior : MonoBehaviour
{
    public CreatureStats CreatureStats;
    public CreatureManager CreatureManager;

    public GameObject foodOnDeath;
    
    public GameObject currentTarget;

    public GameObject closestUnoccupiedFood;
    float distanceToCurrentUnoccupaiedFood = Mathf.Infinity;
    float distanceToClosestPrey = Mathf.Infinity;
    float distanceToClosestPreditor = Mathf.Infinity;

    //public List<Collider2D> allColldersList = new List<Collider2D>();
    public Collider2D[] allColliders;
    //public GameObject[] allCreatures;

    public GameObject closestPrey;
    public GameObject closestPreditor;

    public bool eating;
    public bool hungry;
    public bool fleeing;
    public bool hunting;

    //public bool isSearchingForFood;
    public GameObject newChildObject;

    //public float lookDirection;
    public Vector2 lookDirection;

    private void Awake()
    {
        CreatureStats = this.GetComponent<CreatureStats>();
        CreatureManager = this.GetComponent<CreatureManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
        ConstantMovingForward();
        RotateCreature();

        FindClosestColliderInRadius();

        Task();

        if (CreatureStats.currentBirthcooldown > 0)
        {
            CreatureStats.currentBirthcooldown -= 1 * Time.deltaTime;

        }
    }

    public void Feelings()
    {
        //want to eat becouse its hungry
        
    }

    public void RandomWandering()
    { 
        CreatureStats.changeDirectionTime -= 1 * Time.deltaTime;

        if (CreatureStats.changeDirectionTime <= 0)
        {
            float xAngle = Random.Range(-1f, 1f);
            float yAngle = Random.Range(-1f, 1f);

            lookDirection.x = xAngle;
            lookDirection.y = yAngle;

            //Debug.Log("bytt direction");
            CreatureStats.changeDirectionTime = CreatureStats.timeToChangeDirectionWhileWandering;
            return;
        } 
    }

    public void ConstantMovingForward()
    {
        //transform.up == (0,1,0)
        this.GetComponent<Rigidbody2D>().velocity = transform.up * CreatureStats.currentMovementSpeed * Time.fixedDeltaTime * 10f;
    }
    public void RotateCreature()
    {
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation; //Quaternion.Lerp(transform.rotation, rotation, CreatureStats.rotationSpeed * Time.deltaTime).normalized;

        
    }

    public void Task()
    {
        if(CreatureManager.isPreditor == false)
        {
            if (closestPreditor != null && Vector2.Distance(this.transform.position, closestPreditor.transform.position) <= CreatureStats.lookRadius)
            {
                fleeing = true;
                Flee();
            }
            else
            {
                fleeing = false;
            }
        }else if(CreatureManager.isPreditor == true)
        {
            if (closestPrey != null && Vector2.Distance(this.transform.position, closestPrey.transform.position) <= CreatureStats.lookRadius)
            {
                           
                if (closestUnoccupiedFood != null && Vector2.Distance(this.transform.position, closestUnoccupiedFood.transform.position) <= CreatureStats.lookRadius && hungry == true)
                {
                    hunting = false;
                    currentTarget = closestUnoccupiedFood;
                    GoTowardsFoodInRadius();
                }
                else if(( closestUnoccupiedFood == null || Vector2.Distance(this.transform.position, closestUnoccupiedFood.transform.position) > CreatureStats.lookRadius ) && hungry == true)
                {
                    hunting = true;
                    currentTarget = closestPrey;
                    ChasePreyInRadius();
                }
                else
                {
                    hunting = false;
                    currentTarget = null;
                    RandomWandering();
                }
                //hunting = true;
                //Hunt();
            }
            else
            {

            }
        }
        
        else
        {
            fleeing = false;
        }
        if (CreatureStats.currentEnergy < CreatureStats.maxEnergy * 0.5f)
        {
            hungry = true;
            
        }
        else if (CreatureStats.currentEnergy >= CreatureStats.maxEnergy)
        {
            hungry = false;
        }
  
        if (CreatureStats.currentEnergy > CreatureStats.maxEnergy * 0.8 && CreatureManager.mature == true && CreatureStats.currentBirthcooldown <= 0f && fleeing == false)
        {
            GiveBirth();
            //return;
        }
        else
        {
            //fleeing = false;
            RandomWandering();
        }
        
    }

    public void GiveBirth()
    {

        //reset and add some stats on child
        newChildObject = Instantiate(this.gameObject);
        newChildObject.GetComponent<CreatureStats>().generation += 1;
        newChildObject.GetComponent<CreatureStats>().currentEnergy = newChildObject.GetComponent<CreatureStats>().currentEnergy * 0.8f;
        newChildObject.GetComponent<CreatureStats>().currentBirthcooldown = newChildObject.GetComponent<CreatureStats>().maxBirthCooldown;
        newChildObject.GetComponent<CreatureStats>().children = 0;
        newChildObject.GetComponent<CreatureStats>().currentAge = 0;
        newChildObject.GetComponent<CreatureManager>().mature = false;
        newChildObject.GetComponent<CreatureStats>().currentMovementSpeed = newChildObject.GetComponent<CreatureStats>().maxMovementSpeed;



        for (int i = 0; i < 5; i++)
        {

            //mutate child Stats
            float MutateAmount = Random.Range(-CreatureStats.maxMutateAmount, CreatureStats.maxMutateAmount);
            float mutatePercentAmount = (MutateAmount / 100) + 1;
            if (i == 0)
            {
                newChildObject.GetComponent<CreatureStats>().maxHealth *= mutatePercentAmount;
                continue;
            }
            if (i == 1)
            {
                newChildObject.GetComponent<CreatureStats>().maxEnergy *= mutatePercentAmount;
                continue;
            }
            if (i == 2)
            {
                newChildObject.GetComponent<CreatureStats>().maxMovementSpeed *= mutatePercentAmount;
                continue;
            }
            if (i == 3)
            {
                newChildObject.GetComponent<CreatureStats>().rotationSpeed *= mutatePercentAmount;
                continue;
            }
            if (i == 4)
            {
                newChildObject.GetComponent<CreatureStats>().lookRadius *= mutatePercentAmount;
                continue;
            }
            
        }
        newChildObject = null;

        //apply birth energy cost
        CreatureStats.currentEnergy -= CreatureStats.birthEnergyCost;
        CreatureStats.currentBirthcooldown = CreatureStats.maxBirthCooldown;
        //+1 on children
        CreatureStats.children++;
        
    }
      

    //put all colliders in radius in an array and select the closest colider
    private void FindClosestColliderInRadius()
    {
        //float distanceToClosestThing = Mathf.Infinity;
        allColliders = Physics2D.OverlapCircleAll(this.transform.position, CreatureStats.lookRadius);
        //allColldersList.AddRange(allColliders);
        

        if (allColliders != null)
        {
            
            foreach (Collider2D currentCollider in allColliders)
            {


                if (currentCollider.gameObject.tag == "Food")
                {
                    float distanceToFood = Vector2.Distance(this.transform.position, currentCollider.transform.position);

                    //find closest plant
                    if (CreatureManager.isPreditor == false && currentCollider.gameObject.GetComponent<Food>().foodType == "Plant")
                    {
       
                        if (closestUnoccupiedFood != null)
                        {
                            distanceToCurrentUnoccupaiedFood = Vector2.Distance(this.transform.position, closestUnoccupiedFood.transform.position);

                            if(closestUnoccupiedFood.GetComponent<Food>().Occupier == null || closestUnoccupiedFood.gameObject.GetComponent<Food>().Occupier == this.gameObject)
                            {
                                if (currentCollider.gameObject.GetComponent<Food>().Occupier == null || currentCollider.gameObject.GetComponent<Food>().Occupier == this.gameObject)
                                {
                                    if (distanceToFood < distanceToCurrentUnoccupaiedFood && currentCollider.gameObject.GetComponent<Food>().Occupier != this.gameObject)
                                    {
                                        distanceToCurrentUnoccupaiedFood = distanceToFood;
                                        closestUnoccupiedFood = currentCollider.gameObject;
                                        continue;
                                    }
                                    else
                                    {
                                        //Debug.Log("misan");
                                        //distanceToCurrentUnoccupaiedFood = distanceToFood;
                                        //closestUnoccupiedFood = currentCollider.gameObject;
                                        continue;
                                    }

                                }
                                else
                                {
                                    //distanceToCurrentUnoccupaiedFood = distanceToFood;
                                    //closestUnoccupiedFood = currentCollider.gameObject;
                                    continue;
                                }
                            }
                            else
                            {
                                closestUnoccupiedFood = null;
                                //distanceToCurrentUnoccupaiedFood = distanceToFood;
                                continue;
                            }
                            

                        }
                        else
                        {
                            distanceToCurrentUnoccupaiedFood = distanceToFood;
                            closestUnoccupiedFood = currentCollider.gameObject;
                            continue;
                        }
                                           

                    }
                    //find closest meat
                    if (CreatureManager.isPreditor == true && currentCollider.gameObject.GetComponent<Food>().foodType == "Meat")
                    {
                        if (closestUnoccupiedFood != null)
                        {
                            distanceToCurrentUnoccupaiedFood = Vector2.Distance(this.transform.position, closestUnoccupiedFood.transform.position);

                        }

                        if (distanceToFood < distanceToCurrentUnoccupaiedFood && (currentCollider.gameObject.GetComponent<Food>().Occupier == null || currentCollider.gameObject.GetComponent<Food>().Occupier == this.gameObject))
                        {
                            distanceToCurrentUnoccupaiedFood = distanceToFood;
                            closestUnoccupiedFood = currentCollider.gameObject;
                            //Debug.Log("hej");
                            continue;
                        }
                        else
                        {
                            //closestUnoccupiedFood = null;
                        }
                    }
                }
                else if(currentCollider.gameObject.tag == "Creature")
                {
                  
                    

                 
                    //Find closest Prey
                    if (currentCollider.gameObject != this.gameObject && currentCollider.gameObject.GetComponent<CreatureManager>().isPreditor == false)
                    {
                       
                        if (closestPrey == null)
                        {
                            closestPrey = currentCollider.gameObject;
                            continue;
                        }

                        
                        float distanceToPrey = (currentCollider.gameObject.transform.position - this.transform.position).sqrMagnitude;

                        if(distanceToPrey < distanceToClosestPrey)
                        {
                            distanceToClosestPrey = distanceToPrey;
                            closestPrey = currentCollider.gameObject;
                            continue;
                        }
                        
                    }
                    //Find closest Preditor
                    if (currentCollider.gameObject != this.gameObject && currentCollider.gameObject.GetComponent<CreatureManager>().isPreditor == true)
                    {
                       
                        if (closestPreditor == null)
                        {
                            closestPreditor = currentCollider.gameObject;
                            continue;
                        }

                        
                        float distanceToPreditor = (currentCollider.gameObject.transform.position - this.transform.position).sqrMagnitude;
                        if (distanceToPreditor < distanceToClosestPreditor)
                        {
                            distanceToClosestPreditor = distanceToPreditor;
                            closestPreditor = currentCollider.gameObject;
                            continue;
                        }
                    }
                }
               
            }
            
        }
       
        
        if (closestUnoccupiedFood != null && hungry == true && fleeing == false)
        {
            //currentTarget = closestUnoccupiedFood;
            GoTowardsFoodInRadius();

        }
        else
        {
           
        }
    }
    
    //going towards the food
    public void GoTowardsFoodInRadius()
    {
        if (closestUnoccupiedFood != null)
        {
            //change look direction to closest food
            lookDirection = closestUnoccupiedFood.transform.position - this.transform.position;
            Debug.DrawLine(this.transform.position, closestUnoccupiedFood.transform.position);

        }
        else
        {
            
        }
        
    }

    public void FindClosestPreyAndPreditor()
    {
        /*
        float distanceToClosestPrey = Mathf.Infinity;
        float distanceToClosestPreditor = Mathf.Infinity;
        allCreatures = Physics2D.OverlapCircleAll(this.transform.position, CreatureStats.lookRadius); ;
        

        foreach (Collider currentCreature in allCreatures)
        {
            //Find closest Prey
            if(currentCreature != this.gameObject && currentCreature.GetComponent<CreatureManager>().isPreditor == false)
            {
                float distanceToPrey = (currentCreature.transform.position - this.transform.position).sqrMagnitude;
                if (distanceToPrey < distanceToClosestPrey)
                {
                    distanceToClosestPrey = distanceToPrey;
                    closestPrey = currentCreature;
                    continue;
                }
            }
            //Find closest Preditor
            if (currentCreature != this.gameObject && currentCreature.GetComponent<CreatureManager>().isPreditor == true)
            {
                float distanceToPreditor = (currentCreature.transform.position - this.transform.position).sqrMagnitude;
                if (distanceToPreditor < distanceToClosestPreditor)
                {
                    distanceToClosestPreditor = distanceToPreditor;
                    closestPreditor = currentCreature;
                    continue;
                }
            }
        }
        if(fleeing == true)
        {
            Flee();
        }

        //Debug.DrawLine(this.transform.position, closestPrey.transform.position, Color.red);
        //Debug.DrawLine(this.transform.position, closestPreditor.transform.position,Color.green);
        */

    }
    public void Flee()
    {
        hungry = false;

        Vector2 fleeDirection = new Vector2();
        fleeDirection = closestPreditor.transform.position - this.transform.position;
        lookDirection = -fleeDirection;
        
        //lookDirection.Normalize();

    }
    public void ChasePreyInRadius()
    {
        if (Vector2.Distance(this.transform.position, closestPrey.transform.position) <= CreatureStats.lookRadius)
        {
            Vector2 HuntDirection = new Vector2();
            HuntDirection = closestPrey.transform.position - this.transform.position;
            lookDirection = HuntDirection;
            Debug.DrawLine(this.transform.position, closestPrey.transform.position);
        }
        
    }

    public void Die()
    {
        Instantiate(foodOnDeath, this.transform.position, Quaternion.identity);
        //Debug.Log(CreatureStats.name + " has died");
        Destroy(this.gameObject);
    }
    public void Eat(GameObject food)
    {
        CreatureStats.eatTimer -= 1 * Time.deltaTime;
        //eating = true;

        if (CreatureStats.eatTimer > 0)
        {
            //this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            
        }else if(CreatureStats.eatTimer <= 0)
        {
            CreatureStats.currentEnergy += food.GetComponent<Food>().foodValue;
            eating = false;
            CreatureStats.eatTimer = CreatureStats.maxEatingTime;
            //Debug.Log(CreatureStats.name + " has eaten " + food.name);
            Destroy(food);
            closestUnoccupiedFood = null;
        }
        
    }
    public void Bite(GameObject creature)
    {
        creature.GetComponent<CreatureStats>().currentHealth -= 100f;
        this.CreatureStats.currentEnergy += CreatureStats.biteEnergyCost;
    }

    //when it touches something
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Food>() != null)
        {
            if (this.CreatureManager.isPreditor == false && collision.gameObject.GetComponent<Food>().foodType == "Plant" && hungry == true)
            {
                eating = true;
                Eat(collision.gameObject);
                return;
            }
            else if (this.CreatureManager.isPreditor == true && collision.gameObject.GetComponent<Food>().foodType == "Meat" && hungry == true)
            {
                eating = true;
                Eat(collision.gameObject);
                return;
            }
            else
            {
                return;
                // Debug.Log(CreatureStats.name + " hit " + collision.gameObject.name);
            }

        }

        
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        eating = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject != null)
        {
            if (collision.gameObject.tag == "Creature" && collision.gameObject.GetComponent<CreatureStats>().creatureName != this.CreatureStats.creatureName && this.CreatureManager.isPreditor == true)
            {
                Bite(collision.gameObject);
                return;
            }
        }

    }

    //Debugging lookradius
    private void OnDrawGizmosSelected()
    {
        if (CreatureStats != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, CreatureStats.lookRadius);
        }     
    }
}
