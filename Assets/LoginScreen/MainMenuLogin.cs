using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class MainMenuLogin : MonoBehaviour
{
    [SerializeField]
    TMP_InputField nameText;
    [SerializeField]
    TMP_InputField passwordText;
    [SerializeField]
    string worldMapScene;

    public void Login() {
        var name = nameText.text.Trim();
        if (name.Length == 0 || name.Any(c => !char.IsLetterOrDigit(c)))
        {
            for (int i = 0; i < name.Length; i++)
            {
                Debug.Log((int)name[i]);
            }
            Debug.LogError("Name must consist of alphanumeric characters only.");
            return;
        }
        SaveManager.SetSaveFileName(nameText.text);
        SceneManager.LoadScene(worldMapScene, LoadSceneMode.Single);
    }
}
