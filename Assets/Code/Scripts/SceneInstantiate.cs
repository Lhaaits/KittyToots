using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Remember to add Managers Scene to build settings
/// </summary>
public class SceneInstantiate : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadSceneAsync("Managers", LoadSceneMode.Additive);
    }

    public static void ReloadScene()
    {
        // SceneManager.UnloadSceneAsync("GameScene");
        // SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("GameScene");
    }
}
