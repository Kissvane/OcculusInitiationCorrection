using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrPointer : MonoBehaviour
{
    public enum pointerMode { grab, teleport };

    public Material hoverMaterial;
    [HideInInspector]
    public Material originalMaterial;
    public float PointerLength = 100f;
    Transform Hand;
    [HideInInspector]
    public Renderer hovered;
    Renderer lastHovered;
    public LineRenderer lineRenderer;

    //layer mask
    public LayerMask grabbable;
    public LayerMask teleport;

    [HideInInspector]
    public OVRInput.Button teleportButton;

    //nullable vector
    // when it's null the ray don't touch a valid point to be teleport at
    public Vector3? wantedPosition = null;

    //useful stuff to parameter ray feedbacks
    public pointerMode mode = VrPointer.pointerMode.grab;
    public Gradient grabGradient;
    public Gradient teleportGradient;

    // Start is called before the first frame update
    void Awake()
    {
        Hand = transform;
        //initialize lineRenderer
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(Hand.position, Hand.forward);
        lineRenderer.SetPosition(0, ray.origin);
        //if the player is pressing the teleport button
        if (OVRInput.Get(teleportButton))
        {
            //we change pointer color
            if (mode == pointerMode.grab)
            {
                lineRenderer.colorGradient = teleportGradient;
                mode = pointerMode.teleport;
            }
            //making a raycast that only touch ground
            if (Physics.Raycast(ray, out RaycastHit hit, PointerLength, teleport))
            {
                //storing the raycast hit point
                wantedPosition = hit.point;
                //positioning end point of line renderer
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                //storing that the raycast touch nothing
                wantedPosition = null;
                //positioning end point of line renderer
                lineRenderer.SetPosition(1, ray.origin + ray.direction * PointerLength);
            }
        }
        //otherwise
        else
        {
            //we change pointer color
            if (mode == pointerMode.teleport)
            {
                lineRenderer.colorGradient = grabGradient;
                mode = pointerMode.grab;
            }
            //making a raycast that only touch grabbable objects
            if (Physics.Raycast(ray, out RaycastHit hit, PointerLength, grabbable))
            {
                //storing the renderer hit by the raycast
                hovered = hit.collider.gameObject.GetComponent<Renderer>();
                //positioning end point of line renderer
                lineRenderer.SetPosition(1, hit.point);
                //manage hover feedback
                if (lastHovered != hovered)
                {
                    if (lastHovered != null)
                    {
                        lastHovered.material = originalMaterial;
                    }
                    lastHovered = hovered;
                    originalMaterial = hovered.material;
                    hovered.material = hoverMaterial;
                }
            }
            else
            {
                //storing that the raycast touch nothing
                hovered = null;
                //manage hover feedback
                if (lastHovered != hovered)
                {
                    if (lastHovered != null)
                    {
                        lastHovered.material = originalMaterial;
                    }
                    lastHovered = null;
                }
                //positioning end point of line renderer
                lineRenderer.SetPosition(1, ray.origin + ray.direction * PointerLength);
            }
        }


    }
}
