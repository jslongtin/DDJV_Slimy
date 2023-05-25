using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class bridgePiedestal : MonoBehaviour
{
    private UnityAction<object> ev_Slime;
    public static int slimeOnPlace = 0;
    [SerializeField]
    private GameObject bridge;
    [SerializeField]
    private GameObject blueSlime;
    [SerializeField]
    private GameObject angelSlime;


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
        }
        if (slimeOnPlace == 3)
        {
    
            bridge.SetActive(true);
            EventManager.TriggerEvent("sacrifice", (blueSlime.transform.position,angelSlime.transform.position));
            Destroy(blueSlime);
            Destroy(angelSlime);

        }


    }
}
