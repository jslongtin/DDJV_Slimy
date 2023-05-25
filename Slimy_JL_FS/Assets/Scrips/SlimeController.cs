using UnityEngine;
using UnityEngine.Events;

public class SlimeController : MonoBehaviour
{
    public GameObject targetSlime;


    private void Start()
    {
  

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == targetSlime)
        {

            CheckBridgeUnlockConditions(1);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == targetSlime)
        {
            CheckBridgeUnlockConditions(-1);
        }

      
    }

    private void CheckBridgeUnlockConditions(int value)
    {
        // Check if all conditions are met to unlock the bridge
         EventManager.TriggerEvent("stepPiedestal", value);
       
    }
}
