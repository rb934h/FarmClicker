using UnityEngine.SceneManagement;
using UnityEngine;

public static class SceneLoader
{
    public static AsyncOperation LoadMainMenuScene()
    {
        return SceneManager.LoadSceneAsync(0);
    }
    
    public static AsyncOperation LoadLevelScene()
    {
        return SceneManager.LoadSceneAsync(1);
    }
 
}