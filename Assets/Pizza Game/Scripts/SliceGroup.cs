using UnityEngine;
using System.Collections.Generic;
using TMPro;


/**
    Class SliceGroup represents a collection of pizza slices. 
    This class implements logic related to the addition of pizza slices.
*/
[SelectionBase]
public class SliceGroup : MonoBehaviour {

    public TextMeshPro text;
    private List<Slice> slices;

    public Fraction fraction = "0/1";

    void Awake()
    {
        slices = new List<Slice>();
    }

    /**
        Given a slice, add the slice to current slice group.
    */
    public void AddSlice(Slice slice) {

        if(slices.Contains(slice)) {
            return;
        }

        Debug.Log(slice);

        // Slice group already has a rigidbody and snap components so we can remove them on slice. 
        // i.e. the slice group is a compound collider made up of individual pizza slice colliders
        Destroy(slice.GetComponent<Rigidbody>());
        Destroy(slice.GetComponent<Snap>());

        // Set up parent relationship between slice group and slice.
        slice.transform.parent = this.transform;

        // Rotation is relative to slice group 
        slice.transform.localPosition =Vector3.zero;
        slice.transform.localRotation = Quaternion.Euler(0, 360 * ((float) fraction.numerator/fraction.denominator), 0);


        slices.Add(slice);
        fraction += slice.fraction;

        name = $"SliceGroup: {fraction}";
        
        // update text above the slice group to represent new fraction
        text.SetText(fraction.ToString());
    }


    /**
        Given a slice group, add all slices in slice group to current slice group.
    */
    public void AddSliceGroup(SliceGroup sliceGroup) {
        foreach(Slice slice in sliceGroup.slices) {
            AddSlice(slice);
        }
    }

}