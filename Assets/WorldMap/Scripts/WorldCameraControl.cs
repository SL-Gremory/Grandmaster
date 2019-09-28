using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class WorldCameraControl : MonoBehaviour
{
	[SerializeField]
	float referenceHeight = 1080;
	[SerializeField]
	float pixelsPerUnit = 32f;
	[SerializeField]
	float maxZoom = 12f;
	[SerializeField]
	int showRegionAtScale = 3;
	[SerializeField]
	float scrollSpeed = 0.2f;

	[SerializeField]
	CanvasGroup[] worldSprites;
	[SerializeField]
	CanvasGroup[] regions;
	[SerializeField]
	Map.Region[] regionSelectors; // to enable/disable region change

	CinemachineVirtualCamera cam;
	CinemachineCameraOffset offset;
	float oldScale = 1f;
	public static float ZoomAmount = 1f;
	public static bool CanZoom { get; set; } = true;

	Coroutine regionC;
	Coroutine worldC;
	Coroutine zoomC;

	void Start()
	{
		cam = GetComponent<CinemachineVirtualCamera>();
		offset = GetComponent<CinemachineCameraOffset>();
		worldC = StartCoroutine(SetWorld(true));
		regionC = StartCoroutine(SetRegions(false));
	}

	// Update is called once per frame
	void Update()
	{
		var mousePos = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition) * 2f - Vector2.one;
		if (mousePos.magnitude > 0.75f)
		{
			var camSize = cam.m_Lens.OrthographicSize;
			var scroll = scrollSpeed * camSize;
			offset.m_Offset.x += mousePos.x * Time.deltaTime * scroll;
			offset.m_Offset.y += mousePos.y * Time.deltaTime * scroll;
			offset.m_Offset = Vector3.ClampMagnitude(offset.m_Offset, camSize * 0.8f);
		}

		if (CanZoom)
		{
			ZoomAmount += Input.mouseScrollDelta.y;
			ZoomAmount = Mathf.Clamp(ZoomAmount, 1f, maxZoom);

			var scale = Mathf.Max(1f, Mathf.Round(ZoomAmount/* * (Screen.height / referenceHeight)*/));

			if (scale >= showRegionAtScale && oldScale < showRegionAtScale)
			{
				if (worldC != null)
					StopCoroutine(worldC);
				if (regionC != null)
					StopCoroutine(regionC);
				regionC = StartCoroutine(SetRegions(true));
				worldC = StartCoroutine(SetWorld(false));
			}
			else if (scale < showRegionAtScale && oldScale >= showRegionAtScale)
			{
				if (worldC != null)
					StopCoroutine(worldC);
				if (regionC != null)
					StopCoroutine(regionC);
				regionC = StartCoroutine(SetRegions(false));
				worldC = StartCoroutine(SetWorld(true));
			}

			if (oldScale != scale)
			{
				if (zoomC != null)
					StopCoroutine(zoomC);
				zoomC = StartCoroutine(ZoomSmoothly(oldScale, scale));
			}
			oldScale = scale;
		}
	}

	IEnumerator ZoomSmoothly(float oldScale, float newScale)
	{
		var oldHalf = cam.m_Lens.OrthographicSize;
		var newHalf = Screen.height / (2f * pixelsPerUnit * newScale);

		float timeTaken = 0f;
		while (timeTaken < 1f)
		{
			cam.m_Lens.OrthographicSize = Mathf.Lerp(oldHalf, newHalf, timeTaken);

			timeTaken += Time.deltaTime;
			yield return null;
		}

		cam.m_Lens.OrthographicSize = newHalf;
	}

	IEnumerator SetRegions(bool enableRegions)
	{

		float startAlpha = enableRegions ? 0 : 1;
		float targetAlpha = enableRegions ? 1 : 0;

		if (enableRegions)
		{
			foreach (var item in regions)
			{
				item.gameObject.SetActive(true);
			}
		}

		var timeTaken = 0f;
		while (timeTaken < 1f)
		{
			timeTaken += Time.deltaTime;
			foreach (var item in regions)
			{
				item.alpha = Mathf.Lerp(startAlpha, targetAlpha, timeTaken);
			}
			yield return null;
		}
		if (!enableRegions)
		{
			foreach (var item in regions)
			{
				item.gameObject.SetActive(false);
			}
		}
	}

	IEnumerator SetWorld(bool enableWorld)
	{
		foreach (var item in regionSelectors)
		{
			item.enabled = enableWorld;
		}

		float startAlpha = enableWorld ? 0 : 1;
		float targetAlpha = enableWorld ? 1 : 0;

		if (enableWorld)
		{
			foreach (var item in worldSprites)
			{
				item.gameObject.SetActive(true);
			}
		}

		var timeTaken = 0f;
		while (timeTaken < 1f)
		{
			timeTaken += Time.deltaTime;
			foreach (var item in worldSprites)
			{
				item.alpha = Mathf.Lerp(startAlpha, targetAlpha, timeTaken);
			}
			yield return null;
		}
		if (!enableWorld)
		{
			foreach (var item in worldSprites)
			{
				item.gameObject.SetActive(false);
			}
		}
	}
}
