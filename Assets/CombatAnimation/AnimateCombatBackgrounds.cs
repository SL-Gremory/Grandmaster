using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateCombatBackgrounds : MonoBehaviour
{
	[SerializeField]
	protected float letterBoxingLength = 1f;
	[SerializeField]
	protected float animationLength = 5f;
	[SerializeField]
	protected AnimationCurve easingCurve;
	[SerializeField]
	protected RectTransform[] rects;

	private void Awake()
	{
		if (rects.Length == 0)
			rects = GetComponentsInChildren<RectTransform>();
		InitBeforeOpening();
	}

	public void AnimateIn()
	{
		StopAllCoroutines();
		InitBeforeOpening();
		StartCoroutine(AnimateInScenery());
		StartCoroutine(AnimateInLetterbox());
	}

	public void AnimateOut()
	{
		StopAllCoroutines();
		StartCoroutine(AnimateOutCoroutine());
	}

	void InitBeforeOpening()
	{
		var rectsParent = GetComponent<RectTransform>();
		var parentsParent = transform.parent.GetComponent<RectTransform>();
		var sizeDelta = rectsParent.sizeDelta;
		sizeDelta.y = -parentsParent.sizeDelta.y;
		rectsParent.sizeDelta = sizeDelta;

		foreach (var rect in rects)
		{
			var pivot = rect.pivot;
			pivot.x = 1f;
			rect.pivot = pivot;
			var pos = rect.anchoredPosition;
			pos.x = 0f;
			rect.anchoredPosition = pos;
		}
	}

	IEnumerator AnimateInLetterbox()
	{
		var rectsParent = GetComponent<RectTransform>();
		var parentsParent = transform.parent.GetComponent<RectTransform>();
		float startTime = Time.time;
		while (startTime + letterBoxingLength > Time.time)
		{
			var fraction = (Time.time - startTime) / letterBoxingLength;
			var sizeDelta = rectsParent.sizeDelta;
			sizeDelta.y = Mathf.Lerp(-parentsParent.sizeDelta.y, 0f, fraction);
			rectsParent.sizeDelta = sizeDelta;
			yield return null;
		}
		{ //to force the last lerp value
			var fraction = (Time.time - startTime) / letterBoxingLength;
			var sizeDelta = rectsParent.sizeDelta;
			sizeDelta.y = 0f;
			rectsParent.sizeDelta = sizeDelta;
		}
	}

	IEnumerator AnimateInScenery()
	{

		var rectsParent = GetComponent<RectTransform>();

		float startTime = Time.time;
		while (startTime + animationLength > Time.time)
		{
			var fraction = (Time.time - startTime) / animationLength;
			fraction = easingCurve.Evaluate(fraction);
			foreach (var rect in rects)
			{
				var pos = rect.anchoredPosition;
				pos.x = Mathf.Lerp(0f, rect.rect.size.x - rectsParent.rect.size.x, fraction);
				rect.anchoredPosition = pos;
			}
			yield return null;
		}
		{ //to force the last lerp value
			foreach (var rect in rects)
			{
				var pos = rect.anchoredPosition;
				pos.x = rect.rect.size.x - rectsParent.rect.size.x;
				rect.anchoredPosition = pos;
			}
		}
	}

	IEnumerator AnimateOutCoroutine()
	{
		var rectsParent = GetComponent<RectTransform>();
		var parentsParent = transform.parent.GetComponent<RectTransform>();
		float startTime = Time.time;
		while (startTime + letterBoxingLength > Time.time)
		{
			var fraction = (Time.time - startTime) / letterBoxingLength;
			var sizeDelta = rectsParent.sizeDelta;
			sizeDelta.y = Mathf.Lerp(0f, -parentsParent.sizeDelta.y, fraction);
			rectsParent.sizeDelta = sizeDelta;
			yield return null;
		}

		{ //to force the last lerp value
			var fraction = (Time.time - startTime) / letterBoxingLength;
			var sizeDelta = rectsParent.sizeDelta;
			sizeDelta.y = -parentsParent.sizeDelta.y;
			rectsParent.sizeDelta = sizeDelta;
		}
	}
}
