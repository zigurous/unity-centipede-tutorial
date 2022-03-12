using UnityEngine;

public class Blaster : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private Vector2 direction;
    private Vector2 spawnPosition;
    public float speed = 20f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spawnPosition = transform.position;
    }

    private void Update()
    {
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        position += direction.normalized * speed * Time.fixedDeltaTime;
        rigidbody.MovePosition(position);
    }

    public void Respawn()
    {
        transform.position = spawnPosition;
        gameObject.SetActive(true);
    }

}
