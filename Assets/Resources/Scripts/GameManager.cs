using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameData GameData;
    public void LoadScene(string sceneName)
    {
        GameData.lastLoadedScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public void ExitSettingsScene() {
        GameData.lastLoadedScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(GameData.lastLoadedScene);
    }

}
