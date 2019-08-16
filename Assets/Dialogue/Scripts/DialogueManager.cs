using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogueManager : MonoBehaviour
{
	public static DialogueManager Instance { get; private set; }

	[SerializeField]
	TextMeshProUGUI namePlateText;
	[SerializeField]
	TextMeshProUGUI speechText;
	[SerializeField]
	Transform splashArtHolder;
	[SerializeField]
	Transform backgroundHolder;
	[SerializeField]
	UnityEngine.UI.Image portrait;

	new AudioSource audio;

	void Awake()
	{
		if (Instance != null)
			Debug.LogError("Can't have multiple DialogueManager instances.");
		Instance = this;
		audio = GetComponent<AudioSource>();
		gameObject.SetActive(false);
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && currConv != null)
			Continue();
	}

	public void EnableBackground(bool en)
	{
		backgroundHolder.gameObject.SetActive(en);
	}

	public void EnableSplashArt(bool en)
	{
		splashArtHolder.gameObject.SetActive(en);
	}

	ConversationSO currConv;
	int speechIndex;
	public bool IsInDialogue { get; private set; }
	List<CharacterSO> characters = new List<CharacterSO>();

	public void StartConversation(ConversationSO conv)
	{
		InitCoversation(conv);
		Continue();
	}

	void InitCoversation(ConversationSO conv)
	{
		currConv = conv;
		speechIndex = -1;
		characters.Clear();
		IsInDialogue = true;
		gameObject.SetActive(true);
		foreach (Transform child in backgroundHolder)
		{
			Destroy(child.gameObject);
		}
		if (conv.BackgroundPefab != null)
		{
			Instantiate(conv.BackgroundPefab, backgroundHolder);
		}
		for (int i = 0; i < splashArtHolder.childCount; i++)
		{
			Destroy(splashArtHolder.GetChild(i).gameObject);
		}
		for (int i = 0; i < conv.Actors.Length; i++)
		{
			ApplySplash(conv.Actors[i].character);
		}
	}

	public void Continue()
	{
		if (speechText.pageToDisplay < speechText.textInfo.pageCount)
		{
			speechText.pageToDisplay += 1;
			OnPageTurned();
			return;
		}
		if (speechIndex + 1 < currConv.Conversation.Length)
		{
			speechText.pageToDisplay = 1;
			speechIndex += 1;
			var charName = currConv.Conversation[speechIndex].speaker;
			namePlateText.text = currConv.Conversation[speechIndex].speaker;
			speechText.text = currConv.Conversation[speechIndex].speech;
			var character = currConv.GetCharacterForName(charName);
			if (character != null)
			{
				ApplyCharacter(character);
			}
			var clip = currConv.VoiceActing[speechIndex];
			audio.Stop();
			if (clip != null)
				audio.PlayOneShot(clip);
		}
		else
		{
			IsInDialogue = false;
			gameObject.SetActive(false);
		}
	}

	void ApplyCharacter(CharacterSO character)
	{
		foreach (var image in splashArtHolder.GetComponentsInChildren<UnityEngine.UI.Image>())
		{
			image.color = new Color(0.3f, 0.3f, 0.3f);
		}
		ApplySplash(character);
		portrait.sprite = character.PortraitArtMain; //TODO emotions
	}

	void ApplySplash(CharacterSO character)
	{
		var ind = characters.IndexOf(character);
		if (ind != -1)
		{
			splashArtHolder.GetChild(ind).GetComponent<UnityEngine.UI.Image>().color = Color.white;
			return;
		}
		characters.Add(character);
		var go = new GameObject("Splash");
		go.transform.SetParent(splashArtHolder, false);
		var image = go.AddComponent<UnityEngine.UI.Image>();
		image.sprite = character.SplashArt;
		image.color = Color.white;
		image.preserveAspect = true;
		//image.SetNativeSize();
	}

	void OnPageTurned()
	{
		//add sound effect when new text appears etc.
	}
}
