using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Sacrifice : MonoBehaviour
{
    private UnityAction<object> ev_Slime;
    // Start is called before the first frame update
    void Start()
    {
        ev_Slime = new UnityAction<object>(ev_Sacrefice);
        EventManager.StartListening("sacrifice", ev_Slime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ev_Sacrefice(object ob)
    {

        Debug.Log(ob.GetType());
       



    }
}
