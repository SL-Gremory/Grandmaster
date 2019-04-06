using UnityEngine;

public class RotateAroundPoint : MonoBehaviour
{
    [SerializeField]
    Vector3 pos;

    void Update()
    {
        var move = -Input.GetAxis("Horizontal");
        transform.RotateAround(pos, Vector3.up, move * 100f * Time.deltaTime);
    }
}
