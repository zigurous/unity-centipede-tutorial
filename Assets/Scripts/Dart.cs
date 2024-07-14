using UnityEngine;

public class Dart : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Transform parent;

    public float speed = 50f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;

        parent = transform.parent;
    }

    private void Update()
    {
        if (rb.isKinematic && Input.GetButton("Fire1"))
        {
            transform.SetParent(null);
            rb.bodyType = RigidbodyType2D.Dynamic;
            boxCollider.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        if (rb.isKinematic) return;

        Vector2 position = rb.position;
        position += speed * Time.fixedDeltaTime * Vector2.up;
        rb.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.SetParent(parent);
        transform.localPosition = new Vector3(0f, 0.5f, 0f);
        rb.bodyType = RigidbodyType2D.Kinematic;
        boxCollider.enabled = false;
    }

}
