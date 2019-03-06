using System.Collections;
using UnityEngine;

namespace Map
{

    public class MoveEffect : MonoBehaviour
    {
        [SerializeField]
        float travelTime = 3f;

        Coroutine current;
        bool isRunning;

        public void InitialPosition(GameObject player, Node init)
        {
            player.transform.position = init.transform.position;
        }

        public bool IsTraveling()
        {
            return isRunning;
        }

        public void Travel(GameObject player, Node from, Node to)
        {
            if (current != null)
                StopCoroutine(current);
            current = StartCoroutine(MovePlayerCoroutine(player, from, to));
        }

        IEnumerator MovePlayerCoroutine(GameObject player, Node from, Node to)
        {
            isRunning = true;
            var startTime = Time.time;
            while (startTime + travelTime > Time.time)
            {
                player.transform.position = Vector3.Lerp(from.transform.position, to.transform.position, (Time.time - startTime) / travelTime);
                yield return null;
            }
            isRunning = false;
        }
    }
}