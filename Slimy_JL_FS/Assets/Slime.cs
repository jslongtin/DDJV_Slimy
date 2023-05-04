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
}
