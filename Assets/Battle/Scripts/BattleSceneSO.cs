using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = "New Battle Scene", menuName = "Battle Scene <--", order = 2)]
public class BattleSceneSO : ScriptableObject, ISequence
{
    [SerializeField]
    [Header("You can add other information about battle to this class.")]
    SceneReference scene;
    [SerializeField]
    EndCondition winCondition;
    [SerializeField]
    EndCondition loseCondition;

    public EndCondition WinCondition => winCondition;
    public EndCondition LoseCondition => loseCondition;

#if UNITY_EDITOR
    void OnValidate() {/*
        if (battleJudge == null)
            return;
        if (!(battleJudge is IBattleJudge)) {
            battleJudge = null;
            Debug.LogWarning("A Judge must implement IBattleJudge interface.");
            return;
        }
        (battleJudge as UnityEditor.MonoScript).GetClass().*/
    }
#endif

    private void OnBattleLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.sceneLoaded -= OnBattleLoaded;

        FindObjectOfType<BattleState>().BattleData = this;
    }

    public void BeginSequence()
    {
        SceneManager.LoadScene("BattleSceneBase", LoadSceneMode.Single);
        SceneManager.LoadScene(scene.ScenePath, LoadSceneMode.Additive);
        SceneManager.sceneLoaded += OnBattleLoaded;
    }



    public bool HasSequenceEnded()
    {
        //this won't get called because new scene will be loaded
        return true;
    }
}
