using UnityEngine;
using UnityEngine.Events;

public class SlimeController : MonoBehaviour
{
    public Transform pedestal;
    public Transform bridge;
    public GameObject otherSlime;


    private bool isOnPedestal;


    private void Start()
    {
        isOnPedestal = false;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == otherSlime)
        {
            isOnPedestal = true;
            Debug.Log(" slime entered pedestal trigger");
        }

        CheckBridgeUnlockConditions();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == otherSlime)
        {
            isOnPedestal = false;
            Debug.Log("slime exited pedestal1 trigger");
        }

        CheckBridgeUnlockConditions();
    }

    private void CheckBridgeUnlockConditions()
    {
        // Check if all conditions are met to unlock the bridge
        if (isOnPedestal)
        {
            EventManager.TriggerEvent("stepPiedestal", otherSlime);
        }
    }
}
