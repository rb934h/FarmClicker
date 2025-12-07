using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

public static class SceneLoader
{
    public static async UniTask LoadMainMenuScene()
    {
        await SceneManager.LoadSceneAsync(0);
    }
    
    public static async UniTask LoadLevelScene()
    {
        await SceneManager.LoadSceneAsync(1);
    }
 
}