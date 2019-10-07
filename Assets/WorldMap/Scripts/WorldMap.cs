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

		Node currentNode = null;
		Region currentRegion = null;

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
		public bool TravelTo(Node to)
		{
			if (!moveEffect.IsTraveling() && currentNode.CanTravelTo(to))
			{
				moveEffect.Travel(player, currentNode, to);
				var eventTriggers = currentNode.GetComponentsInChildren<NodeEventTrigger>();
				if (eventTriggers != null)
				{
					foreach (var item in eventTriggers)
					{
						item.OnTravel(to);
					}
				}
				currentNode = to;
				currentRegion = to.GetRegion();
				SaveManager.UpdateSaveData();
				return true;
			}
			return false;
		}

		public void SetCurrentNode(Node n)
		{
			if (currentNode != null)
			{
				var eventTriggers = currentNode.GetComponentsInChildren<NodeEventTrigger>();
				if (eventTriggers != null)
				{
					foreach (var item in eventTriggers)
					{
						item.OnTravel(n);
					}
				}
			}
			currentNode = n;
			currentRegion = n.GetRegion();
			moveEffect.InitialPosition(player, currentNode);
			SaveManager.UpdateSaveData();
		}

		public void FakeTravelTo(Node to)
		{
			if (!moveEffect.IsTraveling())
			{
				moveEffect.Travel(player, currentNode, to);
			}
		}

		public string CurrentNodeName()
		{
			return currentNode?.name;
		}

		public Region GetCurrentRegion()
		{
			return currentRegion;
		}

		public Node GetCurrentNode()
		{
			return currentNode;
		}
	}
}