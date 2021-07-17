using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AppleCounter : MonoBehaviour
{
    int totalApples;
    int greenApples;

    [SerializeField]
    TextMeshProUGUI text;

    // Start is called before the first frame update
    IEnumerator Start()
    {  
        text.SetText($"Current: {greenApples}/{totalApples}");

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
        if(other.gameObject.layer != LayerMask.NameToLayer("Apple")) {
            return;
        }

        totalApples++;
        if(other.tag.Equals("GreenApple")) {
            greenApples++;
        }

        if(greenApples == 3 && totalApples == 7) {
            text.SetText($"Nice!");
        } else {
            text.SetText($"Current: {greenApples}/{totalApples}");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer != LayerMask.NameToLayer("Apple")) {
            return;
        }
        totalApples--;
        if(other.tag.Equals("GreenApple")) {
            greenApples--;
        }


        if(greenApples == 3 && totalApples == 7) {
            text.SetText($"Nice!");
        } else {
            text.SetText($"Current: {greenApples}/{totalApples}");
        }
    }
}
