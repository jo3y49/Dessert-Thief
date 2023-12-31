using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugRoomManager : WorldManager {
    public Vector2[] spawnLocations;
    private GameObject[] activeEnemies;
    public GameObject enemyPrefab;

    public GameObject tutText;

    private int shopScene = 2;
    private int craftScene = 3;

    private void Awake() {
        activeEnemies = new GameObject[spawnLocations.Length];
    }
    private void Update() {

        if (battleUI.activeSelf) return;
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            for (int i = 0; i < spawnLocations.Length; i++)
            {
                if (activeEnemies[i] == null)
                {
                    GameObject enemy = Instantiate(enemyPrefab);
                    enemy.transform.position = spawnLocations[i];
                    activeEnemies[i] = enemy;
                    break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            shopUI.SetActive(!shopUI.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            bakeUI.SetActive(!bakeUI.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GameManager.instance.SetFoodUses(Enumerable.Repeat(0, GameManager.instance.GetFoodUsesList().Count).ToList());
            playerBattle.RegetUses();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GameManager.instance.AddPlayerMoney(50);
        }
    }

    // public override void EncounterEnemy()
    // {
    //     base.EncounterEnemy();

    //     tutText.SetActive(false);
    // }

    // public override void WinBattle()
    // {
    //     base.WinBattle();

    //     tutText.SetActive(true);
    // }
}