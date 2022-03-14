using UnityEngine;

public class MushroomField : MonoBehaviour
{
    private BoxCollider2D area;
    public Mushroom prefab;
    public int amount = 50;

    private void Awake()
    {
        area = GetComponent<BoxCollider2D>();
    }

    public void Generate()
    {
        Clear();

        Bounds bounds = area.bounds;

        for (int i = 0; i < amount; i++)
        {
            Vector2 position = Vector2.zero;

            position.x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
            position.y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));

            Instantiate(prefab, position, Quaternion.identity, transform);
        }
    }

    public void Clear()
    {
        Mushroom[] mushrooms = FindObjectsOfType<Mushroom>();

        for (int i = 0; i < mushrooms.Length; i++) {
            Destroy(mushrooms[i].gameObject);
        }
    }

    public void Heal()
    {
        Mushroom[] mushrooms = FindObjectsOfType<Mushroom>();

        for (int i = 0; i < mushrooms.Length; i++) {
            mushrooms[i].Heal();
        }
    }

}
