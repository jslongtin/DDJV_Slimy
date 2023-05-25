using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 5f;

    // Reference to the particle system prefab
    public ParticleSystem destroyParticlesPrefab;

    void Start()
    {
        StartCoroutine(DestroyAfterTime(lifetime));
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Instantiate the particle system at the projectile's location
        ParticleSystem destroyParticles = Instantiate(destroyParticlesPrefab, transform.position, Quaternion.identity);
        destroyParticles.Play();

        // Destroy the projectile
        Destroy(gameObject);
    }
}
