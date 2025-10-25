using UnityEngine;
using System.Collections.Generic;

public class BoardDisplay : MonoBehaviour
{
    public GameObject enemyPrefab;
    private GameObject[] enemies;
    public Transform[] boardSlots;

    public void setEnemy(Enemy enemy, int slot) {
        GameObject enemyDisplay = Instantiate(enemyPrefab, boardSlots[slot].transform.position, Quaternion.identity,  boardSlots[slot].transform);
        enemyDisplay.GetComponent<EnemyDisplay>().enemy = enemy;
        enemies[slot] = enemyDisplay;    
    }

    void Start() {
        enemies = new GameObject[3];
    }
}
