using System.Collections;
using UnityEngine;

public class BounceSlime : MonoBehaviour
{
    public float bounceHeight = 1.0f;
    public float bounceDuration = 1.0f;

    private Vector2 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
        StartCoroutine(BounceCoroutine());
    }

    private IEnumerator BounceCoroutine()
    {
        while (true)
        {
            // Move up
            Vector2 targetPosition = originalPosition + new Vector2(0f, bounceHeight);
            yield return MoveToPosition(targetPosition, bounceDuration / 2f);

            // Move down
            yield return MoveToPosition(originalPosition, bounceDuration / 2f);
        }
    }

    private IEnumerator MoveToPosition(Vector2 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector2 startingPosition = transform.position;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector2.Lerp(startingPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

}

