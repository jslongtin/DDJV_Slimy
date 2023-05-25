using UnityEngine;

public class BridgeController : MonoBehaviour
{
    public bool isActivated = false;
    public GameObject bridgeBase;
    public GameObject bridgeSides;

    public void ActivateBridge()
    {
        isActivated = true;
        bridgeBase.SetActive(true);
        bridgeSides.SetActive(true);
    }
}
