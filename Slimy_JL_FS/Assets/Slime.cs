using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement; // Pour recharger/changer de scène
using TMPro;
using UnityEngine.UI;

public class Slime : MonoBehaviour
{
    private Rigidbody2D rig;
    private Vector2 mouvement;
    private bool ouvrir;
    public float vitesse;
    private Animator anim;
    public TMP_Text slimeText;
    public Image blackScreen;


    private Scene currentScene;

    private Vector2 lastMovementDirection;

    public static int nb_slime = 10;

    public GameObject projectilePrefab;
    public float shootForce = 10f;

    public GameObject[] bridgeObjects; // Array to store the bridge GameObjects
    public GameObject[] bridgeblockers; // Array to store the bridge GameObjects

    private bool hasShotInTrigger = false;
    public GameObject[] particleSystems;
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentScene = SceneManager.GetActiveScene();
        StartCoroutine(FadeStart());
    }

    void Update()
    {

        slimeText.text = "Slimes: " + nb_slime.ToString();

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        mouvement = new Vector2(horizontalInput, verticalInput).normalized;

        anim.SetFloat("Horizontal", mouvement.x);
        anim.SetFloat("Vertical", mouvement.y);


        float size = Mathf.Clamp(nb_slime / 3, 1, 20);
        transform.localScale = new Vector3(size, size, 1);

        Vector2 movementDirection = mouvement.normalized;

        if (movementDirection != Vector2.zero)
        {
            lastMovementDirection = movementDirection;
        }
        if (Input.GetMouseButtonDown(0) && nb_slime > 0)
        {
            ShootProjectile();
            nb_slime--;
            hasShotInTrigger = true;
            StartCoroutine(ResetHasShotInTrigger()); // Start the Coroutine
            UnityEngine.Debug.Log(nb_slime);
        }


        currentScene = SceneManager.GetActiveScene();
        if(nb_slime <= 0) 
        {
            nb_slime = 10;
            SceneManager.LoadScene(currentScene.name);
        }


        anim.SetFloat("LastMovementDirectionX", lastMovementDirection.x);
        anim.SetFloat("LastMovementDirectionY", lastMovementDirection.y);


    }



    void FixedUpdate()
    {
        rig.velocity = mouvement * vitesse;
        rig.velocity = rig.velocity.normalized * vitesse;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Ennemi")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            // Stop moving
            rig.velocity = Vector2.zero;
            // Disable animations
            anim.enabled = false;

            // Wait for a moment

        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer).Contains("areaBridge") && hasShotInTrigger && nb_slime > 7)
        {
            int bridgeIndex = int.Parse(LayerMask.LayerToName(collision.gameObject.layer).Substring(10)) - 1;

            // Play the particle system for the respective bridge
            particleSystems[bridgeIndex].GetComponent<ParticleSystem>().Play();
            bridgeObjects[bridgeIndex].SetActive(true);
            bridgeblockers[bridgeIndex].SetActive(false);

            nb_slime -= 7;
            StartCoroutine(ResetHasShotInTrigger());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (LayerMask.LayerToName(collision.gameObject.layer) == "Ennemi")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            // Stop moving
            rig.velocity = Vector2.zero;
            // Disable animations
            anim.enabled = false;


        }
        if (LayerMask.LayerToName(collision.gameObject.layer) == "ObjetRamassable")
        {

            Destroy(collision.gameObject);
        }
        if (LayerMask.LayerToName(collision.gameObject.layer) == "projectile_enemie")
        {
            nb_slime -= 1;
            Destroy(collision.gameObject);
        }
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Ennemi")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            // Stop moving
            rig.velocity = Vector2.zero;
            // Disable animations
            anim.enabled = false;


        }
        if (LayerMask.LayerToName(collision.gameObject.layer) == "portal")
        {

            StartCoroutine(ChangeScene());

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer).Contains("areaBridge"))
        {
            hasShotInTrigger = false; // Reset the flag when the player exits the trigger

        }
    }
    void ShootProjectile()
    {
        GameObject newProjectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
        rb.AddForce(lastMovementDirection * shootForce, ForceMode2D.Impulse);

    }
    private IEnumerator ResetHasShotInTrigger()
    {
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds
        hasShotInTrigger = false; // Reset the flag
    }
    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(2f);

        // Assuming blackScreen is your UI Image component
        for (float t = 0f; t <= 1; t += Time.deltaTime / 2f)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(0, 1, t));
            blackScreen.color = newColor;
            yield return null;
        }

        // Load new scene
        SceneManager.LoadScene("Niveau2");
    }
    private IEnumerator FadeStart()
    {


        // Assuming blackScreen is your UI Image component
        for (float t = 0f; t <= 1; t += Time.deltaTime / 2f)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(1, 0, t));
            blackScreen.color = newColor;
            yield return null;
        }
    }
}