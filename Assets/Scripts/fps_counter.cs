using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class fps_counter : MonoBehaviour
{
    public Text text;
    private int counter;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        counter = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        counter++;
        if(time>=1)
        {
            Debug.Log((int)(counter / time));
            text.text = ((int)(counter / time)).ToString()+" FPS";
            time -= 1;
            counter = 0;
        }
    }
}
