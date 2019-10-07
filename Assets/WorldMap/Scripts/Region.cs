using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Map
{

	public class Region : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{

		static Region selected = null;
		static System.Action unselectCallback;

		SVGImage image;

		void Awake()
		{
			image = GetComponent<SVGImage>();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (selected != this && WorldMap.Instance.GetCurrentRegion() != this)
			{
				image.color = Color.Lerp(Color.green, Color.white, 0.7f);
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (selected != this)
			{
				image.color = Color.white;
			}
		}

		void OnSelectOther()
		{
			image.color = Color.white;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			image.color = Color.white;
			if (WorldMap.Instance.GetCurrentRegion() == this) {
				unselectCallback?.Invoke();
				selected = null;
				WorldMap.Instance.FakeTravelTo(WorldMap.Instance.GetCurrentNode());
				return;
			}
			var nodes = GetComponentsInChildren<Node>(true);
			if (nodes == null || nodes.Length == 0)
				return;
			if (selected == this)
			{
				selected = null;
				WorldMap.Instance.SetCurrentNode(GetClosestNode(nodes, WorldMap.Instance.GetCurrentNode().transform.position));
				WorldCameraControl.CanZoom = true;
				WorldCameraControl.ZoomAmount = 2f;
			}
			else
			{
				selected = this;
				unselectCallback?.Invoke();
				unselectCallback = OnSelectOther;
				image.color = Color.green;
				WorldMap.Instance.FakeTravelTo(GetClosestNode(nodes, WorldMap.Instance.GetCurrentNode().transform.position));
				WorldCameraControl.ZoomAmount = 1f;
				WorldCameraControl.CanZoom = false;
			}
		}

		Node GetClosestNode(Node[] nodes, Vector3 position)
		{
			Node closest = null;
			float minSqrDist = float.MaxValue;
			foreach (var item in nodes)
			{
				if (Vector3.SqrMagnitude(item.transform.position - position) < minSqrDist)
				{
					minSqrDist = Vector3.SqrMagnitude(item.transform.position - position);
					closest = item;
				}
			}
			return closest;
		}


	}

}
