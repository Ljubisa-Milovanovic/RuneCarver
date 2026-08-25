using QFSW.QC;
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
        Debug.Log(GameData.lastLoadedScene);
        //GameData.lastLoadedScene = SceneManager.GetActiveScene().name;
        Debug.Log(GameData.lastLoadedScene);
        SceneManager.LoadScene(GameData.lastLoadedScene);
    }

    [Command]
    public void LastScene() {
    
        Debug.Log(GameData.lastLoadedScene);
    }

}
