using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureStats : MonoBehaviour
{

    public string creatureName;
    public int generation;
    public float maxHealth;
    public float currentHealth;
    public float maxEnergy;
    public float currentEnergy;
    public float maxMovementSpeed;
    public float currentMovementSpeed;
    public float rotationSpeed;
    public float defenseValue;
    public float maxEatingTime;
    public float eatTimer;
    public float lookRadius;
    public float biteEnergyCost;
    public float currentAge;
    public float matureAge;
    public float maxMutateAmount;
    public float maxTimeToGiveBirth;
    public int children;
    public float birthEnergyCost;
    public float maxBirthCooldown;
    public float currentBirthcooldown;
    public float timeToChangeDirectionWhileWandering;
    public float changeDirectionTime;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
