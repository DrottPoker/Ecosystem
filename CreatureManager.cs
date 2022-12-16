using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    public CreatureStats CreatureStats;
    public CreatureBehavior CreatureBehavior;

    public float energyDrain;
    public float movementEnergyDrain;

    public bool mature;
    public bool isPreditor;

    // Start is called before the first frame update

    private void Awake()
    {
        CreatureStats = this.GetComponent<CreatureStats>();
        CreatureBehavior = this.GetComponent<CreatureBehavior>();
    }
    void Start()
    {
        

        CreatureStats.currentMovementSpeed = CreatureStats.maxMovementSpeed;

        CreatureStats.currentEnergy = CreatureStats.maxEnergy * 0.5f;

        CreatureStats.currentHealth = CreatureStats.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //apply Aging;
        Ageing();
        //while eating
        WhileEating();
        //apply energydrain
        EnergyDrain();
        
    }
    public void EnergyDrain()
    {
        //calculate energi drain
        movementEnergyDrain = CreatureStats.currentMovementSpeed * 0.05f;
        energyDrain = 1 + movementEnergyDrain;
        //apply energy drain
        CreatureStats.currentEnergy -= energyDrain * Time.deltaTime;

        //if energy is over max make it max
        if (CreatureStats.currentEnergy > CreatureStats.maxEnergy)
        {
            CreatureStats.currentEnergy = CreatureStats.maxEnergy;
        }
        if (CreatureStats.currentEnergy <= 0 || CreatureStats.currentHealth <= 0)
        {
            CreatureBehavior.Die();

        }

    }
    public void Ageing()
    {
        CreatureStats.currentAge += 1 * Time.deltaTime;
        if(CreatureStats.currentAge >= CreatureStats.matureAge)
        {
            mature = true;
        }
    }
    public void WhileEating()
    {
        if(CreatureBehavior.eating == true)
        {
            CreatureStats.currentMovementSpeed = CreatureStats.maxMovementSpeed / 2;
        }
        else
        {
            CreatureStats.currentMovementSpeed = CreatureStats.maxMovementSpeed;
        }
    }
    
}
