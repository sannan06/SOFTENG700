using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Class TouchTracker tracks what object the hand has touched last
*/
[CreateAssetMenu(fileName = "TouchTracker", menuName = "SOFTENG700/TouchTracker", order = 0)]
public class TouchTracker : ScriptableObject {
    public GameObject lastTouched; 

    public void setLastTouched(GameObject obj) {
        lastTouched = obj;
    }
}
