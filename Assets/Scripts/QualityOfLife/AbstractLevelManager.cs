using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class AbstractLevelManager : MonoBehaviour
{
    public static void GoToNextLevel(int currentSceneIndex)
    {
        if (!GetSceneIsValid(currentSceneIndex + 1))
        { Debug.Log("Scene by build index " + (currentSceneIndex + 1) + " can not be found."); return; }

        SceneManager.LoadScene(currentSceneIndex + 1);
    }
    public static void GoToPreviousLevel(int currentSceneIndex)
    {
        if (!GetSceneIsValid(currentSceneIndex - 1))
        { Debug.Log("Scene by build index " + (currentSceneIndex - 1) + " can not be found."); return; }

        SceneManager.LoadScene(currentSceneIndex - 1);
    }
    public static void GoToLevel(int index)
    {
        if (!GetSceneIsValid(index))
        { Debug.Log("Scene by build index " + index + " can not be found."); return; }

        SceneManager.LoadScene(index);
    }

    public static bool GetSceneIsValid(int buildIndex) => -1 != SceneUtility.GetBuildIndexByScenePath(SceneUtility.GetScenePathByBuildIndex(buildIndex));
    public static bool GetSceneIsValid(string scenePath) => -1 != SceneUtility.GetBuildIndexByScenePath(scenePath);
}
