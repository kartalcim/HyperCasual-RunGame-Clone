using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruzgar : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("AltKarakterler"))
        {
            other.GetComponent<Rigidbody>().AddForce(new Vector3(-4, 0, 0), ForceMode.Impulse);
        }
    }
}
