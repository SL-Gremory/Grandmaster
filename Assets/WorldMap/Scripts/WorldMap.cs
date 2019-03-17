using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{

    public class WorldMap : MonoBehaviour
    {
        [SerializeField]
        GameObject player;
        [SerializeField]
        MoveEffect moveEffect;

        Node currentNode;
        
        public static WorldMap Instance { get; private set; }

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("More than one WorldMap running, this shouldn't happen.");
                return;
            }
            Instance = this;
        }

        /// <summary>
        /// Tries to travel to a location, returns true if successful.
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        public bool TravelTo(Node to) {
            if (!moveEffect.IsTraveling() && currentNode.CanTravelTo(to))
            {
                moveEffect.Travel(player, currentNode, to);
                var eventTriggers = currentNode.GetComponentsInChildren<NodeEventTrigger>();
                if (eventTriggers != null) {
                    foreach (var item in eventTriggers)
                    {
                        item.OnTravel(to);
                    }
                }
                currentNode = to;
                SaveManager.UpdateSaveData();
                return true;
            }
            return false;
        }

        public void SetCurrentNode(Node n) {
            currentNode = n;
            moveEffect.InitialPosition(player, currentNode);
        }

        public string CurrentNodeName() {
            return currentNode?.name;
        }

        
    }
}