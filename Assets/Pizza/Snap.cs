using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap : MonoBehaviour
{
    public TouchTracker touchTracker;

    public SliceGroup sliceGroupPrefab;

    private int sliceLayer;

    void Awake()
    {
        sliceLayer = LayerMask.NameToLayer("Pizza");
    }

    void OnCollisionEnter(Collision other)
    {

        // if collision is not with a pizza slice, ignore the collision
        if(other.gameObject.layer != sliceLayer) {
            Debug.Log($"Not in slice layer");

            return;
        }

        // if collision occurs not due to the user pushing the slice, ignore the collision
        // if(touchTracker.lastTouched != gameObject) {
        //     Debug.Log($"Not touched gameObject");

        //     return;
        // }

        // if slice being pushed around is currently in a group, ignore the collision,
        if(transform.parent.GetComponent<SliceGroup>() != null) {
            Debug.Log($"Already has parent group");

            return;
        }

        // get slice group of other group
        SliceGroup group = other.transform.GetComponent<SliceGroup>();
        
        // if other slice dosent belong to a group, make one.
        if(group == null) {
            
            group = Instantiate(sliceGroupPrefab, other.transform.position, other.transform.rotation);

            group.AddSlice(other.transform.GetComponent<Slice>());
        }

        group.AddSlice(GetComponent<Slice>());

        touchTracker.setLastTouched(null);
    }
}
