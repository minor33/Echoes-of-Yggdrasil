using UnityEngine;
using System.Collections.Generic;

public class BoardDisplay : MonoBehaviour
{
    public static BoardDisplay Instance { get; private set; }

    public GameObject enemyPrefab;
    private GameObject[] enemies;
    public Transform[] boardSlots;

    public Enemy getEnemy(int index = 0) {
        for(int i = 0; i < 3; i++) { 
            // Change > 0 line
            if (enemies[(index+i)%3] != null) {
                return enemies[(index+i)%3].GetComponent<Enemy>();
            }
        }
        return null;
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
