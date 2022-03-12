using UnityEngine;

public class CentipedeSegment : MonoBehaviour
{
    public Centipede centipede { get; set; }
    public CentipedeSegment next { get; set; }
    public CentipedeSegment previous { get; set; }
    public bool isHead => previous == null;

    private SpriteRenderer spriteRenderer;
    private Vector2 direction = Vector2.one;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Move()
    {
        if (isHead) {
            MoveHead();
        } else {
            MoveBody();
        }
    }

    private void MoveBody()
    {
        direction = previous.direction;
        transform.position = previous.transform.position;
        transform.rotation = previous.transform.rotation;
        spriteRenderer.sprite = centipede.bodySprite;
    }

    private void MoveHead()
    {
        Vector3 position = GetPosition(direction.x, 0f);
        Quaternion rotation = GetRotation(direction.x, 0f);

        if (Physics2D.OverlapBox(position, Vector3.one * 0.5f, 0f, centipede.collisionMask))
        {
            direction.x = -direction.x;
            position = GetPosition(0f, direction.y);

            Bounds homeBounds = centipede.homeArea.bounds;

            if ((direction.y == 1f && position.y > homeBounds.max.y) ||
                (direction.y == -1f && position.y < homeBounds.min.y))
            {
                direction.y = -direction.y;
                position = GetPosition(0f, direction.y);
            }

            rotation = GetRotation(0f, direction.y);
        }

        transform.position = position;
        transform.rotation = rotation;
        spriteRenderer.sprite = centipede.headSprite;
    }

    private Vector2 GetPosition(float directionX, float directionY)
    {
        Vector2 position = transform.position;
        position.x += directionX;
        position.y += directionY;
        return position;
    }

    private Quaternion GetRotation(float directionX, float directionY)
    {
        float angle = Mathf.Atan2(directionY, directionX);
        return Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Dart")) {
            centipede.Remove(this);
        } else if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            GameManager.Instance.ResetRound();
        }
    }

}
