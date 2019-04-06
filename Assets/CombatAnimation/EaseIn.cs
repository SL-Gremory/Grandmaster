using UnityEngine;

public class EaseIn : MonoBehaviour
{
    [SerializeField]
    [Range(1f, 5f)]
    float easeSpeedMultiplier = 1f;
    Vector3 startPosition;
    public float EaseSpeedMultiplier { get { return easeSpeedMultiplier; } }

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        transform.position = startPosition;
    }

    public float MaxX() {
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        if (sprites == null)
            return transform.position.x;
        float max = transform.position.x;
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].bounds.max.x > max)
                max = sprites[i].bounds.max.x;
        }
        return max;
    }

    public float MinX()
    {
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        if (sprites == null)
            return transform.position.x;
        float min = transform.position.x;
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].bounds.min.x < min)
                min = sprites[i].bounds.min.x;
        }
        return min;
    }
}
