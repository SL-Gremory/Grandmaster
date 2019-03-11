using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Battle Scene", menuName = "Battle Scene <--", order = 2)]
public class BattleSceneSO : ScriptableObject, ISequence
{
    [SerializeField][Header("You can add other information about battle to this class.")]
    SceneReference scene;

    public void BeginSequence()
    {
        SceneManager.LoadScene("BattleSceneBase", LoadSceneMode.Single);
        SceneManager.LoadScene(scene.ScenePath, LoadSceneMode.Additive);
        
    }

    public bool HasSequenceEnded()
    {
        //this won't get called because new scene will be loaded
        return true;
    }
}
