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
            if (SaveManager.IsSaveLoaded())
                SaveManager.RestoreFromOpenSave();
            else
                SaveManager.LoadFromDisk();
        }
		/* // implemented in Node itself
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
					Debug.Log("Travelling");
                }
            }
        }*/

        public void Save()
        {
            SaveManager.Save();
        }
    }

}