using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Map
{
    [System.Serializable]
    public struct NodeAndFlag {
        public Node node;
        public string flag;
    }

    [ExecuteInEditMode]
    public class Node : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        List<NodeAndFlag> travelNodes;

		public Region GetRegion() {
			return GetComponentInParent<Region>();
		}

#if UNITY_EDITOR

        private void Update()
        {
            if (!Application.isPlaying)
            {
                foreach (var naf in travelNodes)
                {
                    if (naf.node != null)
                        Debug.DrawLine(transform.position, Vector3.LerpUnclamped(transform.position, naf.node.transform.position, 0.66f), Color.white, 0, false);
                }
            }
        }

#endif

        public bool CanTravelTo(Node to)
        {
            if (to == null)
                return false;
            int index = travelNodes.FindIndex(naf => naf.node == to);
            if (index == -1 || !PlayerFlags.HasFlag(travelNodes[index].flag))
                return false;
            return true;
        }

		public void OnPointerClick(PointerEventData eventData)
		{
			WorldMap.Instance.TravelTo(this);
		}
	}

}