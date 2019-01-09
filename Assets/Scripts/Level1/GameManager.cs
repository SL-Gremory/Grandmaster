using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Make game manager singleton
    public static GameManager instance = null;

    public BoardManager boardScript;



    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Keeps track of information between scenes (levels)
        DontDestroyOnLoad(gameObject);


        boardScript = GetComponent<BoardManager>();
        InitGame();

    }

    void InitGame()
    {
        boardScript.SetupScene(1);
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
