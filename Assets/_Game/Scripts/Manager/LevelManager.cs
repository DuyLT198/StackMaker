using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public List<Level> levels = new List<Level>();
    public Player player;
    protected Level currentLevel;
    protected int level = 1; 

    private void Start()
    {
        UIManager.Instance.OpenMainMenuUI();
        Load();
    }

    public void Load()
    {
        LoadLevel(level);
        OnInit();
    }

    public void LoadLevel(int indexLevel)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }

        currentLevel = Instantiate(levels[indexLevel - 1]);
    }
    // reset chi so
    public void OnInit() 
    {
        player.transform.position = currentLevel.startPoint.position;
        player.OnInit();
    }

    public void OnStart()
    {
        GameManager.Instance.ChangState(GameState.Gameplay);
    }

    public void OnFinish()
    {
        UIManager.Instance.OpenFinishUI();
        GameManager.Instance.ChangState(GameState.Finish);
    }

    public void NextLevel()
    {
        level++;
        Load();
    }
}
