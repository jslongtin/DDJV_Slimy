using UnityEngine;
using UnityEngine.Events;

public class SlimeController : MonoBehaviour
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

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == otherSlime)
        {
            CheckBridgeUnlockConditions(-1);
            Debug.Log("slime exited pedestal1 trigger");
        }

      
    }

    private void CheckBridgeUnlockConditions(int value)
    {
        // Check if all conditions are met to unlock the bridge
         EventManager.TriggerEvent("stepPiedestal", value);
       
    }
}
