using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    public TMP_Text fpsText;
    float fps;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FramesPerSecond());
    }

    // Update is called once per frame
    void Update()
    {
        fpsText.text = fps.ToString();
    }

    IEnumerator FramesPerSecond()
    {
        
        while (true)
        {
            
            yield return new WaitForSeconds(1);
            fps = (int)(1f / Time.unscaledDeltaTime);
        }
        
    }
}
