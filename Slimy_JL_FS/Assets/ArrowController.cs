using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class ArrowGoal
{
    public Transform goalTransform; // Reference to the goal's transform
    public Color arrowColor = Color.white; // Color of the arrow for this goal
}

public class ArrowController : MonoBehaviour
{
    public Transform userTransform; // Reference to the user's transform
    public List<ArrowGoal> arrowGoals; // List of arrow goals with their respective colors

    public GameObject arrowPrefab; // Prefab of the arrow game object

    public float minArrowDistance = 1f; // Minimum distance of the arrow from the user
    public float maxArrowDistance = 3f; // Maximum distance of the arrow from the user
    public float arrowScale = 0.3f; // Scale of the arrows

    public float minOpacity = 0.3f; // Minimum opacity (alpha) for the arrow
    public float maxOpacityDistance = 20f; // Distance at which the arrow reaches maximum opacity

    public GameObject hintToggleBox; // Reference to the hint toggle box GameObject

    private List<GameObject> arrows = new List<GameObject>(); // List to store instantiated arrow game objects

    void Update()
    {
        UpdateArrows();
    }

    void UpdateArrows()
    {
        // Destroy existing arrows
        foreach (GameObject arrow in arrows)
        {
            Destroy(arrow);
        }
        arrows.Clear();

        // Check if the hint toggle box is checked
        bool showArrow = hintToggleBox.GetComponent<Toggle>().isOn;

        if (showArrow)
        {
            // Create new arrows for each goal
            foreach (ArrowGoal arrowGoal in arrowGoals)
            {
                Transform goalTransform = arrowGoal.goalTransform;
                Color arrowColor = arrowGoal.arrowColor;

                // Calculate the direction from user to goal
                Vector3 direction = goalTransform.position - userTransform.position;
                direction.Normalize();

                // Calculate the distance between the user and the goal
                float distanceToGoal = Vector3.Distance(userTransform.position, goalTransform.position);

                // Calculate the arrow distance based on the user's proximity to the goal
                float arrowDistance = Mathf.Lerp(minArrowDistance, maxArrowDistance, distanceToGoal / maxOpacityDistance);

                // Clamp the arrow distance to the maximum value
                arrowDistance = Mathf.Min(arrowDistance, maxArrowDistance);

                // Calculate the position of the arrow relative to the user
                Vector3 arrowPosition = userTransform.position + direction * arrowDistance;

                // Calculate the distance between the arrow and the goal
                float distanceFromGoal = Vector3.Distance(arrowPosition, goalTransform.position);

                // Calculate the opacity based on the distance
                float t = Mathf.InverseLerp(maxOpacityDistance, 0f, distanceFromGoal);
                float currentOpacity = Mathf.Lerp(minOpacity, 1f, t);

                // Instantiate an arrow game object
                GameObject arrow = Instantiate(arrowPrefab, arrowPosition, Quaternion.identity);
                arrows.Add(arrow);

                // Calculate the angle between the direction and the forward vector
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Apply the rotation and scale to the arrow game object
                arrow.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                arrow.transform.localScale = new Vector3(arrowScale, arrowScale, 1f);

                // Apply the color and opacity to the arrow sprite renderer
                SpriteRenderer spriteRenderer = arrow.GetComponent<SpriteRenderer>();
                Color currentColor = arrowColor;
                currentColor.a = currentOpacity;
                spriteRenderer.color = currentColor;
            }
        }
    }
}
