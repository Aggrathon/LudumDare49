using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Despawner : MonoBehaviour
{
    public UnityEvent onTrigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody)
        {
            Destroy(other.attachedRigidbody.gameObject);
            onTrigger.Invoke();
        }
    }
}
