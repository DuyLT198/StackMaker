using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { MainMenu, Gameplay, Finish}
public class GameManager : Singleton<GameManager>
{
    private GameState state;

    // public static GameManager instance;
    void Awake()
    {
        //Setup tong quan game

        // Setup data
        // instance = this;
        ChangState(GameState.MainMenu);
    }

    public void ChangState(GameState gameState)
    {
        state = gameState;
    }

    public bool IsState(GameState gameState)
    {
        return state == gameState;
    }
}
