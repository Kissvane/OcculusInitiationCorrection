using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayerController : MonoBehaviour
{
    public float speed = 10f;
    public OVRInput.Axis2D axis;
    public Transform myTransform;
    public Transform Hand;

    // Start is called before the first frame update
    void Awake()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        //get joystick inputs
        Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        if (primaryAxis != Vector2.zero)
        {
            //move the player with joystick inputs
            myTransform.position += new Vector3(primaryAxis.x, 0, primaryAxis.y) * speed * Time.deltaTime;
        }
    }
}
