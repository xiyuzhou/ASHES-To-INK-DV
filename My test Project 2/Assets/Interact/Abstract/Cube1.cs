using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube1 : Interactable
{
    protected override void Interact()
    {
        this.gameObject.transform.localScale *= 0.9f;
    }
}
