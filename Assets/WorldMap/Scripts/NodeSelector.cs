using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class NodeSelector : MonoBehaviour
    {
        [SerializeField]
        Node startNode;

        private void Start()
        {
            WorldMap.Instance.SetCurrentNode(startNode);
            SaveManager.Load();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var col = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (col == null)
                    return;
                var node = col.GetComponentInChildren<Node>();
                if (node != null)
                {
                    WorldMap.Instance.TravelTo(node);

                }
            }
        }

        public void Save() {
            SaveManager.Save();
        }
    }

}