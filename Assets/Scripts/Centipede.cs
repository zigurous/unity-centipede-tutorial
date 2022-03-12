using System.Collections.Generic;
using UnityEngine;

public class Centipede : MonoBehaviour
{
    private List<CentipedeSegment> segments;
    private float nextUpdate;

    [Header("Prefabs")]
    public CentipedeSegment segmentPrefab;
    public Mushroom mushroomPrefab;

    [Header("Sprites")]
    public Sprite headSprite;
    public Sprite bodySprite;

    [Header("Movement")]
    public LayerMask collisionMask = ~0;
    public Collider2D homeArea;
    public float speed = 16f;
    public int size = 12;

    [Header("Scoring")]
    public int pointsHead = 100;
    public int pointsBody = 10;

    private void Awake()
    {
        segments = new List<CentipedeSegment>();
    }

    public void Respawn()
    {
        foreach (CentipedeSegment segment in segments) {
            Destroy(segment.gameObject);
        }

        segments.Clear();

        for (int i = 0; i < size; i++)
        {
            CentipedeSegment segment = Instantiate(segmentPrefab, transform.position, Quaternion.identity);
            segment.centipede = this;
            segments.Add(segment);
        }

        for (int i = 0; i < size; i++)
        {
            if (i > 0) {
                segments[i].previous = segments[i - 1];
            }

            if (i < size - 1) {
                segments[i].next = segments[i + 1];
            }
        }
    }

    private void Update()
    {
        if (Time.time < nextUpdate) {
            return;
        }

        for (int i = segments.Count - 1; i >= 0; i--) {
            segments[i].Move();
        }

        nextUpdate = Time.time + (1f / speed);
    }

    public void Remove(CentipedeSegment segment)
    {
        int points = segment.isHead ? pointsHead : pointsBody;
        GameManager.Instance.IncreaseScore(points);

        if (segment.previous != null) {
            segment.previous.next = null;
        }

        if (segment.next != null) {
            segment.next.previous = null;
        }

        segments.Remove(segment);

        Instantiate(mushroomPrefab, segment.transform.position, Quaternion.identity);
        Destroy(segment.gameObject);

        if (segments.Count == 0) {
            GameManager.Instance.NextLevel();
        }
    }

}
