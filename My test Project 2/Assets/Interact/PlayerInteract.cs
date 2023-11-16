using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    //private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private string CurString;
    private string PastString;
    private PlayerInput.PlayerBasicCtrlActions BasicCtrl;
    public TextMeshProUGUI interactText;
    public PlayerController controller;
    private bool onInteract;
    private List<GameObject> itemList = new List<GameObject>();

    void Start()
    {
        cam = controller.cam;
        CurString = string.Empty;
        PastString = string.Empty;
        UpdateInteractText(CurString);
        BasicCtrl = controller.BasicCtrl;
    }
    void Update()
    {
        PastString = CurString;
        CurString = string.Empty;
        //Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        //Debug.DrawRay(ray.origin, ray.direction * distance);
        //RaycastHit hitInfo;
        /*if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                if (interactable.InteractionType == InteractionMethod.Raycast)
                {
                    CurString = interactable.promptMessage;
                    if (BasicCtrl.Interact.triggered)
                    {
                        interactable.BaseInteract();
                    }
                }
                    
            }
        }*/
        if (itemList.Count > 0)
        {
            GameObject lastObject = itemList[itemList.Count - 1];
            Interactable interactable = lastObject.GetComponent<Interactable>();
            CurString = interactable.promptMessage;
            if (BasicCtrl.Interact.triggered)
            {
                interactable.BaseInteract();
                //itemList.Remove(lastObject);
            }
        }

        
        if (PastString != CurString)
            UpdateInteractText(CurString);
    }
    private void OnControllerColliderHit(ControllerColliderHit collision)
    {
        //Debug.Log(collision.collider.name);
        if (collision.collider.GetComponent<Interactable>() != null)
        { 
            Interactable interactable = collision.collider.GetComponent<Interactable>();
            if (interactable.InteractionType == InteractionMethod.Collision)
            {
                interactable.BaseInteract();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Interactable>() != null)
        {
            itemList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Interactable>() != null)
        {
            itemList.Remove(other.gameObject);
        }
    }


    void UpdateInteractText(string text)
    {
        interactText.text = text;
    }
}
