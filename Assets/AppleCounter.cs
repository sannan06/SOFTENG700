using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleCounter : MonoBehaviour
{
    int totalApples;
    int greenApples;
    // Start is called before the first frame update
    IEnumerator Start()
    {  
        while(true) {
            Debug.Log($"{greenApples}/{totalApples}");
            yield return new WaitForSeconds(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        totalApples++;
        if(other.tag.Equals("GreenApple")) {
            greenApples++;
        }


    }

    void OnTriggerExit(Collider other)
    {
        totalApples--;
        if(other.tag.Equals("GreenApple")) {
            greenApples--;
        }

    }
}
