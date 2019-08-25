using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateCombatBackgrounds : MonoBehaviour
{
	[SerializeField]
	protected float animationLength = 5f;

	RectTransform[] rects;

	private void Awake()
	{
		rects = GetComponentsInChildren<RectTransform>();
		InitRects(true);
	}

	public void AnimateIn()
	{
		StartCoroutine(Animate(true));
	}

	public void AnimateOut()
	{
		StartCoroutine(Animate(false));
	}

	void InitRects(bool animateIn) {
		foreach (var rect in rects)
		{
			var pivot = rect.pivot;
			pivot.x = animateIn ? 1f : 0f;
			rect.pivot = pivot;
			var pos = rect.anchoredPosition;
			pos.x = 0f;
			rect.anchoredPosition = pos;
		}
	}

	IEnumerator Animate(bool animateIn)
	{
		InitRects(animateIn);
		
		float startTime = Time.time;
		var parent = GetComponent<RectTransform>();
		while (startTime + animationLength > Time.time)
		{
			var fraction = (Time.time - startTime) / animationLength;
			foreach (var rect in rects)
			{
				var pos = rect.anchoredPosition;
				pos.x = Mathf.Lerp(0f, rect.rect.size.x - parent.rect.size.x, fraction);
				rect.anchoredPosition = pos;
			}
			yield return null;
		}
	}
}
