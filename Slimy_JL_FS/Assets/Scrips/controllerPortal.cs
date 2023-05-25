using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllerPortal : MonoBehaviour
{
    public GameObject otherSlime;


    private void Start()
    {


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == otherSlime)
        {

            CheckBridgeUnlockConditions(1);
            Debug.Log(" slime entered pedestal trigger");
        }

    }


    private void CheckBridgeUnlockConditions(int value)
    {
        // Check if all conditions are met to unlock the bridge
        EventManager.TriggerEvent("stepPiedestal", value);

    }
}
