using UnityEngine;

public class PlayerTouch : MonoBehaviour {

    [SerializeField]
    private TouchTracker touchTracker;

    void OnCollisionEnter(Collision collisionInfo)
    {
        if(collisionInfo.gameObject.layer == LayerMask.NameToLayer("Pizza")) {
            touchTracker.setLastTouched(collisionInfo.gameObject);
        }
    }
}