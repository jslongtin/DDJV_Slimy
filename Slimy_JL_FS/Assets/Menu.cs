using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private Transform m_menuConfirmation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPause()
    {
            if (Time.timeScale> 0.5f)
        {
            //pause le son
            //desactiver les controles
            Time.timeScale = 0.0f;

        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
    public void OnTryQuitt()
    {
        m_menuConfirmation.gameObject.SetActive(true);
    }
    public void OnQuitt()
    {
        // ne fonctionne pas dans l'editeur
        Application.Quit();
        Debug.LogWarning("Sashay awway");
    }
    public void OnCancelQuitt()
    {
        m_menuConfirmation.gameObject.SetActive(false);
    }
}
