using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Slime : MonoBehaviour
{
    private Rigidbody2D rig;
    private Vector2 mouvement;
    private bool ouvrir;
    public float vitesse;
    private Animator anim;
    public TMP_Text slimeText;
    public TMP_Text slimeQuest;

    public TMP_Text hint;
    public Image blackScreen;

    private Scene currentScene;
    private Vector2 lastMovementDirection;

    public static int nb_slime = 10;

    public GameObject projectilePrefab;
    public float shootForce = 10f;

    public GameObject[] bridgeObjects;
    public GameObject[] bridgeblockers;

    private bool hasShotInTrigger = false;
    public GameObject[] particleSystems;

    private CircleCollider2D circleCollider;

    private bool isDead = false;
    private float deathAnimDuration = 2f;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
        currentScene = SceneManager.GetActiveScene();
        StartCoroutine(FadeStart());
    }

    void Update()
    {
        slimeText.text = "Slimes: " + nb_slime.ToString();
        slimeQuest.text = bridgePiedestal.slimeOnPlace + "/3";
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        mouvement = new Vector2(horizontalInput, verticalInput).normalized;

        anim.SetFloat("Horizontal", mouvement.x);
        anim.SetFloat("Vertical", mouvement.y);

        float size = Mathf.Clamp(nb_slime / 3, 1, 20);
        transform.localScale = new Vector3(size, size, 1);

        circleCollider.radius = circleCollider.radius;

        Vector2 movementDirection = mouvement.normalized;

        if (movementDirection != Vector2.zero)
        {
            lastMovementDirection = movementDirection;
        }

        if (!isDead && Input.GetMouseButtonDown(0) && nb_slime > 0)
        {
            ShootProjectile();
            nb_slime--;
            hasShotInTrigger = true;
            StartCoroutine(ResetHasShotInTrigger());
            Debug.Log(nb_slime);
        }

        currentScene = SceneManager.GetActiveScene();
        if (nb_slime <= 0 && !isDead)
        {
            size = Mathf.Clamp(nb_slime / 3, 1, 20);
            transform.localScale = new Vector3(size, size, 1);
            isDead = true;
            anim.SetBool("dead", true);
            StartCoroutine(ChangeScene());
            nb_slime = 10;
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
            rig.velocity = Vector2.zero;
            nb_slime--;
            //anim.enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer).Contains("areaBridge"))
        {
            hint.text = "Shoot to build a bridge. cost: 5 slime";
        }
        if (LayerMask.LayerToName(collision.gameObject.layer).Contains("areaBridge") && hasShotInTrigger && nb_slime > 7)
        {
            int bridgeIndex = int.Parse(LayerMask.LayerToName(collision.gameObject.layer).Substring(10)) - 1;
            particleSystems[bridgeIndex].GetComponent<ParticleSystem>().Play();
            bridgeObjects[bridgeIndex].SetActive(true);
            bridgeblockers[bridgeIndex].SetActive(false);
            nb_slime -= 5;
            
            StartCoroutine(ResetHasShotInTrigger());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Ennemi")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            rig.velocity = Vector2.zero;
            //anim.enabled = false;
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

        if (LayerMask.LayerToName(collision.gameObject.layer) == "portal")
        {
            StartCoroutine(ChangeScene());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer).Contains("areaBridge"))
        {
            hasShotInTrigger = false;
            hint.text = "";
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
        yield return new WaitForSeconds(0.5f);
        hasShotInTrigger = false;
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(deathAnimDuration);

        // Fade to black
        for (float t = 0f; t <= 1; t += Time.deltaTime / 2f)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(0, 1, t));
            blackScreen.color = newColor;
            yield return null;
        }

        // Delay before loading the scene
        yield return new WaitForSeconds(2f);

        anim.SetBool("dead", false);
        if (isDead) { SceneManager.LoadScene("Niveau1"); }
        else
        {
            SceneManager.LoadScene("Niveau2");
        }
        isDead = false;

    }

    private IEnumerator FadeStart()
    {
        for (float t = 0f; t <= 1; t += Time.deltaTime / 2f)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(1, 0, t));
            blackScreen.color = newColor;
            yield return null;
        }
    }
    private IEnumerator FadeEnd()
    {
        for (float t = 1f; t <= 1; t -= Time.deltaTime / 2f)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(1, 0, t));
            blackScreen.color = newColor;
            yield return null;
        }
    }
}
