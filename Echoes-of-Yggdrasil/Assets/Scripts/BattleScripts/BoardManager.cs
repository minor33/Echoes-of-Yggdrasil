using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }

    public GameObject enemyPrefab;
    private GameObject[] enemies;
    public Transform[] boardSlots;

    public Enemy getFrontEnemy() {
        for(int i = 0; i < 3; i++){
            if(enemies[i] != null){
                return enemies[i].GetComponent<Enemy>();
            }
        }
        return null;
    }

    public Enemy getRandomEnemy(){
        int numEnemies = 0;
        for(int i = 0; i < 3; i++){
            if(enemies[i] != null){
                numEnemies++;
            }
        }
        if(numEnemies == 0){
            return null;
        }
        int index = Random.Range(0,numEnemies);
        while(enemies[index] == null){
            index++;
        }
        return enemies[index].GetComponent<Enemy>();
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
