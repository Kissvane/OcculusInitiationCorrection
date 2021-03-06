using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTeleport : MonoBehaviour
{
    public OVRInput.Button teleportButton;
    public VrPointer pointer;
    public Transform playerTransform;

    private void Awake()
    {
        //set the teleport button of my linked pointer to the same as mine
        pointer.teleportButton = teleportButton;
    }

    // Update is called once per frame
    void Update()
    {
        //if teleport button is released and pointed position is valid
        if (OVRInput.GetUp(teleportButton) && pointer.wantedPosition.HasValue)
        {
            //teleport player to pointed position
            playerTransform.position = pointer.wantedPosition.Value;
        }
    }
}
