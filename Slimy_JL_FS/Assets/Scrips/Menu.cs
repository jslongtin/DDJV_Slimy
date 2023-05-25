using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Pour recharger/changer de scène<
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private Transform m_menuConfirmation;
    public Image blackScreen;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeStart());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPlay()
    {
        StartCoroutine(FadeEnd());
        SceneManager.LoadScene("Niveau1");
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
    private IEnumerator FadeStart()
    {
        for (float t = 0f; t <= 1; t += Time.deltaTime / 2f)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(1, 0, t));
            blackScreen.color = newColor;
            yield return null;
        }
        blackScreen.transform.gameObject.SetActive(false);
    }
    private IEnumerator FadeEnd()
    {
        blackScreen.transform.gameObject.SetActive(true);
        for (float t = 1f; t <= 0; t -= Time.deltaTime / 2f)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(1, 0, t));
            blackScreen.color = newColor;
            yield return null;
        }
        
    }
}
