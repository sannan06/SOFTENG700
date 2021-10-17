using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Class Snap implements logic to allow objects to snap together
*/
public class Snap : MonoBehaviour
{
    public TouchTracker touchTracker;

    // Reference to prefab for slice group
    public Transform sliceGroupPrefab;

    // Layer index of pizza slices
    private int sliceLayer;

    void Awake()
    {
        sliceLayer = LayerMask.NameToLayer("Pizza");
    }    

    void OnCollisionEnter(Collision other)
    {
        // if collision is not with a pizza slice, ignore the collision
        if(other.gameObject.layer != sliceLayer) {
            return;
        }

        // Get the slice group that this slice is in
        SliceGroup thisGroup = transform.GetComponent<SliceGroup>() ?? transform.parent.GetComponent<SliceGroup>();
        // Get the slice group of the other object
        SliceGroup otherGroup = other.transform.GetComponent<SliceGroup>() ?? other.transform.parent.GetComponent<SliceGroup>();

        // if they belong to the same slice group, dont need to do anything
        if(thisGroup != null && thisGroup == otherGroup) {
            return;
        }

        Debug.Log($"Collision between {this.transform.name} and {other.transform.name}");


        // Resulting slice group cannot be greater than a whole pizza (i.e. <= 1/1)
        var sum = thisGroup == null ? GetComponent<Slice>().fraction : thisGroup.fraction;
        sum += otherGroup == null ? other.transform.GetComponent<Slice>().fraction : otherGroup.fraction;

        Debug.Log($"Sum is ${sum.numerator}/{sum.denominator}");

        if(sum.numerator > sum.denominator) {
            // greater than 1
            return;
        }
        
        // if other slice dosent belong to a group, make one.
        if(otherGroup == null) {

            otherGroup = Instantiate(sliceGroupPrefab, other.transform.position, other.transform.rotation).GetComponent<SliceGroup>();
            
            otherGroup.AddSlice(other.transform.GetComponent<Slice>());

        }

        // if pizza already belongs to a slice group, merge them
        if(thisGroup != null) {

            otherGroup.AddSliceGroup(thisGroup);

            Destroy(thisGroup.gameObject);

        } else {
            otherGroup.AddSlice(GetComponent<Slice>());
        }
        
        touchTracker.setLastTouched(null);
    }


}
