using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character <---", order = 0)]
public class CharacterSO : ScriptableObject
{
    [SerializeField]
    string characterName;
    [SerializeField]
    string nickname;
    [SerializeField]
    [Multiline]
    string description;
    [SerializeField]
    Sprite splashArt;
    [SerializeField]
    Sprite portraitArtMain;
    [SerializeField]
    Sprite portraitArtHappy;
    [SerializeField]
    Sprite portraitArtSad;
    [SerializeField]
    Sprite portraitArtEmbarrassed;
    [SerializeField]
    Sprite portraitArtAngry;

    public string CharacterName { get => characterName; }
    public string Nickname { get => nickname; }
    public string Description { get => description; }
    public Sprite SplashArt { get => splashArt; }
    public Sprite PortraitArtMain { get => portraitArtMain; }
    public Sprite PortraitArtHappy { get => portraitArtHappy; }
    public Sprite PortraitArtSad { get => portraitArtSad; }
    public Sprite PortraitArtEmbarrassed { get => portraitArtEmbarrassed; }
    public Sprite PortraitArtAngry { get => portraitArtAngry; }

    private void OnEnable()
    {
        var save = JsonUtility.ToJson(this, true);
        System.IO.File.WriteAllText(Application.dataPath + "/save", save);
    }
}
