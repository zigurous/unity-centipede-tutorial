using UnityEngine;

public class Dart : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;
    private Transform parent;
    public float speed = 50f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.bodyType = RigidbodyType2D.Kinematic;

        collider = GetComponent<Collider2D>();
        collider.enabled = false;

        parent = transform.parent;
    }

    private void Update()
    {
        if (rigidbody.isKinematic && Input.GetButton("Fire1"))
        {
            transform.SetParent(null);
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            collider.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        if (!rigidbody.isKinematic)
        {
            Vector2 position = rigidbody.position;
            position += Vector2.up * speed * Time.fixedDeltaTime;
            rigidbody.MovePosition(position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.SetParent(parent);
        transform.localPosition = new Vector3(0f, 0.5f, 0f);
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        collider.enabled = false;
    }

}
