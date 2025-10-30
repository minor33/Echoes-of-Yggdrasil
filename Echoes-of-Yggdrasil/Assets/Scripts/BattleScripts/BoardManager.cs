using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }

    public GameObject enemyPrefab;
    private GameObject[] enemies;
    public Transform[] boardSlots;

    public async void executeActions() {
        await Awaitable.WaitForSecondsAsync(0.15f);
        for(int i = 0; i < 3; i++){
            if(enemies[i] != null){
                enemies[i].GetComponent<Enemy>().executeAction();
                await Awaitable.WaitForSecondsAsync(0.35f);
            }
        }
        BattleManager.Instance.progressBattleState();
    }

    public void displayActions() {
        for(int i = 0; i < 3; i++){
            if(enemies[i] != null){
                enemies[i].GetComponent<Enemy>().displayAction();
            }
        }
    }

    public Enemy getFrontEnemy() {
        for(int i = 0; i < 3; i++){
            if(enemies[i] != null){
                return enemies[i].GetComponent<Enemy>();
            }
        }
        return null;
    }

    public Enemy getRandomEnemy(){
        List<Enemy> activeEnemies = new List<Enemy>();
        for(int i = 0; i < 3; i++){
            if(enemies[i] != null){
                activeEnemies.Add(enemies[i].GetComponent<Enemy>());
            }
        }
        if(activeEnemies.Count == 0){
            return null;
        }
        int index = Random.Range(0,activeEnemies.Count);
        return activeEnemies[index].GetComponent<Enemy>();
    }

    public Enemy getEnemy(int index) {
        if(enemies[index] == null){
            return null;
        }
        return enemies[index].GetComponent<Enemy>();
    }

    public void setEnemy(EnemyData enemyData, int slot) {
        GameObject enemy = Instantiate(enemyPrefab, boardSlots[slot].transform.position, Quaternion.identity,  boardSlots[slot].transform);
        enemy.GetComponent<Enemy>().enemyData = enemyData;
        enemies[slot] = enemy;    
    }

    void Start() {
        enemies = new GameObject[3];
    }

    void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
