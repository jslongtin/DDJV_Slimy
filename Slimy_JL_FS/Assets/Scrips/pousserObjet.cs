
using UnityEngine;

public class pousserObjet: MonoBehaviour
{
    private GameObject go;
    private Vector3 offset;
    private BoxCollider2D boxCollider;
    private Vector3 lastMousePos;
    private float mouseVelocity;

    public float throwForce = 100f;
    public float maxMouseVelocity = 100f;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.offset = new Vector2(0f, 0f); // Set the offset to the center of the object
    }

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D target = Physics2D.OverlapPoint(mousePos);

            if (target && target.gameObject == gameObject) // Only select this object if it's the one being clicked on
            {
                go = target.transform.gameObject;
                offset = go.transform.position - mousePos;
                lastMousePos = mousePos;
            }

        }
        else if (Input.GetMouseButton(0))
        {
            if (go)
            {
                go.transform.position = mousePos + offset;
                mouseVelocity = (mousePos - lastMousePos).magnitude / Time.deltaTime;
                lastMousePos = mousePos;
            }
        }
        else if (Input.GetMouseButtonUp(0) && go)
        {
            go.GetComponent<Rigidbody2D>().AddForce((mousePos - lastMousePos) * Mathf.Clamp(mouseVelocity, 0f, maxMouseVelocity) * throwForce);
            go = null;
            mouseVelocity = 0f;
        }
       
    }
}
