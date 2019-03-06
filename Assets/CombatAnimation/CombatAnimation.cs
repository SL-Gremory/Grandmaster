using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class CombatAnimation : MonoBehaviour
{
    [SerializeField]
    AnimationCurve easeCurve;
    [SerializeField]
    [Min(0.1f)]
    float totalTime;
    [SerializeField]
    EaseIn[] layers;
    [SerializeField]
    Camera cam;
    [SerializeField]
    float referenceHeight = 1080;
    [SerializeField]
    float normalScale = 2f;
    [SerializeField]
    float pixelsPerUnit = 32f;
    [SerializeField]
    [Range(-0.5f, 0.5f)]
    float scaleOffset;

    Coroutine running;

    private void Awake()
    {
        foreach (var layer in layers)
        {
            foreach (var tr in layer.GetComponentsInChildren<Transform>())
            {
                tr.position -= new Vector3(tr.position.x % (1f / pixelsPerUnit), tr.position.y % (1f / pixelsPerUnit), 0);
            }
        }
    }

    private void Update()
    {
        var scale = Mathf.Round(normalScale * (Screen.height / referenceHeight) + scaleOffset);
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].transform.localScale = Vector3.one * scale;
        }
        var halfHeight = Screen.height / (2f * pixelsPerUnit);
        cam.orthographicSize = halfHeight;
    }

    public void AnimateIn()
    {
        if (running != null)
            StopCoroutine(running);
        running = StartCoroutine(AnimateInCoroutine());
    }

    public void AnimateOut()
    {
        if (running != null)
            StopCoroutine(running);
        running = StartCoroutine(AnimateOutCoroutine());
    }

    IEnumerator AnimateInCoroutine()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].transform.position = new Vector3(transform.position.x, transform.position.y, layers[i].transform.position.z);
        }
        float startTime = Time.time;
        Vector3 startPos = cam.ViewportToWorldPoint(Vector3.up / 2f);
        startPos = startPos + new Vector3(transform.position.x - CalculateMaxX(), 0, 0);
        while (startTime + totalTime > Time.time)
        {
            float currTime = Time.time - startTime;
            for (int i = 0; i < layers.Length; i++)
            {
                var multipliedTime = currTime * layers[i].EaseSpeedMultiplier;
                if (multipliedTime <= totalTime)
                {
                    var z = layers[i].transform.position.z;
                    layers[i].transform.position
                        = Vector3.Lerp(startPos, transform.position, easeCurve.Evaluate(multipliedTime / totalTime));
                    layers[i].transform.position = new Vector3(layers[i].transform.position.x, layers[i].transform.position.y, z);
                }
            }
            yield return null;
        }
    }

    IEnumerator AnimateOutCoroutine()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].transform.position = new Vector3(transform.position.x, transform.position.y, layers[i].transform.position.z);
        }
        float startTime = Time.time;
        Vector3 endPos = cam.ViewportToWorldPoint(Vector3.up / 2f + Vector3.right);
        endPos = endPos + new Vector3(transform.position.x - CalculateMinX(), 0, 0);
        while (startTime + totalTime > Time.time)
        {
            float currTime = Time.time - startTime;
            for (int i = 0; i < layers.Length; i++)
            {
                var multipliedTime = currTime * layers[i].EaseSpeedMultiplier;
                if (multipliedTime <= totalTime)
                {
                    var z = layers[i].transform.position.z;
                    layers[i].transform.position
                        = Vector3.Lerp(transform.position, endPos, 1f - easeCurve.Evaluate(1f - multipliedTime / totalTime));
                    layers[i].transform.position = new Vector3(layers[i].transform.position.x, layers[i].transform.position.y, z);
                }
            }
            yield return null;
        }
    }

    float CalculateMaxX()
    {
        float max = transform.position.x;
        for (int i = 0; i < layers.Length; i++)
        {
            var currMax = layers[i].MaxX();
            if (currMax > max)
                max = currMax;
        }
        return max;

    }

    float CalculateMinX()
    {
        float min = transform.position.x;
        for (int i = 0; i < layers.Length; i++)
        {
            var currMin = layers[i].MinX();
            if (currMin < min)
                min = currMin;
        }
        return min;

    }
}
