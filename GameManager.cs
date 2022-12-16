using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //public GameObject planet;

    public int setTimeScale;
    public float SimulationAge;
    // Start is called before the first frame update
    private void Awake()
    {
        //Time.timeScale = Time.fixedDeltaTime;
        //Application.targetFrameRate = 200;
    }
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        //Time.timeScale = timeScale;
    }
    // Update is called once per frame
    void Update()
    {
        Time.timeScale = setTimeScale;

        SimulationAgeing();
    }

    public void SimulationAgeing()
    {
        SimulationAge += 1 * Time.deltaTime;
    }
}
