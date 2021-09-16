using UnityEngine;
using System.Collections.Generic;
using TMPro;

[SelectionBase]
public class SliceGroup : MonoBehaviour {

    public TextMeshPro text;
    private List<Slice> slices;

    public Fraction fraction = "0/1";

    void Awake()
    {
        slices = new List<Slice>();
    }

    void Update()
    {
        
    }

    public void AddSlice(Slice slice) {

        if(slices.Contains(slice)) {
            return;
        }

        Debug.Log(slice);

        Destroy(slice.GetComponent<Rigidbody>());
        Destroy(slice.GetComponent<Snap>());


        slice.transform.parent = this.transform;

        // slice.transform.localPosition = slices.Count == 0 ? Vector3.zero : slices[0].transform.localPosition;
        slice.transform.localPosition =Vector3.zero;
        slice.transform.localRotation = Quaternion.Euler(0, 360 * ((float) fraction.numerator/fraction.denominator), 0);

        // if(slices.Count > 0) {
        //     FixedJoint joint = slice.gameObject.AddComponent<FixedJoint>();
        //     joint.connectedBody = slices[slices.Count - 1].GetComponent<Rigidbody>();
        // }

        slices.Add(slice);
        fraction += slice.fraction;

        name = $"SliceGroup: {fraction}";

        text.SetText(fraction.ToString());


    }

    public void AddSliceGroup(SliceGroup sliceGroup) {
        foreach(Slice slice in sliceGroup.slices) {
            AddSlice(slice);
        }
    }

    // public static SliceGroup CreateSliceGroup(Vector3 position, Quaternion rotation) {
    //     GameObject newGroup = new GameObject();

    //     newGroup.transform.position = position;
    //     newGroup.transform.rotation = rotation;

    //     newGroup.layer = LayerMask.NameToLayer("Pizza");

    //     newGroup.AddComponent<Rigidbody>();

    //     return newGroup.AddComponent<SliceGroup>();
    // }
}