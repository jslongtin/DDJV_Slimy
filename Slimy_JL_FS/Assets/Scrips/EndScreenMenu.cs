using System.Collections;
using UnityEngine;

public class EndScreenMenu : MonoBehaviour
{
    public Transform slime;
    public Transform[] monsters;
    public Transform cible;
    public GameObject projectilePrefab;
    public float moveSpeed = 4f;
    public float projectileSpeed = 20f;
    public float runTime = 4f;
    public float minShootInterval = 0.1f;
    public float maxShootInterval = 0.3f;
    public float projectileLifetime = 2f;

    public GameObject[] particleSystemPrefabs;
    public float particleSystemRadius = 5f;
    public float particleSystemInterval = 2f;

    private bool isRunning = true;
    private bool isShooting = false;
    private Vector3 originalSlimePosition;
    private Animator slimeAnimator;

    private void Start()
    {
        originalSlimePosition = slime.position;
        slimeAnimator = slime.GetComponent<Animator>(); // Get the Animator component
        StartCoroutine(AnimateEndScreen());
        StartCoroutine(PlayRandomParticleSystems());
    }

    private IEnumerator AnimateEndScreen()
    {
        while (isRunning)
        {
            // Slime and monsters run from right to left
            float timer = 0f;
            isShooting = false;

            while (timer < runTime)
            {
                slime.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                foreach (Transform monster in monsters)
                {
                    monster.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                    if (monster.localScale.x > 0f)
                    {
                        monster.localScale = new Vector3(-1f, 1f, 1f);
                    }
                }
                timer += Time.deltaTime;

                // Set the walking animation parameter based on the slime's movement direction
                slimeAnimator.SetFloat("Horizontal", 1f);
                yield return null;
            }

            // Slime turns around and shoots projectiles
            slime.localScale = new Vector3(-1f, 1f, 1f);
            isShooting = true;
            ShootProjectiles();

            // Monsters and slime run from left to right
            timer = 0f;
            while (timer < runTime)
            {
                slime.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                foreach (Transform monster in monsters)
                {
                    monster.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                    if (monster.localScale.x < 0f)
                    {
                        monster.localScale = new Vector3(1f, 1f, 1f);
                    }
                }
                timer += Time.deltaTime;

                // Set the walking animation parameter based on the slime's movement direction
                slimeAnimator.SetFloat("Horizontal", -1f);
                yield return null;
            }

            // Reset positions and slime's scale for the next iteration
            ResetPositions();
        }
    }

    private void ShootProjectiles()
    {
        if (isShooting)
        {
            StartCoroutine(ShootProjectilesCoroutine(cible.position));
        }
    }

    private IEnumerator ShootProjectilesCoroutine(Vector3 targetPosition)
    {
        while (isRunning && isShooting)
        {
            float shootInterval = Random.Range(minShootInterval, maxShootInterval);
            yield return new WaitForSeconds(shootInterval);

            Vector3 direction = (targetPosition - slime.position).normalized;
            GameObject newProjectile = Instantiate(projectilePrefab, slime.position, Quaternion.identity);
            Rigidbody2D projectileRigidbody = newProjectile.GetComponent<Rigidbody2D>();
            projectileRigidbody.velocity = direction * projectileSpeed;

            yield return new WaitForSeconds(projectileLifetime);
            Destroy(newProjectile);
        }
    }

    private void ResetPositions()
    {
        foreach (Transform monster in monsters)
        {
            monster.localScale = new Vector3(1f, 1f, 1f);
        }

        slime.position = originalSlimePosition;
    }

    private IEnumerator PlayRandomParticleSystems()
    {
        while (isRunning)
        {
            GameObject particleSystemPrefab = particleSystemPrefabs[Random.Range(0, particleSystemPrefabs.Length)];
            InstantiateParticleSystem(particleSystemPrefab);

            yield return new WaitForSeconds(particleSystemInterval);
        }
    }

    private void InstantiateParticleSystem(GameObject particleSystemPrefab)
    {
        GameObject particleSystemInstance = Instantiate(particleSystemPrefab, GetRandomPositionAroundCamera(), Quaternion.identity);
        particleSystemInstance.transform.parent = transform;
        particleSystemInstance.GetComponent<ParticleSystem>().Play();
    }

    private Vector3 GetRandomPositionAroundCamera()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 randomOffset = Random.insideUnitCircle.normalized * particleSystemRadius;
        return cameraPosition + randomOffset;
    }
}
