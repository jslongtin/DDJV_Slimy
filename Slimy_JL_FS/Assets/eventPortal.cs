using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class eventPortal : MonoBehaviour
{
    private UnityAction<object> ev_Slime;
    public GameObject[] particleSystems;
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
            if (particleSystems.Length == 3)
            {
                particleSystems[0].SetActive(true);
                particleSystems[1].SetActive(true);
                particleSystems[2].SetActive(true);
                particleSystems[0].GetComponent<ParticleSystem>().Play();
                particleSystems[1].GetComponent<ParticleSystem>().Play();
                particleSystems[2].GetComponent<ParticleSystem>().Play();
            }
            
            Debug.Log("atata");
        }
       


    }
}
