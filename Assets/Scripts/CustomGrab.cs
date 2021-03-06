using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrab : MonoBehaviour
{
    public OVRInput.Button button;
    public VrPointer pointer;
    //the transform of the grabbed object
    Transform grabbed;
    //the transform used as parent when an object is grabbed
    public Transform grabTransform;
    //the original parent of the grabbed object 
    Transform originalParent;
    bool previousGrabbedKinematicState = false; 

    // Start is called before the first frame update
    void Awake()
    {
        if (grabTransform == null)
        {
            grabTransform = transform;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //when pressing down grab button and a grabbable object is touched by the pointer
        if (OVRInput.GetDown(button) && grabbed == null && pointer.hovered != null)
        {
            //grab the object hovered by the pointer
            Grab(pointer.hovered.transform);  
        }
        //when releasing grab button and an object is grabbed
        else if (OVRInput.GetUp(button) && grabbed != null)
        {
            //ungrab it
            Ungrab();
        }
    }

    //grab the tarnsform t
    void Grab(Transform t)
    {
        grabbed = t;
        grabbed.parent = grabTransform;
        //ignore gravity and other forces while the object is grabbed
        Collider col = grabbed.GetComponent<Collider>();
        if (col.attachedRigidbody != null)
        {
            previousGrabbedKinematicState = col.attachedRigidbody.isKinematic;
            col.attachedRigidbody.isKinematic = true;
        }
    }

    //ungrab the grabbed object
    void Ungrab()
    {
        //set the isKenimatic property as it was before the grab
        Collider col = grabbed.GetComponent<Collider>();
        if (col.attachedRigidbody != null)
        {
            col.attachedRigidbody.isKinematic = previousGrabbedKinematicState;
        }
        grabbed.transform.parent = null;
        grabbed = null;
    }
}
