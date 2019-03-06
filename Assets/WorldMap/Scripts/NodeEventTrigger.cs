using UnityEngine;
using UnityEngine.Events;

namespace Map
{
    [RequireComponent(typeof(Node))]
    public class NodeEventTrigger : MonoBehaviour
    {
        [SerializeField]
        Node destination;
        [SerializeField]
        string hasFlag;
        [SerializeField]
        UnityEvent onTravelTo;
        

        public UnityEvent OnTravelTo { get { return onTravelTo; } }

        public void OnTravel(Node to) {
            if (to == destination && PlayerFlags.HasFlag(hasFlag))
                onTravelTo.Invoke();
        }
    }
}