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
    Sprite portraitArtBase;
    [SerializeField]
    Sprite portraitArtHappy;
    [SerializeField]
    Sprite portraitArtSad;
    [SerializeField]
    Sprite portraitArtEmbarrassed;
    [SerializeField]
    Sprite portraitArtAngry;
	[SerializeField] //Varler added this
    Sprite portraitArtPerplexed; //Varler added this

    public string CharacterName { get => characterName; }
    public string Nickname { get => nickname; }
    public string Description { get => description; }
    public Sprite SplashArt { get => splashArt; }
    public Sprite PortraitArtBase { get => portraitArtBase; }
    public Sprite PortraitArtHappy { get => portraitArtHappy; }
    public Sprite PortraitArtSad { get => portraitArtSad; }
    public Sprite PortraitArtEmbarrassed { get => portraitArtEmbarrassed; }
    public Sprite PortraitArtAngry { get => portraitArtAngry; }
	public Sprite PortraitArtPerplexed {get => portraitArtPerplexed; } //Varler added this

    private void OnEnable()
    {
        var save = JsonUtility.ToJson(this, true);
        System.IO.File.WriteAllText(Application.dataPath + "/save", save);
    }
}
