using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalInteract : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Animal"))
        {
            Debug.Log("MOO");
        }
    }
}
