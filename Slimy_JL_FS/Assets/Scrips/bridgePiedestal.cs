using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class bridgePiedestal : MonoBehaviour
{
    private UnityAction<object> ev_Slime;
    public static int slimeOnPlace = 0;
    public GameObject bridge;
    // Start is called before the first frame update
    void Start()
    {
        ev_Slime = new UnityAction<object>(projet);
        EventManager.StartListening("stepPiedestal", ev_Slime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void projet(object ob)
    {
        if (ob is int)
        {
            slimeOnPlace += (int)ob;
            Debug.Log(slimeOnPlace);
        }
        if (slimeOnPlace == 3)
        {
            //faire pont
            Debug.Log("faire pont");
            bridge.SetActive(true);
        }


    }
}
