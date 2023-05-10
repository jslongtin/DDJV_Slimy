using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement; // Pour recharger/changer de scène

public class Slime : MonoBehaviour
{
    private Rigidbody2D rig;
    private Vector2 mouvement;
    private bool ouvrir;
    public float vitesse;
    private Animator anim;



    private Scene currentScene;

    private Vector2 lastMovementDirection;

    [Serialize]
    private int nb_slime = 10;

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
    }

    void Update()
    {


        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        mouvement = new Vector2(horizontalInput, verticalInput).normalized;

        anim.SetFloat("Horizontal", mouvement.x);
        anim.SetFloat("Vertical", mouvement.y);


        Vector2 movementDirection = mouvement.normalized;

        if (movementDirection != Vector2.zero)
        {
            lastMovementDirection = movementDirection;
        }
        if (Input.GetMouseButtonDown(0) && nb_slime > 1)
        {
            ShootProjectile();
            nb_slime--;
            hasShotInTrigger = true;
            StartCoroutine(ResetHasShotInTrigger()); // Start the Coroutine
            UnityEngine.Debug.Log(nb_slime);
        }

       

        currentScene = SceneManager.GetActiveScene();


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
        if (LayerMask.LayerToName(collision.gameObject.layer).Contains("areaBridge") && hasShotInTrigger && nb_slime > 1)
        {
            int bridgeIndex = int.Parse(LayerMask.LayerToName(collision.gameObject.layer).Substring(10)) - 1;

            // Play the particle system for the respective bridge
            particleSystems[bridgeIndex].GetComponent<ParticleSystem>().Play();
            bridgeObjects[bridgeIndex].SetActive(true);
            bridgeblockers[bridgeIndex].SetActive(false);

            nb_slime--;
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
    
}