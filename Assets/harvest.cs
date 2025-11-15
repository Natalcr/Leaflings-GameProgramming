using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class harvest : MonoBehaviour
{
 private int cropCount = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Animal"))
        {
            Debug.Log("MOO");
        }

        if (other.CompareTag("Plant"))
        {
            cropCount++;
            Debug.Log("Crop harvested: " + cropCount);

            Destroy(other.gameObject);
        }
    }
}
