using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TouchTracker", menuName = "SOFTENG700/TouchTracker", order = 0)]
public class TouchTracker : ScriptableObject {
    public GameObject lastTouched; 

    public void setLastTouched(GameObject obj) {
        lastTouched = obj;
    }
}
