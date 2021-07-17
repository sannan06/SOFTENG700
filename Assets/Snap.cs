using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Snapper") {
            Debug.Log(other.gameObject.tag );

        }   
    }
}
