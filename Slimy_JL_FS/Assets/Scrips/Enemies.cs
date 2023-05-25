using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    [SerializeField]
    private string nomDeLaCible;

    [SerializeField]
    private float longueurRayon;

    [SerializeField]
    private float vitesse = 2.0f;
    [SerializeField]
    private float vitesseProjectile = 7.0f;

    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private ParticleSystem destroyParticlesPrefab;

    private Transform cible;
    private Rigidbody2D rig;
    private Vector3 direction;
    private Vector3 mouvement;
    private bool estEnChasse = false;
    private bool canMove = true;
    private bool isShooting = false;

    Color col = Color.red;
    float longueurDebug;

    // Start is called before the first frame update
    void Start()
    {
        longueurDebug = longueurRayon;
        GameObject go = GameObject.Find(nomDeLaCible);
        Debug.Assert(go != null);
        if (go != null)
        {
            cible = go.transform;
        }

        rig = GetComponent<Rigidbody2D>();

        StartCoroutine(Errance());
        estEnChasse = false;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = mouvement.x;
        float verticalInput = mouvement.y;
        mouvement = new Vector2(horizontalInput, verticalInput).normalized;

        Debug.DrawRay(transform.position, direction * longueurDebug, col);
        Vector2 movementDirection = mouvement.normalized;



        float idleFrameIndex = Random.Range(0.5f, 2f); // Assumes you have 4 idle animations, numbered 0 through 3
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            rig.velocity = Vector2.zero;
            return;
        }

        direction = cible.position - transform.position;
        direction.Normalize();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, longueurRayon, LayerMask.GetMask("slime", "Mur", "hidingSpot"));
        if (hit.collider != null)
        {
            col = Color.blue;
            longueurDebug = hit.distance;
            if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "slime")
            {
                if (!estEnChasse)
                {
                    StopCoroutine(Errance());
                    estEnChasse = true;
                    if (!isShooting)
                    {
                        isShooting = true;
                        StartCoroutine(ShootAtSlime());
                    }
                }
                col = Color.green;
                mouvement = direction;
            }
            else
            {
                if (estEnChasse)
                {
                    estEnChasse = false;
                    isShooting = false;
                    StartCoroutine(Errance());
                }
            }
        }
        else
        {
            if (estEnChasse)
            {
                estEnChasse = false;
                isShooting = false;
            }
            StartCoroutine(Errance());
        }
        rig.velocity = mouvement * vitesse;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "slime")
        {
            //canMove = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "projectile_slime")
        {
            Slime.nb_slime += 5;
            Destroy(gameObject);
            Destroy(collision.gameObject);
            ParticleSystem destroyParticles = Instantiate(destroyParticlesPrefab, transform.position, Quaternion.identity);
            destroyParticles.Play();
        }
    }

    IEnumerator Errance()
    {
        while (true)
        {
            mouvement = Vector2.zero;
            yield return new WaitForSeconds(Random.Range(0.2f, 1.0f));
            mouvement = Random.insideUnitCircle;
            mouvement.Normalize();
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
        }
    }

    private IEnumerator ShootAtSlime()
    {
        while (estEnChasse) 
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = direction * vitesseProjectile;

            yield return new WaitForSeconds(4f); 

            // When we're done shooting, mark that we're not shooting anymore
            if (!estEnChasse)
            {
                isShooting = false;
            }
        }
    }
}
