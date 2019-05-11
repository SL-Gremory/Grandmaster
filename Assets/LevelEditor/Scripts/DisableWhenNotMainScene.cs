using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisableWhenNotMainScene : MonoBehaviour
{
    void Awake() {
		if (SceneManager.sceneCount > 1) {
            gameObject.SetActive(false);
			//gameObject.GetComponent<AudioListener>().enabled = false;
        }
    }
}
