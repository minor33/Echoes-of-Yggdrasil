using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void startNewRun() {
        SceneManager.LoadScene("BattleScene");
    }
}
