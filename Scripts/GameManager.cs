using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    menu,
    inGame,
    gameOver
}
public class GameManager : MonoBehaviour
{

    public GameState currentGameState = GameState.menu;
    public static GameManager sharedInstance;

    private PlayerController controller;

    public int collectedObject = 0;

    private void Awake()
    {
        if (sharedInstance == null) {
            sharedInstance = this;

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        ////get key s
        if (Input.GetButtonDown("Submit")&&
            currentGameState != GameState.inGame)
        {
            StartGame();
        }
    }
    public void StartGame()
    {
        SetGameState(GameState.inGame);
    }
    public void GameOver()
    {
        SetGameState(GameState.gameOver);

    }
    public void BackToMenu()
    {
        SetGameState(GameState.menu);

    }
    private void SetGameState(GameState newGameState)
    {
        if (newGameState == GameState.menu)
        {
            MenuManager.sharedInstance.ShowMainMenu();
            MenuManager.sharedInstance.HideScore();
           MenuManager.sharedInstance.HideOver();


            //logica delmenu
        }
        else if(newGameState==GameState.inGame)
        {
            //preparar eschena para jugar
            LevelManager.sharedInstance.RemoveAllLevelBlocks();
            LevelManager.sharedInstance.GenerateInitialBlocks();
            controller.StartGame();
            MenuManager.sharedInstance.HideMainMenu();
            MenuManager.sharedInstance.HideOver();
            MenuManager.sharedInstance.ShowScore();
                

        }
        else if (newGameState==GameState.gameOver)
        {
            //preparar el juego para game over
            MenuManager.sharedInstance.HideMainMenu();
            MenuManager.sharedInstance.ShowOver();

        }
        this.currentGameState = newGameState;
    }

    public void CollectObject(Collectable collectable)
    {
        collectedObject += collectable.value;
    }
}
