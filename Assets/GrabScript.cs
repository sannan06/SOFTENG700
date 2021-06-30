using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViveHandTracking.Sample {

  // This script controls action about grabing boxes remotely
  class GrabScript : MonoBehaviour {
    private static Color grabColor = new Color(0, 0, 0.3f, 1);
    private static Color selectColor = new Color(0, 0.3f, 0, 1);

    public LineRenderer laser = null;

    private Transform Camera = null;
    private Transform Anchor = null;
    private int state = 0;
    private Renderer candidate = null;
    private Renderer selected = null;

    void Awake() {
      Anchor = new GameObject("Anchor").transform;
      Anchor.parent = transform;
    }

    void Start() {
      Camera = GestureProvider.Current.transform;
    }

    void Update() {
      if (state == 0) return;

      Anchor.position = GestureProvider.RightHand.position;

      if (state == 2) return;

      // find hit objects by raycast
      RaycastHit hit;
      LayerMask mask = LayerMask.GetMask("Pickable");
      if (Physics.Raycast(laser.transform.position, laser.transform.forward, out hit, Mathf.Infinity, mask)) {
        if (candidate == hit.collider.GetComponent<Renderer>()) return;
        Debug.Log(hit.collider);
        SetCandidate(hit.collider);
      } else
        ClearCandidate();
    }

    public void OnStateChanged(int state) {
      this.state = state;
      if (state == 2) {
        selected = candidate;
        if (selected != null) {
          selected.GetComponent<Rigidbody>().useGravity = false;
          selected.GetComponent<Rigidbody>().drag = 5f;
          Anchor.SetParent(selected.transform.parent, true);
          selected.transform.SetParent(Anchor, true);
        }
      } else if (selected != null) {
        selected.GetComponent<Rigidbody>().useGravity = true;
        selected.GetComponent<Rigidbody>().drag = 0.5f;
        selected.transform.SetParent(Anchor.parent, true);
        Anchor.SetParent(transform, true);
        selected = null;
      }
      if (selected != null)
        selected.material.SetColor("_EmissionColor", selectColor);
      else if (state != 1)
        ClearCandidate();
    }

    void SetCandidate(Collider other) {
      if (candidate != null) ClearCandidate();
      candidate = other.GetComponent<Renderer>();
      if (candidate != null) {
        candidate.material.EnableKeyword("_EMISSION");
        candidate.material.SetColor("_EmissionColor", grabColor);
      }
    }

    void ClearCandidate() {
      if (candidate != null) candidate.material.DisableKeyword("_EMISSION");
      candidate = null;
    }
  }

}
