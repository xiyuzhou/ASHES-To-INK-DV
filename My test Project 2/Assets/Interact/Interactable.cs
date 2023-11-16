using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Default,
}
public enum InteractionMethod
{
    Raycast,
    Cursor,
    Collision,
    WithInRange
}
public abstract class Interactable : MonoBehaviour
{
    public bool useEvents;
    public bool DestoryAfterInteraction;
    //private bool UseCollision;
    public string promptMessage;
    public ItemType itemtype = ItemType.Default;
    public InteractionMethod InteractionType = InteractionMethod.WithInRange;
    public ParticleSystem DestroyEffect;
    public virtual string OnLook()
    {
        return promptMessage;
    }
    public void BaseInteract()
    {
        if (useEvents)
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        Interact();
        if (DestoryAfterInteraction)
            Destroy(gameObject);
    }
    protected virtual void Interact()
    {

    }
}
