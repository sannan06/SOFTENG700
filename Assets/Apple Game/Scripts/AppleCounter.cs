using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**
    Class AppleCounter deals with functionality to detect new apples entering a area and updating text
*/
public class AppleCounter : MonoBehaviour
{
    int totalApples;
    int greenApples;

    Fraction goal = "3/7";

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

    /**
        On new apple enter, increment relevant fraction and update text
    */
    void OnTriggerEnter(Collider other)
    {
        // chceck if gameobject is actually an apple
        if(other.gameObject.layer != LayerMask.NameToLayer("Apple")) {
            return;
        }

        totalApples++;
        if(other.tag.Equals("GreenApple")) {
            greenApples++;
        }

        // goal is 3/7
        updateText();
        
    }

    /** On apple exist, should decrement fraction. */
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer != LayerMask.NameToLayer("Apple")) {
            return;
        }
        totalApples--;
        if(other.tag.Equals("GreenApple")) {
            greenApples--;
        }


        updateText();
    }

    /** Update Text depending on if it reached goal */
    void updateText() {
        if(greenApples == goal.numerator && totalApples == goal.denominator) {
            text.SetText($"Nice!");
        } else {
            text.SetText($"Current: {greenApples}/{totalApples}");
        }   
    }

}
