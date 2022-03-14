using UnityEngine;

public class CentipedeSegment : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Centipede centipede { get; set; }
    public CentipedeSegment ahead { get; set; }
    public CentipedeSegment behind { get; set; }
    public bool isHead => ahead == null;

    private Vector2 direction = Vector2.right + Vector2.down;
    private Vector2 targetPosition;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPosition = transform.position;
    }

    private void Update()
    {
        // Set the next target position if the segment has reached its target
        if (isHead && Vector2.Distance(transform.position, targetPosition) < 0.1f) {
            UpdateHeadSegment();
        }

        // Move towards the target position
        Vector2 currentPosition = transform.position;
        float speed = centipede.speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed);

        // Rotate the segment to face the direction it is moving
        Vector2 movementDirection = (targetPosition - currentPosition).normalized;
        float angle = Mathf.Atan2(movementDirection.y, movementDirection.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);

        // Set the corresponding sprite
        spriteRenderer.sprite = isHead ? centipede.headSprite : centipede.bodySprite;
    }

    private void UpdateHeadSegment()
    {
        Vector2 gridPosition = GridPosition(transform.position);

        // Calculate the next grid position
        targetPosition = gridPosition;
        targetPosition.x += direction.x;

        // Check if the segment will collide with an object
        if (Physics2D.OverlapBox(targetPosition, Vector2.one * 0.5f, 0f, centipede.collisionMask))
        {
            // Reverse horizontal direction
            direction.x = -direction.x;

            // Advance to the next row
            targetPosition.x = gridPosition.x;
            targetPosition.y = gridPosition.y + direction.y;

            Bounds homeBounds = centipede.homeArea.bounds;

            // Reverse vertical direction if the segment leaves the home area
            if ((direction.y == 1f && targetPosition.y > homeBounds.max.y) ||
                (direction.y == -1f && targetPosition.y < homeBounds.min.y))
            {
                direction.y = -direction.y;
                targetPosition.y = gridPosition.y + direction.y;
            }
        }

        // Update the body segments
        if (behind != null) {
            behind.UpdateBodySegment();
        }
    }

    private void UpdateBodySegment()
    {
        // Follow the segment ahead
        Vector2 nextPosition = GridPosition(ahead.transform.position);
        direction = ahead.direction;

        // Realign the segment to the grid
        targetPosition = GridPosition(transform.position);

        // Offset the target position by the direction to the next segment
        // Follow horizontally first, then vertically
        if (targetPosition.x != nextPosition.x) {
            targetPosition.x = nextPosition.x;
        } else if (targetPosition.y != nextPosition.y) {
            targetPosition.y = nextPosition.y;
        }

        // Update the next body segment
        if (behind != null) {
            behind.UpdateBodySegment();
        }
    }

    private Vector2 GridPosition(Vector2 position)
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        return position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            GameManager.Instance.ResetRound();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Dart") && collision.collider.enabled)
        {
            collision.collider.enabled = false;
            centipede.Remove(this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(targetPosition, Vector3.one);
    }

}
